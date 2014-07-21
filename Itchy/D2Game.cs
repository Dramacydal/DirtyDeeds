using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected Process process;
        protected ProcessDebugger pd;
        protected Thread th;

        public D2Game(Process process)
        {
            this.process = process;
        }

        public override string ToString()
        {
            return process.MainModule.FileVersionInfo.InternalName + " - " + process.Id + (Installed ? " (*)" : "");
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
            }
            catch (Exception)
            {

                return false;
            }

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

            return true;
        }

        public bool GetPlayerUnit(out UnitAny unit)
        {
            var pUnit = pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.GetPlayerUnit,
                CallingConventionEx.StdCall);

            if (pUnit == 0)
            {
                unit = new UnitAny();
                return false;
            }

            unit = pd.MemoryHandler.Read<UnitAny>(pUnit);
            return true;
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
            RevealAct();
        }

        public int[] m_ActLevels = new int[]
        {
            1, 10, 75, 103, 109, 137
        };

        private uint GetLevelPointer(uint pActMisc, int nLevel)
        {
            if (pActMisc == 0 || nLevel < 0)
                return 0;

            var actMist = pd.MemoryHandler.Read<ActMisc>(pActMisc);
            Level level;
            for (uint pLevel = actMist.pLevelFirst; pLevel != 0; pLevel = level.pNextLevel)
            {
                level = pd.MemoryHandler.Read<Level>(pLevel);
                if (level.dwLevelNo == nLevel && level.dwSizeX > 0)
                    return pLevel;
            }

            return pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetLevel,
                CallingConventionEx.FastCall,
                pActMisc, (uint)nLevel);
        }

        private void InitLevel(uint pLevel)
        {
            pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.InitLevel,
                CallingConventionEx.StdCall,
                pLevel);
        }

        private uint InitAutomapLayer(uint levelno)
        {
            var ret = pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetLayer,
                CallingConventionEx.FastCall,
                levelno);
            if (ret == 0)
                return 0;

            var layer = pd.MemoryHandler.Read<AutomapLayer2>(ret);
            ret = pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.InitAutomapLayer_I,
                CallingConventionEx.Register,
                layer.nLayerNo);

            return ret;
        }

        private void RevealLevel(Level lvl)
        {
            if (InitAutomapLayer(lvl.dwLevelNo) == 0)
                return;

            UnitAny unit;
            if (!GetPlayerUnit(out unit))
                return;

            var misc = pd.MemoryHandler.Read<ActMisc>(lvl.pMisc);
            var path = pd.MemoryHandler.Read<Path>(unit.pPath);
            var pLayer = pd.MemoryHandler.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pAutoMapLayer);

            var pRoom2 = lvl.pRoom2First;
            var roomCount = 0;
            for (; ; )
            {
                if (pRoom2 == 0)
                    break;

                ++roomCount;

                var room2 = pd.MemoryHandler.Read<Room2>(pRoom2);
                var added = false;
                
                if (room2.pRoom1 == 0)
                {
                    pRoom2 = room2.pRoom2Next;
                    continue;
                    pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.AddRoomData,
                        CallingConventionEx.StdCall,
                        misc.pAct, lvl.dwLevelNo, room2.dwPosX, room2.dwPosY, path.pRoom1);

                    added = true;
                    room2 = pd.MemoryHandler.Read<Room2>(pRoom2);
                }

                if (room2.pRoom1 == 0)
                {
                    pRoom2 = room2.pRoom2Next;
                    continue;
                }

                pd.MemoryHandler.Call(pd.GetModuleAddress("d2client.dll") + D2Client.RevealAutomapRoom,
                    CallingConventionEx.StdCall,
                    room2.pRoom1, 1, pLayer);

                //Drawpresets(...)

                if (added)
                {
                    pd.MemoryHandler.Call(pd.GetModuleAddress("d2common.dll") + D2Common.RemoveRoomData,
                        CallingConventionEx.StdCall,
                        misc.pAct, lvl.dwLevelNo, room2.dwPosX, room2.dwPosY, path.pRoom1);
                }

                pRoom2 = room2.pRoom2Next;
            }

            var path2 = pd.MemoryHandler.Read<Path>(unit.pPath);
            var room1 = pd.MemoryHandler.Read<Room1>(path2.pRoom1);
            var room22 = pd.MemoryHandler.Read<Room2>(room1.pRoom2);
            var lvl2 = pd.MemoryHandler.Read<Level>(room22.pLevel);

            InitAutomapLayer(lvl2.dwLevelNo);
        }

        public void RevealAct()
        {
            SuspendProcess();

            //string str ="";
            UnitAny unit;
            if (!GetPlayerUnit(out unit))
            {
                MessageBox.Show("Failed to get player unit");
                ResumeProcess();
                return;
            }

            //var lvl = pd.MemoryHandler.Read<Level>(unit

            var act = pd.MemoryHandler.Read<Act>(unit.pAct);
            for (var i = m_ActLevels[act.dwAct] + 1; i < m_ActLevels[act.dwAct + 1]; ++i)
            {
                var pLevel = GetLevelPointer(act.pMisc, i);
                if (pLevel == 0)
                    continue;

                var lvl = pd.MemoryHandler.Read<Level>(pLevel);
                if (lvl.pRoom2First == 0)
                    InitLevel(pLevel);
                lvl = pd.MemoryHandler.Read<Level>(pLevel);
                if (lvl.pRoom2First == 0)
                    continue;

                RevealLevel(lvl);

                //MessageBox.Show("Revealing " + i.ToString());
            }

            //MessageBox.Show(str);

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
    }
}
