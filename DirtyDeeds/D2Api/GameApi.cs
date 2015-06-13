using System;
using System.Collections.Generic;
using DD.D2Enums;
using DD.Log;
using WhiteMagic;
using DD.D2Structs;
using DD.D2Pointers;
using DD.Extensions;

namespace DD
{
    public partial class D2Game
    {
        public uint GetPlayerUnit()
        {
            /*return pd.Call(pd.GetModuleAddress("d2client.dll") + D2Client.GetPlayerUnit,
                CallingConventionEx.StdCall);*/
            return pd.Read<uint>(D2Client.pPlayerUnit);
        }

        public byte GetDifficulty()
        {
            return pd.Read<byte>(D2Client.pDifficulty);
        }

        public uint GetUIVar(UIVars uiVar)
        {
            if (uiVar > UIVars.Max)
                return 0;

            return pd.Read<uint>(D2Client.pUiVars + (uint)uiVar * 4);
        }

        public void SetUIVar(UIVars uiVar, uint value)
        {
            if (uiVar > UIVars.Max)
                return;

            pd.Call(D2Client.SetUiVar,
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

        public volatile string PlayerName = "";

        public string GetPlayerName()
        {
            try
            {
                UnitAny unit;
                if (!GetPlayerUnit(out unit) || unit.pPlayerData == 0)
                    return "";

                var data = pd.Read<PlayerData>(unit.pPlayerData);
                return data.szName;
            }
            catch
            {
            }

            return "";
        }

        public void ReceivePacket(byte[] packet)
        {
            var addr = pd.AllocateBytes(packet);
            pd.Call(D2Net.ReceivePacket,
                CallingConventionEx.StdCall,
                addr, packet.Length);
            pd.FreeMemory(addr);
        }

        public void SendPacket(byte[] packet)
        {
            var addr = pd.AllocateBytes(packet);
            pd.Call(D2Net.SendPacket,
                CallingConventionEx.StdCall,
                packet.Length, 1, addr);
            pd.FreeMemory(addr);
        }

        public void UseItem(uint pItem)
        {
            if (pItem == 0)
                return;

            var item = pd.Read<UnitAny>(pItem);
            UnitAny player;
            if (!GetPlayerUnit(out player))
                return;

            switch (GetItemLocation(item))
            {
                case StorageType.Inventory:
                    {
                        var path = pd.Read<Path>(player.pPath);
                        var bytes = new List<byte>();

                        bytes.Add(0x20);
                        bytes.AddRange(BitConverter.GetBytes(item.dwUnitId));
                        bytes.AddRange(BitConverter.GetBytes((uint)path.xPos));
                        bytes.AddRange(BitConverter.GetBytes((uint)path.yPos));
                        SendPacket(bytes.ToArray());
                        break;
                    }
                case StorageType.Belt:
                    {
                        var bytes = new List<byte>();

                        bytes.Add(0x26);
                        bytes.AddRange(BitConverter.GetBytes(item.dwUnitId));
                        bytes.AddRange(BitConverter.GetBytes((uint)0));
                        bytes.AddRange(BitConverter.GetBytes((uint)0));
                        SendPacket(bytes.ToArray());
                        break;
                    }
            }
        }

        public void Interact(uint dwUnitId, UnitType unitType)
        {
            var packet = new List<byte>();
            packet.Add(0x13);
            packet.AddRange(BitConverter.GetBytes((uint)unitType));
            packet.AddRange(BitConverter.GetBytes(dwUnitId));
            SendPacket(packet.ToArray());
        }

        public void PrintGameString(string str, D2Color color = D2Color.Default)
        {
            try
            {
                var addr = pd.AllocateUTF16String(str);
                pd.Call(D2Client.PrintGameString,
                    CallingConventionEx.StdCall,
                    addr, (uint)color);

                pd.FreeMemory(addr);
            }
            catch
            {
            }
        }

        public void OpenStash()
        {
            if (!GameReady())
                return;

            var packet = new byte[] { 0x77, 0x10 };
            ReceivePacket(packet);
        }

        public void OpenCube()
        {
            if (!GameReady())
                return;

            var packet = new byte[] { 0x77, 0x15 };
            ReceivePacket(packet);
        }

        public void ExitGame()
        {
            if (!GameReady())
                return;

            pd.Call(D2Client.ExitGame,
                CallingConventionEx.FastCall);
        }

        public bool HasSkill(SkillType skillId)
        {
            UnitAny unit;
            if (!GetPlayerUnit(out unit) || unit.pInfo == 0)
                return false;

            var info = pd.Read<Info>(unit.pInfo);

            for (var pSkill = info.pFirstSkill; pSkill != 0; )
            {
                var skill = pd.Read<Skill>(pSkill);
                if (skill.pSkillInfo == 0)
                {
                    pSkill = skill.pNextSkill;
                    continue;
                }

                var skillInfo = pd.Read<SkillInfo>(skill.pSkillInfo);
                if (skillInfo.wSkillId == (ushort)skillId)
                    return true;

                pSkill = skill.pNextSkill;
            }

            return false;
        }

        public SkillType GetSkill(bool left = false)
        {
            UnitAny unit;
            if (!GetPlayerUnit(out unit) || unit.pInfo == 0)
                return SkillType.None;

            var info = pd.Read<Info>(unit.pInfo);
            if (!left && info.pRightSkill == 0 || left && info.pLeftSkill == 0)
                return SkillType.None;

            var skill = pd.Read<Skill>(left ? info.pLeftSkill : info.pRightSkill);
            if (skill.pSkillInfo == 0)
                return SkillType.None;

            var skillInfo = pd.Read<SkillInfo>(skill.pSkillInfo);
            return (SkillType)skillInfo.wSkillId;
        }

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
                Logger.Generic.Log(this, LogType.None, "You are in town.");
                return false;
            }

            if (lvl.IsUberTristram())
            {
                Logger.Generic.Log(this, LogType.None, "Can't open portal in Uber Tristram!");
                return false;
            }

            bool isBook = false;
            var pItem = FindItem("tbk", StorageType.Inventory);
            if (pItem == 0 || GetUnitStat(pItem, StatType.AmmoQuantity) == 0)
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
                Logger.Generic.Log(this, LogType.Warning, "Failed to find any town portal scrolls or books.");
                return false;
            }

            if (isBook)
            {
                var cnt = GetUnitStat(pItem, StatType.AmmoQuantity);
                if (cnt <= 5)
                    Logger.Generic.Log(this, LogType.Warning, "Warning: {0} TP's left", cnt - 1);
            }

            UseItem(pItem);

            return true;
        }

        public uint GetUnitStat(uint pUnit, StatType stat)
        {
            return pd.Call<uint>(D2Common.GetUnitStat,
                CallingConventionEx.StdCall,
                pUnit, (uint)stat, 0);
        }

        public uint GetBaseUnitStat(uint pUnit, StatType stat)
        {
            return pd.Call<uint>(D2Common.GetBaseUnitStat,
                CallingConventionEx.StdCall,
                pUnit, (uint)stat, 0);
        }
        public bool TeleportTo(ushort x, ushort y)
        {
            return CastSkillXY(SkillType.Teleport, x, y);
        }

        public bool IsDead()
        {
            var pUnit = GetPlayerUnit();
            if (pUnit == 0)
                return false;

            return llGetUnitStat(pUnit, StatType.Health) == 0;
        }

        public ushort GetUnitX(uint pUnit)
        {
            return pd.Call<ushort>(D2Client.GetUnitX,
                CallingConventionEx.ThisCall,
                pUnit);
        }

        public ushort GetUnitY(uint pUnit)
        {
            return pd.Call<ushort>(D2Client.GetUnitY,
                CallingConventionEx.ThisCall,
                pUnit);
        }

        public void RefreshUnitPosition()
        {
            UnitAny unit;
            if (!GetPlayerUnit(out unit))
                return;

            var path = pd.Read<Path>(unit.pPath);
            CurrentX = path.xPos;
            CurrentY = path.yPos;
            /*var pUnit = GetPlayerUnit();
            if (pUnit == 0)
                return;

            CurrentX = GetUnitX(pUnit);
            CurrentY = GetUnitY(pUnit);*/
        }
    }
}
