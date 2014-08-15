using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhiteMagic;

using System.Xml.Serialization;

namespace Itchy
{
    public partial class D2Game
    {
        public Process Process { get { return process; } }
        public ProcessDebugger Debugger { get { return pd; } }
        public Thread Thread { get { return th; } }
        public bool Installed { get { return pd != null; } }
        public OverlayWindow Overlay { get { return overlay; } }
        public Itchy Itchy { get { return itchy; } }
        public ItemStorage ItemStorage { get { return itchy.ItemStorage; } }

        public GameSettings Settings
        {
            get { return itchy.Settings; }
            set { itchy.Settings = value; }
        }

        public ItemDisplaySettings ItemSettings { get { return itchy.ItemSettings; } }

        protected Process process = null;
        protected ProcessDebugger pd = null;
        protected Thread th = null;
        protected volatile OverlayWindow overlay = null;
        protected Itchy itchy;

        protected List<uint> revealedActs = new List<uint>();
        public volatile bool backToTown = false;
        public volatile uint viewingUnit = 0;

        public volatile bool hasPendingTask = false;

        public volatile ConcurrentDictionary<uint, uint> socketsPerItem = new ConcurrentDictionary<uint, uint>();
        public volatile ConcurrentDictionary<uint, uint> pricePerItem = new ConcurrentDictionary<uint, uint>();

        private int threadSuspendCount = 0;

        public D2Game() { }
        public D2Game(Process process, Itchy itchy)
        {
            this.process = process;
            this.itchy = itchy;
        }

        public override string ToString()
        {
            return process.MainModule.FileVersionInfo.InternalName + " - " + process.Id + (Installed ? " (*) " + PlayerName : "");
        }

        public bool Exists()
        {
            process.Refresh();

            if (process.HasExited)
                return false;

            return true;
        }

        public bool Install()
        {
            process.Refresh();
            if (process.HasExited)
                return false;

            try
            {
                pd = new ProcessDebugger(process.Id);
                th = ProcessDebugger.Run(ref pd);

                if (!pd.WaitForComeUp(500))
                    return false;

                ApplySettings();
            }
            catch (Exception)
            {
                return false;
            }

            overlay = new OverlayWindow();
            overlay.game = this;
            overlay.Show();

            return true;
        }

        public bool Detach()
        {
            process.Refresh();
            if (process.HasExited)
                return true;

            try
            {
                pd.StopDebugging();
                th.Join();
                pd = null;
            }
            catch (Exception)
            {
                return false;
            }

            overlay.Dispose();
            overlay = null;

            return true;
        }

        public uint GetPlayerUnit()
        {
            /*return pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.GetPlayerUnit,
                CallingConventionEx.StdCall);*/
            return pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pPlayerUnit);
        }

        public uint GetUIVar(UIVars uiVar)
        {
            if (uiVar > UIVars.Max)
                return 0;

            return pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pUiVars + (uint)uiVar * 4);
        }

        public void SetUIVar(UIVars uiVar, uint value)
        {
            if (uiVar > UIVars.Max)
                return;

            pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.SetUiVar,
                CallingConventionEx.FastCall,
                (uint)uiVar,
                value,
                0);
        }

        public bool GetPlayerUnit(out UnitAny unit)
        {
            var pUnit = GetPlayerUnit();
            if (pUnit == 0)
            {
                unit = new UnitAny();
                return false;
            }

            unit = pd.Read<UnitAny>(pUnit);
            return true;
        }

        public string PlayerName { get; set; }

        public string GetPlayerName()
        {
            try
            {
                UnitAny unit;
                if (!GetPlayerUnit(out unit))
                    return "";

                var data = pd.Read<PlayerData>(unit.pPlayerData);
                return Encoding.ASCII.GetString(data.szName);
            }
            catch (Exception)
            {
            }

            return "";
        }

        public void PrintGameString(string str, D2Color color = D2Color.Default, bool suspend = false)
        {
            if (suspend)
                SuspendThreads();
            try
            {
                var addr = pd.AllocateUTF16String(str);
                pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.PrintGameString,
                    CallingConventionEx.StdCall,
                    addr, (uint)color);

                pd.FreeMemory(addr);
            }
            catch (Exception)
            {
            }

            if (suspend)
                ResumeThreads();
        }

        public void Test()
        {
            //RevealAct();

            UnitAny unit;
            if (!GetPlayerUnit(out unit))
                return;

            SetUIVar(UIVars.Inventory, 0);

            //OpenPortal();
        }

        public void ResumeStormThread()
        {
            var hModule = WinApi.GetModuleHandle("kernel32.dll");
            if (hModule == 0)
                hModule = WinApi.LoadLibraryA("kernel32.dll");
            if (hModule == 0)
                throw new DebuggerException("Failed to get kernel32.dll module");

            var funcAddress = WinApi.GetProcAddress(hModule, "ResumeThread");

            var handle = pd.ReadUInt(pd.GetModuleAddress("storm.dll") + Storm.pHandle);

            pd.Call(pd.GetModuleAddress("kernel32.dll") + funcAddress - hModule, CallingConventionEx.StdCall, handle);
        }

        public void OpenStash()
        {
            SuspendThreads();

            var packet = new byte[] { 0x77, 0x10 };
            ReceivePacket(packet);

            ResumeThreads();
        }

        public void OpenCube()
        {
            SuspendThreads();

            var packet = new byte[] { 0x77, 0x15 };
            ReceivePacket(packet);

            ResumeThreads();
        }

        public void ExitGame()
        {
            if (GetPlayerUnit() == 0)
                return;

            pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.ExitGame,
                CallingConventionEx.FastCall);
        }

        public void SuspendThreads(params int[] except)
        {
            if (++threadSuspendCount > 1)
                return;

            pd.SuspendAllThreads(except);
        }

        public void ResumeThreads()
        {
            if (--threadSuspendCount > 0)
                return;

            pd.ResumeAllThreads();
        }

        public void Log(string message, params object[] args)
        {
            this.overlay.logTextBox.LogLine(message, Color.Empty, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            this.overlay.logTextBox.LogLine(message, Color.OrangeRed, args);
        }

        public void ExitedGame()
        {
            chickening = false;
            backToTown = false;
            pricePerItem.Clear();
            socketsPerItem.Clear();
            viewingUnit = 0;
        }

        public void EnteredGame()
        {
            revealedActs.Clear();
        }

        public uint GetViewingUnit()
        {
            var pPlayer = GetPlayerUnit();
            if (viewingUnit == 0)
                return pPlayer;

            if (GetUIVar(UIVars.Inventory) == 0)
            {
                viewingUnit = 0;
                return pPlayer;
            }

            var pUnit = FindServerSideUnit(viewingUnit, (uint)UnitType.Player);
            if (pUnit == 0)
            {
                viewingUnit = 0;
                return pPlayer;
            }

            var unit = pd.Read<UnitAny>(pUnit);
            if (unit.pInventory == 0)
            {
                viewingUnit = 0;
                return pPlayer;
            }

            return pUnit;
        }

        public void OnViewInventoryKey()
        {
            if (!GameReady())
                return;

            var pSelected = pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.GetSelectedUnit,
                CallingConventionEx.StdCall);

            if (pSelected == 0)
                return;

            var unit = pd.Read<UnitAny>(pSelected);
            if (unit.dwMode == 0 || unit.dwMode == 17 || unit.dwType != (uint)UnitType.Player)
                return;

            viewingUnit = unit.dwUnitId;
            if (GetUIVar(UIVars.Inventory) == 0)
                SetUIVar(UIVars.Inventory, 0);
        }

        public uint GetMouseX()
        {
            return pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pMouseX);
        }

        public uint GetMouseY()
        {
            return pd.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pMouseY);
        }

        enum MessageEvent : int
        {
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101,
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202,
            WM_HOTKEY = 0x312,
        }

        public bool HandleMessage(int code, IntPtr wParam, IntPtr lParam)
        {
            var mEvent = (MessageEvent)wParam.ToInt32();
            var vkCode = (Keys)Marshal.ReadInt32(lParam);

            //Console.WriteLine(mEvent.ToString() + " " + vkCode.ToString());
            Log(mEvent.ToString() + " " + vkCode.ToString());

            if (vkCode == Keys.LControlKey || vkCode == Keys.RControlKey)
            {
                if (mEvent == MessageEvent.WM_KEYUP && !overlay.ClickThrough && !overlay.propertiesExpandButton.Expanded)
                        overlay.MakeNonInteractive(true);
                    else if (mEvent == MessageEvent.WM_KEYDOWN && overlay.ClickThrough)
                        overlay.MakeNonInteractive(false);
            }

            if (mEvent == MessageEvent.WM_KEYUP && overlay.propertiesExpandButton.Expanded)
                return overlay.HandleMessage(code, wParam, lParam);

            if (mEvent == MessageEvent.WM_KEYUP && GetUIVar(UIVars.ChatInput) == 0)
            {
                if (vkCode == Settings.FastExit.Key)
                {
                    SuspendThreads();
                    ExitGame();
                    ResumeThreads();
                }
                if (vkCode == Settings.OpenCube.Key)
                {
                    OpenCube();
                }
                if (vkCode == Settings.OpenStash.Key)
                {
                    OpenStash();
                }
                if (vkCode == Settings.RevealAct.Key)
                {
                    RevealAct();
                }
                if (vkCode == Settings.FastPortal.Key)
                {
                    SuspendThreads();
                    if (OpenPortal() && Settings.FastPortal.GoToTown)
                        backToTown = true;
                    ResumeThreads();
                }

                if (vkCode == Settings.ViewInventory.ViewInventoryKey && Settings.ViewInventory.Enabled)
                {
                    SuspendThreads();
                    OnViewInventoryKey();
                    ResumeThreads();
                }
            }

            if (mEvent == MessageEvent.WM_LBUTTONDOWN && Settings.ViewInventory.Enabled)
            {
                if (viewingUnit != 0 && GameReady() && GetUIVar(UIVars.Inventory) != 0 && GetViewingUnit() != 0 &&
                    viewingUnit != 0 && GetMouseX() >= 400)
                    return false;
            }

            return true;
        }

        private void AddBreakPoint(D2BreakPoint bp)
        {
            pd.AddBreakPoint(bp, pd.GetModuleAddress(bp.ModuleName));
        }

        public void ApplySettings()
        {
            process.Refresh();
            if (process.HasExited)
                return;

            pd.RemoveBreakPoints();

            if (Settings.LightHack.Enabled)
                AddBreakPoint(new LightBreakPoint(this));

            if (Settings.WeatherHack.Enabled)
                AddBreakPoint(new WeatherBreakPoint(this));

            if (Settings.ReceivePacketHack.Enabled)
                AddBreakPoint(new ReceivePacketBreakPoint(this));

            if (Settings.ItemNameHack.Enabled)
                AddBreakPoint(new ItemNameBreakPoint(this));

            if (Settings.ViewInventory.Enabled)
            {
                AddBreakPoint(new ViewInventoryBp1(this));
                AddBreakPoint(new ViewInventoryBp2(this));
                AddBreakPoint(new ViewInventoryBp3(this));
            }
        }
    }
}
