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
        public uint FindUnit(uint dwId, UnitType dwType)
        {
            var result = FindServerSideUnit(dwId, (uint)dwType);
            return result != 0 ? result : FindClientSideUnit(dwId, (uint)dwType);
        }

        public bool FindUnit(uint dwId, UnitType dwType, out UnitAny unit)
        {
            var result = FindUnit(dwId, dwType);
            if (result == 0)
            {
                unit = new UnitAny();
                return false;
            }

            unit = pd.Read<UnitAny>(result);
            return true;
        }

        protected uint FindServerSideUnit(uint dwId, uint dwType)
        {
            var pHashTable = D2Client.pServerUnitTable;

            pHashTable += pd.GetModuleAddress("d2client.dll") + 128 * 4 * dwType;

            var result = pd.ReadUInt(pHashTable + 4 * (dwId & 0x7F));
            if (result != 0)
            {
                while (pd.ReadUInt(result + 12) != dwId)
                {
                    result = pd.ReadUInt(result + 228);
                    if (result == 0)
                        return 0;
                }

                if (pd.ReadUInt(result) != dwType)
                    throw new Exception("FindServerSideUnit type != dwType");
            }

            return result;
        }

        protected uint FindClientSideUnit(uint dwId, uint dwType)
        {
            var pHashTable = D2Client.pClientUnitTable;

            pHashTable += pd.GetModuleAddress("d2client.dll") + 128 * 4 * dwType;

            var result = pd.ReadUInt(pHashTable + 4 * (dwId & 0x7F));
            if (result != 0)
            {
                while (pd.ReadUInt(result + 12) != dwId)
                {
                    result = pd.ReadUInt(result + 228);
                    if (result == 0)
                        return 0;
                }

                if (pd.ReadUInt(result) != dwType)
                    throw new Exception("FindServersideUnit type != dwType");
            }

            return result;
        }

        public uint FindItem(string code, StorageType storage)
        {
            UnitAny unit;
            if (!GetPlayerUnit(out unit))
                return 0;

            var inv = pd.Read<Inventory>(unit.pInventory);

            uint pItem = 0u;
            for (pItem = inv.pFirstItem; pItem != 0; )
            {
                var item = pd.Read<UnitAny>(pItem);
                var itemData = pd.Read<ItemData>(item.pItemData);

                var pItemTxt = pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetItemText,
                    CallingConventionEx.StdCall,
                    item.dwTxtFileNo);
                var txt = pd.Read<ItemTxt>(pItemTxt);
                if ((uint)storage != itemData.ItemLocation && storage != StorageType.Null)
                {
                    pItem = itemData.pNextInvItem;
                    continue;
                }

                if (txt.GetCode() == code)
                    break;

                pItem = itemData.pNextInvItem;
            }

            return pItem;
        }

        public StorageType GetItemLocation(UnitAny item)
        {
            if (item.pItemData == 0)
                return StorageType.Null;

            var itemData = pd.Read<ItemData>(item.pItemData);
            switch ((StorageType)itemData.ItemLocation)
            {
                case StorageType.Inventory:
                    return StorageType.Inventory;
                case StorageType.Trade:
                    if ((Nodepage)itemData.NodePage == Nodepage.Storage)
                        return StorageType.Trade;
                    break;
                case StorageType.Cube:
                    return StorageType.Cube;
                case StorageType.Stash:
                    return StorageType.Stash;
                case StorageType.Null:
                    switch ((Nodepage)itemData.NodePage)
                    {
                        case Nodepage.Equip:
                            return StorageType.Equip;
                        case Nodepage.BeltSlots:
                            return StorageType.Belt;
                    }
                    break;
            }

            return StorageType.Null;
        }

        public StorageType GetItemLocation(uint pItem)
        {
            var item = pd.Read<UnitAny>(pItem);
            return GetItemLocation(item);
        }

        public uint GetItemText(uint dwTxtFileNo)
        {
            if (dwTxtFileNo >= pd.ReadUInt(pd.GetModuleAddress("d2common.dll") + D2Common.pMaxItemText))
                return 0;

            var pData = pd.ReadUInt(pd.GetModuleAddress("d2common.dll") + D2Common.pItemTextData);
            if (pData == 0)
                return 0;

            return pData + 424 * dwTxtFileNo;
        }

        public void ReceivePacket(byte[] packet)
        {
            var addr = pd.AllocateBytes(packet);
            pd.Call(pd.GetModuleAddress("d2net.dll") + D2Net.ReceivePacket,
                CallingConventionEx.StdCall,
                addr, (uint)packet.Length);
            pd.FreeMemory(addr);
        }

        public void SendPacket(byte[] packet)
        {
            var addr = pd.AllocateBytes(packet);
            pd.Call(pd.GetModuleAddress("d2net.dll") + D2Net.SendPacket,
                CallingConventionEx.StdCall,
                (uint)packet.Length, 1, addr);
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

        public uint llGetUnitStat(uint pUnit, StatType stat)
        {
            var unit = pd.Read<UnitAny>(pUnit);
            if (unit.pStats == 0)
                return 0;

            var pTables = pd.ReadUInt(pd.GetModuleAddress("d2common.dll") + D2Common.sgptDataTables);
            var s755 = pd.ReadUInt(pTables + 755 * 4);
            var s757 = pd.ReadUInt(pTables + 757 * 4);

            if ((uint)stat > s757 || s755 == 0 || s755 + 324 * (uint)stat == 0)
                return 0;

            return sub_6FDA83F0(unit.pStats, (uint)stat << 16, s755 + 324 * (uint)stat);
        }

        private uint sub_6FDA83F0(uint pStatList, uint statMask, uint someOffs)
        {
            var v3 = pd.ReadUInt(pStatList + 16) & 0x80000000;
            var v4 = pStatList + 72;
            if (v3 == 0)
                v4 = pStatList + 36;
            var v5 = sub_6FDA7C20(v4, (int)statMask);
            var v6 = v5 > 0 ? pd.ReadUInt(v4) + 8 * (uint)v5 : 0;

            var result = 0u;
            if (v5 >= 0 && v6 != 0)
            {
                result = pd.ReadUInt(v6 + 4);
                if (someOffs != 0)
                {
                    var gdwBitMask = pd.ReadByte(pd.GetModuleAddress("fog.dll") + Fog.gdwBitMasks + 8);
                    if ((pd.ReadByte(someOffs + 5) & gdwBitMask) != 0)
                    {
                        if (v3 != 0)
                        {
                            var v8 = pd.ReadUInt(pStatList + 68);
                            if (v8 != 0)
                            {
                                var tmp = pd.ReadUInt(v8);
                                if (tmp == 0 || tmp == 1)
                                {
                                    var v9 = pd.ReadUInt(someOffs + 44);
                                    if (result < v9)
                                        result = v9 << pd.ReadByte(someOffs + 24);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private int sub_6FDA7C20(uint a1, int statMask)
        {
            var v3 = pd.ReadUInt(a1);
            var v4 = (int)pd.ReadShort(a1 + 4);
            var v2 = 0;
            var result = 0;
            while (v2 < v4)
            {
                result = v2 + (v4 - v2) / 2;
                var v6 = pd.ReadUInt(v3 + 8 * (uint)result);
                if (statMask <= v6)
                {
                    if (statMask >= v6)
                        return result;
                    v4 = v2 + (v4 - v2) / 2;
                }
                else
                {
                    v2 = result + 1;
                }
            }

            return -1;
        }
    }
}
