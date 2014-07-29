using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    public partial class D2Game
    {
        public bool GameReady()
        {
            UnitAny player;
            if (!GetPlayerUnit(out player))
                return false;

            if (player.pPath == 0 || player.pInventory == 0 || player.pAct == 0)
                return false;

            var path = pd.Read<Path>(player.pPath);
            if (path.xPos == 0 || path.pRoom1 == 0)
                return false;

            var room1 = pd.Read<Room1>(path.pRoom1);
            if (room1.pRoom2 == 0)
                return false;

            var room2 = pd.Read<Room2>(room1.pRoom2);
            if (room2.pLevel == 0)
                return false;

            var lvl = pd.Read<Level>(room2.pLevel);
            if (lvl.dwLevelNo == 0)
                return false;

            return true;
        }

        public bool GetPlayerLevel(out Level level)
        {
            UnitAny player;
            if (!GetPlayerUnit(out player))
            {
                level = new Level();
                return false;
            }

            if (player.pPath == 0)
            {
                level = new Level();
                return false;
            }

            var path = pd.Read<Path>(player.pPath);
            if (path.pRoom1 == 0)
            {
                level = new Level();
                return false;
            }

            var room1 = pd.Read<Room1>(path.pRoom1);
            if (room1.pRoom2 == 0)
            {
                level = new Level();
                return false;
            }

            var room2 = pd.Read<Room2>(room1.pRoom2);
            if (room2.pLevel == 0)
            {
                level = new Level();
                return false;
            }

            level = pd.Read<Level>(room2.pLevel);
            return true;
        }

        public bool IsInTown()
        {
            Level lvl;
            if (!GetPlayerLevel(out lvl))
                return false;

            return lvl.IsTown();
        }

        public bool OpenPortal()
        {
            if (!GameReady())
                return false;

            Level lvl;
            if (!GetPlayerLevel(out lvl))
                return false;

            if (lvl.IsTown())
            {
                Log("You are in town");
                return false;
            }

            if (lvl.IsUberTristram())
            {
                Log("Can't open portal in Uber Tristram!");
                return false;
            }

            bool isBook = false;
            var pItem = FindItem("tbk", StorageType.Inventory);
            if (pItem == 0 || GetUnitStat(pItem, Stat.AmmoQuantity) == 0)
            {
                if (pItem != 0)
                pItem = FindItem("tsc", StorageType.Inventory);
            }
            else
                isBook = true;

            if (pItem == 0)
                pItem = FindItem("tsc", StorageType.Belt);

            if (pItem == 0)
            {
                Log("Failed to find any town portal scrolls or books");
                return false;
            }

            if (isBook)
            {
                var cnt = GetUnitStat(pItem, Stat.AmmoQuantity);
                if (cnt <= 5)
                {
                    LogWarning("Warning: {0} TP's left", cnt - 1);
                }
            }

            UseItem(pItem);

            return true;
        }

        public uint GetUnitStat(uint pItem, Stat stat)
        {
            return pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetUnitStat,
                CallingConventionEx.StdCall,
                pItem, (uint)stat, 0);
        }
    }
}
