using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhiteMagic;

namespace Itchy
{
    public class D2Game
    {
        public Process Process { get { return process; } }
        public ProcessDebugger Debugger { get { return pd; } }
        public Thread Thread { get { return th; } }
        public bool Installed { get { return pd != null; } }
        public OverlayWindow Overlay { get { return overlay; } }
        public Itchy Itchy { get { return itchy; } }

        protected Process process = null;
        protected ProcessDebugger pd = null;
        protected Thread th = null;
        protected volatile OverlayWindow overlay = null;
        protected Itchy itchy;

        protected List<uint> revealedLevels = new List<uint>();

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
                //var bp = new LightBreakPoint(0x233A7, 1, HardwareBreakPoint.Condition.Code);
                //pd.AddBreakPoint("d2client.dll", bp);

                // 6FB332FF
                var bp = new ReceivePacketBreakPoint(0x832FF, 1, HardwareBreakPoint.Condition.Code);
                pd.AddBreakPoint("d2client.dll", bp);
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
                SuspendProcess();
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
                ResumeProcess();
        }

        public void Test()
        {
            /*var dt = DateTime.Now.Ticks;

            SuspendProcess();
            for (var i = 0; i < 500; ++i)
            {
                UnitAny u;
                GetPlayerUnit(out u);
            }
            ResumeProcess();

            MessageBox.Show(((DateTime.Now.Ticks - dt) / TimeSpan.TicksPerMillisecond).ToString());*/
            //WinApi.BlockInput(true);
            RevealAct();
            //WinApi.BlockInput(false);
            //RevealLevel();
            //OpenStash();
            //OpenCube();
            //SpeedTest();
        }

        protected int[] m_ActLevels = new int[]
        {
            1, 40, 75, 103, 109, 137
        };

        public void RevealAct()
        {
            UnitAny unit;
            if (!GetPlayerUnit(out unit))
            {
                //MessageBox.Show("Failed to get player unit");
                return;
            }

            if (unit.pAct == 0)
            {
                //MessageBox.Show("Failed to get act");
                return;
            }

            var act = pd.MemoryHandler.Read<Act>(unit.pAct);
            var expCharFlag = pd.MemoryHandler.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pExpCharFlag);
            var diff = pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.GetDifficulty,
                CallingConventionEx.StdCall);

            var pAct = pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.LoadAct,
                CallingConventionEx.StdCall,
                unit.dwAct,
                act.dwMapSeed,
                expCharFlag,
                0,
                diff,
                0,
                (uint)m_ActLevels[unit.dwAct],
                pd.GetModuleAddress("d2client.dll") + D2Client.LoadAct_1,
                pd.GetModuleAddress("d2client.dll") + D2Client.LoadAct_2);

            if (pAct == 0)
            {
                //MessageBox.Show("Failed to load act");
                return;
            }

            act = pd.MemoryHandler.Read<Act>(pAct);
            if (act.pMisc == 0)
            {
                //MessageBox.Show("Failed to get act misc");
                return;
            }

            var actMisc = pd.MemoryHandler.Read<ActMisc>(act.pMisc);
            if (actMisc.pLevelFirst == 0)
            {
                //MessageBox.Show("Failed to get act misc level first");
                return;
            }

            for (var i = m_ActLevels[unit.dwAct]; i < m_ActLevels[unit.dwAct + 1]; ++i)
            {
                Level lvl = pd.MemoryHandler.Read<Level>(actMisc.pLevelFirst);
                uint pLevel = 0;
                for (pLevel = actMisc.pLevelFirst; pLevel != 0; pLevel = lvl.pNextLevel)
                {
                    if (pLevel != actMisc.pLevelFirst)
                        lvl = pd.MemoryHandler.Read<Level>(pLevel);

                    if (lvl.dwLevelNo == (uint)i && lvl.dwPosX > 0)
                        break;
                }

                if (pLevel == 0)
                    pLevel = pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetLevel,
                        CallingConventionEx.FastCall,
                        act.pMisc, (uint)i);
                if (pLevel == 0)
                    continue;

                lvl = pd.MemoryHandler.Read<Level>(pLevel);
                if (lvl.pRoom2First == 0)
                    pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.InitLevel,
                        CallingConventionEx.StdCall,
                        pLevel);

                if (lvl.dwLevelNo > 255)
                    continue;

                InitLayer(lvl.dwLevelNo);
                lvl = pd.MemoryHandler.Read<Level>(pLevel);

                for (var pRoom = lvl.pRoom2First; pRoom != 0; )
                {
                    var room = pd.MemoryHandler.Read<Room2>(pRoom);

                    var actMisc2 = pd.MemoryHandler.Read<ActMisc>(lvl.pMisc);
                    var roomData = false;
                    if (room.pRoom1 == 0)
                    {
                        roomData = true;
                        pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.AddRoomData,
                            CallingConventionEx.ThisCall,
                            0, actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);
                    }

                    room = pd.MemoryHandler.Read<Room2>(pRoom);
                    if (room.pRoom1 == 0)
                        continue;

                    var pAutomapLayer = pd.MemoryHandler.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pAutoMapLayer);

                    pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.RevealAutomapRoom,
                        CallingConventionEx.StdCall,
                        room.pRoom1,
                        1,
                        pAutomapLayer);

                    if (roomData)
                        pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.RemoveRoomData,
                            CallingConventionEx.StdCall,
                            actMisc2.pAct, lvl.dwLevelNo, room.dwPosX, room.dwPosY, room.pRoom1);

                    pRoom = room.pRoom2Next;
                }
            }

            var path = pd.MemoryHandler.Read<Path>(unit.pPath);
            var room1 = pd.MemoryHandler.Read<Room1>(path.pRoom1);
            var room2 = pd.MemoryHandler.Read<Room2>(room1.pRoom2);
            var lev = pd.MemoryHandler.Read<Level>(room2.pLevel);
            InitLayer(lev.dwLevelNo);
            pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.UnloadAct,
                CallingConventionEx.StdCall,
                pAct);

            PrintGameString("Revealed act", D2Color.Red);

            Log("Revealed act");

            //ResumeProcess();
        }

        public void InitLayer(uint levelNo)
        {
            var pLayer = pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetLayer,
                CallingConventionEx.FastCall,
                levelNo);
            if (pLayer == 0)
                return;

            var layer = pd.MemoryHandler.Read<AutomapLayer2>(pLayer);

            pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.InitAutomapLayer_I,
                CallingConventionEx.Register,
                layer.nLayerNo);
        }

        public void OpenStash()
        {
            SuspendProcess();

            var packet = new byte[] { 0x77, 0x10 };
            var addr = pd.MemoryHandler.AllocateBytes(packet);
            pd.MemoryHandler.Call(pd.GetModuleAddress("d2net.dll") + D2Net.ReceivePacket,
                CallingConventionEx.StdCall,
                addr, (uint)packet.Length);
            ResumeProcess();
        }

        public void OpenCube()
        {
            SuspendProcess();

            var packet = new byte[] { 0x77, 0x15 };
            var addr = pd.MemoryHandler.AllocateBytes(packet);
            pd.MemoryHandler.Call(pd.GetModuleAddress("d2net.dll") + D2Net.ReceivePacket,
                CallingConventionEx.StdCall,
                addr, (uint)packet.Length);
            ResumeProcess();
        }

        private void SuspendProcess()
        {
            pd.MemoryHandler.SuspendAllThreads();
        }

        private void ResumeProcess()
        {
            pd.MemoryHandler.ResumeAllThreads();
        }

        private void Log(string message)
        {
            var time = DateTime.Now;
            var str = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", time.Hour, time.Minute, time.Second);
            this.overlay.logTextBox.AppendLine(str + message, Color.Empty);
        }
    }
}
