using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using DD.AutoTeleport;
using DD.D2Enums;
using DD.Log;
using WhiteMagic;
using WhiteMagic.WinAPI;
using DD.D2Pointers;
using DD.D2Structs;
using DD.Settings;
using DD.Breakpoints;

namespace DD
{
    public enum MessageEvent : int
    {
        WM_KEYDOWN = 0x100,
        WM_KEYUP = 0x101,
        WM_LBUTTONDOWN = 0x201,
        WM_LBUTTONUP = 0x202,
        WM_HOTKEY = 0x312,
    }

    public partial class D2Game : IDisposable
    {
        public Process Process { get { return process; } }
        public ProcessDebugger Debugger { get { return pd; } }
        public bool Installed { get { return pd != null; } }
        public OverlayWindow Overlay { get { return overlay; } }
        public DirtyDeeds DirtyDeeds { get { return dirtyDeeds; } }

        public GameSettings Settings
        {
            get { return dirtyDeeds.Settings; }
            set { dirtyDeeds.Settings = value; }
        }

        public OverlaySettings OverlaySettings
        {
            get { return dirtyDeeds.OverlaySettings; }
        }

        public ItemProcessingSettings ItemProcessingSettings { get { return dirtyDeeds.ItemProcessingSettings; } }

        protected volatile Process process = null;
        protected volatile ProcessDebugger pd = null;
        protected volatile OverlayWindow overlay = null;
        protected volatile DirtyDeeds dirtyDeeds;

        public volatile bool backToTown = false;
        public volatile uint viewingUnit = 0;

        public volatile bool hasPendingTask = false;

        public volatile ConcurrentDictionary<uint, uint> socketsPerItem = new ConcurrentDictionary<uint, uint>();
        public volatile ConcurrentDictionary<uint, uint> pricePerItem = new ConcurrentDictionary<uint, uint>();

        protected Thread syncThread = null;
        protected Thread gameCheckThread = null;

        public int MainThreadId { get { return mainThreadId; } }
        protected int mainThreadId = 0;

        public D2Game() { }

        public void Dispose()
        {
            if (!Installed)
                return;

            Detach();
        }

        public D2Game(Process process, DirtyDeeds dirtyDeeds)
        {
            this.process = process;
            this.dirtyDeeds = dirtyDeeds;
        }

        public override string ToString()
        {
            return process.MainModule.FileVersionInfo.InternalName + " - " + process.Id + (Installed ? " " + PlayerName : "");
        }

        public bool Exists()
        {
            if (pd != null && pd.HasExited)
                return false;

            if (process.HasExited)
                return false;

            process.Refresh();

            if (process.HasExited)
                return false;

            return true;
        }

        public bool Install()
        {
            if (!Exists())
                return false;

            try
            {
                pd = new ProcessDebugger(process.Id);
                pd.Run();

                var now = DateTime.Now;
                while (!pd.WaitForComeUp(50) && now.MSecToNow() < 1000)
                { }

                foreach (ProcessThread pT in process.Threads)
                    if (pd.GetThreadStartAddress(pT.Id) == pd.GetAddress(Game.mainThread))
                    {
                        mainThreadId = pT.Id;
                        break;
                    }

                ApplySettings();
            }
            catch
            {
                return false;
            }

            syncThread = new Thread(() => WindowChecker(this));
            syncThread.Start();

            gameCheckThread = new Thread(() => GameChecker(this));
            gameCheckThread.Start();

            CheckInGame(true);


            overlay = new OverlayWindow();
            overlay.game = this;
            overlay.PostCreate();
            overlay.InGameStateChanged(InGame);

            pickit = new Pickit(this);
            PlayerInfo = new PlayerInfo(this);
            MapHandler = new MapHandler(this);
            AutoTeleport = new AutoTeleHandler(this);

            return true;
        }

        public bool Detach()
        {
            if (overlay != null)
            {
                overlay.Close();
                overlay.Dispose();
                overlay = null;
            }

            if (syncThread != null)
            {
                syncThread.Abort();
                syncThread = null;
            }
            if (gameCheckThread != null)
            {
                gameCheckThread.Abort();
                gameCheckThread = null;
            }

            if (pickit != null)
            {
                pickit.Stop();
                pickit = null;
            }

            if (MapHandler != null)
            {
                MapHandler.Reset();
                MapHandler = null;
            }

            if (AutoTeleport != null)
            {
                AutoTeleport.Terminate();
                AutoTeleport = null;
            }

            PlayerInfo = null;

            if (Installed)
            {
                try
                {
                    pd.StopDebugging();
                    pd.Join();
                    pd = null;
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        protected static void WindowChecker(D2Game game)
        {
            while (true)
            {
                try
                {
                    if (game.Debugger.HasExited)
                    {
                        game.Detach();
                        return;
                    }

                    if (game.Overlay != null)
                    {
                        game.Overlay.Invoke((MethodInvoker)delegate
                        {
                            game.Overlay.UpdateOverlay();
                        });
                    }
                }
                catch
                {
                    //MessageBox.Show(e.ToString());
                }
                Thread.Sleep(300);
            }
        }

        protected static void GameChecker(D2Game g)
        {
            while (true)
            {
                try
                {
                    g.CheckInGame();
                }
                catch
                {
                    //MessageBox.Show(e.ToString());
                }
                Thread.Sleep(300);
            }
        }

        public volatile bool InGame = false;
        public volatile ushort CurrentX = 0;
        public volatile ushort CurrentY = 0;

        public volatile PlayerInfo PlayerInfo;
        public volatile MapHandler MapHandler;
        public volatile AutoTeleHandler AutoTeleport;

        private void CheckInGame(bool first = false)
        {
            var pPlayer = GetPlayerUnit();
            var check = pPlayer != 0;

            try
            {
                if (check)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        //RefreshUnitPosition();
                    }
                }

                if (check == InGame && !first)
                    return;

                if (check)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        PlayerName = GetPlayerName();
                    }
                }
                else
                    PlayerName = "";

                InGame = check;

                if (InGame)
                    EnteredGame();
                else
                    ExitedGame();

                try
                {
                    overlay.Invoke((MethodInvoker)delegate
                    {
                        overlay.InGameStateChanged(InGame);
                    });
                }
                catch { }
            }
            catch { }
        }

        public void Test()
        {
            /*var val = llGetUnitStat(GetPlayerUnit(), StatType.MaxHealth);
            val >>= 8;
            this.PrintGameString(val.ToString());*/

            using (var suspender = new GameSuspender(this))
            {
                Thread.Sleep(15000);
            }
        }

        public void ResumeStormThread()
        {
            lock ("moduleLoad")
            {
                var hModule = Kernel32.GetModuleHandle("kernel32.dll");
                if (hModule == 0)
                    hModule = Kernel32.LoadLibraryA("kernel32.dll");
                if (hModule == 0)
                    throw new DebuggerException("Failed to get kernel32.dll module");

                var funcAddress = Kernel32.GetProcAddress(hModule, "ResumeThread");

                var handle = pd.ReadUInt(Storm.pHandle);

                pd.Call(pd.GetModuleAddress("kernel32.dll") + funcAddress - hModule, CallingConventionEx.StdCall, handle);
            }
        }

        /*public void Log(string message, Color color, params object[] args)
        {
            this.overlay.logTextBox.LogLine(message, color, args);
        }

        public void Log(string message, params object[] args)
        {
            Log(message, Color.Empty, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Log(message, Color.Red, args);
        }*/

        public void ExitedGame()
        {
            chickening = false;
            backToTown = false;
            pricePerItem.Clear();
            socketsPerItem.Clear();
            viewingUnit = 0;

            if (pickit != null)
                pickit.Reset();

            if (PlayerInfo != null)
                PlayerInfo.Reset();

            if (AutoTeleport != null)
                AutoTeleport.Reset();
        }

        public void EnteredGame()
        {
            if (MapHandler != null)
                MapHandler.Reset();
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

            var pSelected = pd.Call(D2Client.GetSelectedUnit,
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
            return pd.ReadUInt(D2Client.pMouseX);
        }

        public uint GetMouseY()
        {
            return pd.ReadUInt(D2Client.pMouseY);
        }

        public bool HandleMessage(Keys key, MessageEvent mEvent)
        {
            //Console.WriteLine(mEvent.ToString() + " " + vkCode.ToString());
            //Log(mEvent.ToString() + " " + key.ToString());

            if (key == Keys.LControlKey || key == Keys.RControlKey)
            {
                if (mEvent == MessageEvent.WM_KEYUP && !overlay.ClickThrough && !overlay.settingsExpandButton.Expanded)
                        overlay.MakeNonInteractive(true);
                    else if (mEvent == MessageEvent.WM_KEYDOWN && overlay.ClickThrough)
                        overlay.MakeNonInteractive(false);
            }

            if (mEvent == MessageEvent.WM_KEYUP && overlay.settingsExpandButton.Expanded)
                if (!overlay.HandleMessage(key, mEvent))
                    return false;

            if (mEvent == MessageEvent.WM_KEYUP && GetUIVar(UIVars.ChatInput) == 0)
            {
                if (key == Settings.FastExit.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        ExitGame();
                    }
                }
                if (key == Settings.OpenCube.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        OpenCube();
                    }
                }
                if (key == Settings.OpenStash.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        OpenStash();
                    }
                }
                if (key == Settings.RevealAct.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        MapHandler.RevealAct();
                        //ItemStorage.LoadCodes(this);
                        //Test();
                    }
                }
                if (key == Settings.FastPortal.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        if (OpenPortal() && Settings.FastPortal.GoToTown && Settings.ReceivePacketHack.Enabled)
                            backToTown = true;
                    }
                }

                if (key == Settings.ViewInventory.ViewInventoryKey && Settings.ViewInventory.Enabled)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        OnViewInventoryKey();
                    }
                }

                if (key == Settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key &&
                    Settings.ReceivePacketHack.ItemTracker.Enabled &&
                    Settings.ReceivePacketHack.ItemTracker.EnablePickit)
                {
                    if (pickit != null)
                        pickit.ToggleEnabled();
                }

                if (key == Settings.AutoteleNext.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        AutoTeleport.ManageTele(TeleType.Next);
                    }
                }

                if (key == Settings.AutoteleMisc.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        AutoTeleport.ManageTele(TeleType.Misc);
                    }
                }

                if (key == Settings.AutoteleWP.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        AutoTeleport.ManageTele(TeleType.WP);
                    }
                }

                if (key == Settings.AutotelePrev.Key)
                {
                    using (var suspender = new GameSuspender(this))
                    {
                        AutoTeleport.ManageTele(TeleType.Prev);
                    }
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
            if (pd.Breakpoints.Count >= Kernel32.MaxHardwareBreakpoints)
                return;

            try
            {
                pd.AddBreakPoint(bp, pd.GetModuleAddress(bp.ModuleName));
            }
            catch
            {
                Logger.Generic.Log(this, LogType.Warning, "Failed to apply hacks. Try again");
            }
        }

        public void ApplySettings()
        {
            if (!Exists())
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

            if (Settings.Infravision.Enabled)
                AddBreakPoint(new InfravisionBreakPoint(this));
        }
    }
}
