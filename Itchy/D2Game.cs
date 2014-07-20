using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
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
            var addr = pd.BlackMagic.AllocateMemory(1024);

            pd.BlackMagic.Asm.Clear();
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2client.dll") + 0x613C0);
            pd.BlackMagic.Asm.AddLine("retn");
            var playerAddr = pd.BlackMagic.Asm.InjectAndExecute(addr);
            pd.BlackMagic.FreeMemory(addr);
            if (playerAddr == 0)
            {
                unit = new UnitAny();
                return false;
            }

            unit = (UnitAny)pd.BlackMagic.ReadObject(playerAddr, typeof(UnitAny));
            return true;
        }

        public void Test()
        {
            var m = new MemoryHandler(process);
            m.SuspendAllThreads();
            var addr2 = pd.BlackMagic.AllocateMemory(1024);
            if (addr2 == 0)
                MessageBox.Show("Fail #1");

            pd.BlackMagic.WriteUnicodeString(addr2, "lol!!");

            pd.BlackMagic.Asm.AddLine("push 7");
            pd.BlackMagic.Asm.AddLine("push {0}", addr2);
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2client.dll") + D2Client.PrintGameString);
            pd.BlackMagic.Asm.AddLine("retn");

            pd.BlackMagic.Asm.InjectAndExecute(addr2 + 50);

            if (!pd.BlackMagic.FreeMemory(addr2))
                MessageBox.Show("Fail #3");

            m.ResumeAllThreads();



            return;
            var addr = pd.BlackMagic.AllocateMemory(1024);

            UnitAny unit;
            if (GetPlayerUnit(out unit))
                MessageBox.Show(unit.dwAct.ToString());

            RevealAct();
        }

        public int[] m_ActLevels = new int[]
        {
            1, 40, 75, 103, 109, 137
        };

        private uint GetLevelPointer(uint pActMisc, int nLevel)
        {
            if (pActMisc == 0 || nLevel < 0)
                return 0;

            ActMisc actMist = (ActMisc)pd.BlackMagic.ReadObject(pActMisc, typeof(ActMisc));
            Level level;
            for (uint pLevel = actMist.pLevelFirst; pLevel != 0; pLevel = level.pNextLevel)
            {
                level = (Level)pd.BlackMagic.ReadObject(pLevel, typeof(Level));
                if (level.dwLevelNo == nLevel && level.dwSizeX > 0)
                    return pLevel;
            }

            var addr = pd.BlackMagic.AllocateMemory(1024);
            pd.BlackMagic.Asm.Clear();
            pd.BlackMagic.Asm.AddLine("mov ecx, {0}", pActMisc);
            pd.BlackMagic.Asm.AddLine("mov edx, {0}", nLevel);
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2common.dll") + D2Common.GetLevel);
            pd.BlackMagic.Asm.AddLine("retn");

            var ret = pd.BlackMagic.Asm.InjectAndExecute(addr);
            pd.BlackMagic.FreeMemory(addr);
            return ret;
        }

        private void InitLevel(uint pLevel)
        {
            var addr = pd.BlackMagic.AllocateMemory(1024);
            pd.BlackMagic.Asm.Clear();
            pd.BlackMagic.Asm.AddLine("push {0}", pLevel);
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2common.dll") + D2Common.InitLevel);
            pd.BlackMagic.Asm.AddLine("retn");
            pd.BlackMagic.Asm.InjectAndExecute(addr);
        }

        private uint InitAutomapLayer(uint levelno)
        {
            var addr = pd.BlackMagic.AllocateMemory(1024);
            pd.BlackMagic.Asm.Clear();
            pd.BlackMagic.Asm.AddLine("mov eax, {0}", levelno);
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2common.dll") + D2Common.GetLayer);
            pd.BlackMagic.Asm.AddLine("retn");
            var ret = pd.BlackMagic.Asm.InjectAndExecute(addr);
            pd.BlackMagic.FreeMemory(addr);
            if (ret == 0)
                return 0;

            AutomapLayer2 layer = (AutomapLayer2)pd.BlackMagic.ReadObject(ret, typeof(AutomapLayer2));
            addr = pd.BlackMagic.AllocateMemory(1024);
            pd.BlackMagic.Asm.Clear();
            pd.BlackMagic.Asm.AddLine("mov eax, {0}", layer.nLayerNo);
            pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2client.dll") + D2Client.InitAutomapLayer_I);
            pd.BlackMagic.Asm.AddLine("retn");
            ret = pd.BlackMagic.Asm.InjectAndExecute(addr);
            pd.BlackMagic.FreeMemory(addr);

            return ret;
        }

        private void RevealLevel(Level lvl)
        {
            if (InitAutomapLayer(lvl.dwLevelNo) == 0)
                return;

            UnitAny unit;
            if (!GetPlayerUnit(out unit))
                return;

            uint pRoom2 = lvl.pRoom2First;
            for (; ; )
            {
                if (pRoom2 == 0)
                    break;

                Room2 room2 = (Room2)pd.BlackMagic.ReadObject(pRoom2, typeof(Room2));
                var added = false;

                ActMisc misc = new ActMisc();
                Path path = new Path();
                if (room2.pRoom1 == 0)
                {
                    misc = (ActMisc)pd.BlackMagic.ReadObject(lvl.pMisc, typeof(ActMisc));
                    path = (Path)pd.BlackMagic.ReadObject(unit.pPath, typeof(Path));

                    var addr = pd.BlackMagic.AllocateMemory(1024);
                    pd.BlackMagic.Asm.Clear();
                    pd.BlackMagic.Asm.AddLine("push {0}", path.pRoom1);
                    pd.BlackMagic.Asm.AddLine("push {0}", room2.dwPosY);
                    pd.BlackMagic.Asm.AddLine("push {0}", room2.dwPosX);
                    pd.BlackMagic.Asm.AddLine("push {0}", lvl.dwLevelNo);
                    pd.BlackMagic.Asm.AddLine("push {0}", misc.pAct);
                    pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2common.dll") + D2Common.AddRoomData);
                    pd.BlackMagic.Asm.AddLine("retn");
                    pd.BlackMagic.Asm.InjectAndExecute(addr);
                    pd.BlackMagic.FreeMemory(addr);

                    added = true;
                }

                room2 = (Room2)pd.BlackMagic.ReadObject(pRoom2, typeof(Room2));
                if (room2.pRoom1 == 0)
                    continue;

                var addr2 = pd.BlackMagic.AllocateMemory(1024);
                pd.BlackMagic.Asm.Clear();
                var pLayer = pd.BlackMagic.ReadUInt(pd.GetModuleAddress("d2client.dll") + D2Client.pAutoMapLayer);
                pd.BlackMagic.Asm.AddLine("push {0}", pLayer);
                pd.BlackMagic.Asm.AddLine("push {0}", 1);
                pd.BlackMagic.Asm.AddLine("push {0}", room2.pRoom1);
                pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2client.dll") + D2Client.RevealAutomapRoom);
                pd.BlackMagic.Asm.AddLine("retn");
                pd.BlackMagic.Asm.InjectAndExecute(addr2);
                pd.BlackMagic.FreeMemory(addr2);

                //Drawpresets(...)

                if (added)
                {
                    var addr = pd.BlackMagic.AllocateMemory(1024);
                    pd.BlackMagic.Asm.Clear();
                    pd.BlackMagic.Asm.AddLine("push {0}", path.pRoom1);
                    pd.BlackMagic.Asm.AddLine("push {0}", room2.dwPosY);
                    pd.BlackMagic.Asm.AddLine("push {0}", room2.dwPosX);
                    pd.BlackMagic.Asm.AddLine("push {0}", lvl.dwLevelNo);
                    pd.BlackMagic.Asm.AddLine("push {0}", misc.pAct);
                    pd.BlackMagic.Asm.AddLine("call {0}", pd.GetModuleAddress("d2common.dll") + D2Common.RemoveRoomData);
                    pd.BlackMagic.Asm.AddLine("retn");
                    pd.BlackMagic.Asm.InjectAndExecute(addr);
                    pd.BlackMagic.FreeMemory(addr);
                }

                pRoom2 = room2.pRoom2Next;
            }

            Path path2 = (Path)pd.BlackMagic.ReadObject(unit.pPath, typeof(Path));
            Room1 room1 = (Room1)pd.BlackMagic.ReadObject(path2.pRoom1, typeof(Room1));
            Room2 room22 = (Room2)pd.BlackMagic.ReadObject(room1.pRoom2, typeof(Room2));
            Level lvl2 = (Level)pd.BlackMagic.ReadObject(room22.pLevel, typeof(Level));

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

            Act act = (Act)pd.BlackMagic.ReadObject(unit.pAct, typeof(Act));
            for (var i = m_ActLevels[act.dwAct] + 1; i < m_ActLevels[act.dwAct + 1]; ++i)
            {
                uint pLevel = GetLevelPointer(act.pMisc, i);
                if (pLevel == 0)
                    continue;

                Level lvl = (Level)pd.BlackMagic.ReadObject(pLevel, typeof(Level));
                if (lvl.pRoom2First == 0)
                    InitLevel(pLevel);
                lvl = (Level)pd.BlackMagic.ReadObject(pLevel, typeof(Level));
                if (lvl.pRoom2First == 0)
                    continue;

                RevealLevel(lvl);

                MessageBox.Show("Revealing " + i.ToString());
            }

            //MessageBox.Show(str);

            ResumeProcess();
        }

        private void SuspendProcess()
        {
            process.Refresh();
            foreach (ProcessThread pT in process.Threads)
            {
                //if (pT.Id == pd.BlackMagic.ThreadId)
                //    continue;

                IntPtr pOpenThread = WinApi.OpenThread(ThreadAccess.SUSPEND_RESUME, false, pT.Id);
                if (pOpenThread == IntPtr.Zero)
                    continue;

                WinApi.SuspendThread(pOpenThread);
                WinApi.CloseHandle(pOpenThread);
            }
        }

        private void ResumeProcess()
        {
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = WinApi.OpenThread(ThreadAccess.SUSPEND_RESUME, false, pT.Id);
                if (pOpenThread == IntPtr.Zero)
                    continue;

                var suspendCount = 0;
                do
                {
                    suspendCount = WinApi.ResumeThread(pOpenThread);
                }
                while (suspendCount > 0);

                WinApi.CloseHandle(pOpenThread);
            }
        }
    }
}
