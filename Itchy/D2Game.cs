using System;
using System.Collections.Generic;
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

        public GameSettings Settings { get { return Itchy.Settings; } }

        protected Process process = null;
        protected ProcessDebugger pd = null;
        protected Thread th = null;
        protected volatile OverlayWindow overlay = null;
        protected Itchy itchy;

        protected List<uint> revealedActs = new List<uint>();

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

                // 0x6FD80C92 0x30C92
                //var bp2 = new RainBreakPoint(0x30C92, 1, HardwareBreakPoint.Condition.Code);
                //pd.AddBreakPoint("d2common.dll", bp2);

                // 0x6FAD33A7 0x233A7
                var bp = new LightBreakPoint(this, 0x233A7, 1, HardwareBreakPoint.Condition.Code);
                pd.AddBreakPoint("d2client.dll", bp);

                // 6FB332FF
                var bp2 = new ReceivePacketBreakPoint(this, 0x832FF, 1, HardwareBreakPoint.Condition.Code);
                pd.AddBreakPoint("d2client.dll", bp2);
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
            return pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.GetPlayerUnit,
                CallingConventionEx.StdCall);
        }

        public uint GetUIVar(UIVars uiVar)
        {
            if (uiVar > UIVars.Max)
                return 0;

            return pd.MemoryHandler.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pUiVars + (uint)uiVar * 4);
        }

        public bool GetPlayerUnit(out UnitAny unit)
        {
            var pUnit = GetPlayerUnit();
            if (pUnit == 0)
            {
                unit = new UnitAny();
                return false;
            }

            unit = pd.MemoryHandler.Read<UnitAny>(pUnit);
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

                var data = pd.MemoryHandler.Read<PlayerData>(unit.pPlayerData);
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
                var addr = pd.MemoryHandler.AllocateUTF16String(str);
                pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.PrintGameString,
                    CallingConventionEx.StdCall,
                    addr, (uint)color);

                pd.MemoryHandler.FreeMemory(addr);
            }
            catch (Exception)
            {
            }

            if (suspend)
                ResumeThreads();
        }

        public void Test()
        {
            RevealAct();
        }

        public void ResumeStormThread()
        {
            var hModule = WinApi.GetModuleHandle("kernel32.dll");
            if (hModule == 0)
                hModule = WinApi.LoadLibraryA("kernel32.dll");
            if (hModule == 0)
                throw new DebuggerException("Failed to get kernel32.dll module");

            var funcAddress = WinApi.GetProcAddress(hModule, "ResumeThread");

            var handle = pd.MemoryHandler.ReadUInt(pd.GetModuleAddress("storm.dll") + Storm.pHandle);

            pd.MemoryHandler.Call(pd.GetModuleAddress("kernel32.dll") + funcAddress - hModule, CallingConventionEx.StdCall, handle);
        }

        public void OpenStash()
        {
            SuspendThreads();

            var packet = new byte[] { 0x77, 0x10 };
            var addr = pd.MemoryHandler.AllocateBytes(packet);
            pd.MemoryHandler.Call(pd.GetModuleAddress("d2net.dll") + D2Net.ReceivePacket,
                CallingConventionEx.StdCall,
                addr, (uint)packet.Length);
            ResumeThreads();
        }

        public void OpenCube()
        {
            SuspendThreads();

            var packet = new byte[] { 0x77, 0x15 };
            var addr = pd.MemoryHandler.AllocateBytes(packet);
            pd.MemoryHandler.Call(pd.GetModuleAddress("d2net.dll") + D2Net.ReceivePacket,
                CallingConventionEx.StdCall,
                addr, (uint)packet.Length);
            ResumeThreads();
        }

        public void ExitGame()
        {
            if (GetPlayerUnit() == 0)
                return;

            pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.ExitGame,
                CallingConventionEx.FastCall);
        }

        private void SuspendThreads(params int[] except)
        {
            pd.MemoryHandler.SuspendAllThreads(except);
        }

        private void ResumeThreads()
        {
            pd.MemoryHandler.ResumeAllThreads();
        }

        public void Log(string message, params object[] args)
        {
            message = string.Format(message, args);

            var time = DateTime.Now;
            var str = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", time.Hour, time.Minute, time.Second);
            this.overlay.logTextBox.AppendLine(str + message, Color.Empty);
        }

        public void ExitedGame()
        {
        }

        public void EnteredGame()
        {
            revealedActs.Clear();
        }


        enum MessageEvent : int
        {
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101,
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
                if (mEvent == MessageEvent.WM_KEYUP && !overlay.ClickThrough)
                        overlay.MakeNonInteractive(true);
                    else if (mEvent == MessageEvent.WM_KEYDOWN && overlay.ClickThrough)
                        overlay.MakeNonInteractive(false);
            }

            if (mEvent == MessageEvent.WM_KEYUP && GetUIVar(UIVars.ChatInput) == 0)
            {
                if (vkCode == Settings.FastExitKey)
                {
                    SuspendThreads();
                    ExitGame();
                    ResumeThreads();
                }
                if (vkCode == Settings.OpenCubeKey)
                {
                    OpenCube();
                }
                if (vkCode == Settings.OpenStashKey)
                {
                    OpenStash();
                }
                if (vkCode == Settings.RevealActKey)
                {
                    RevealAct();
                }
            }

            return true;
        }
    }
}
