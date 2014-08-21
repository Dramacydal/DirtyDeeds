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
                if ((uint)storage != itemData.ItemLocation && storage != StorageType.Null)
                {
                    pItem = itemData.pNextInvItem;
                    continue;
                }

                var pItemTxt = pd.Call(pd.GetModuleAddress("d2common.dll") + D2Common.GetItemText,
                    CallingConventionEx.StdCall,
                    item.dwTxtFileNo);
                var txt = pd.Read<ItemTxt>(pItemTxt);
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

        /*public uint llGetUnitStat(uint pUnit, Stat stat)
        {
            if (pUnit == 0)
                return 0;

            if (pd.ReadUInt(pUnit + 92) != 0)
            {
                var pTables = pd.GetModuleAddress("d2common.dll") + D2Common.sgptDataTables;
            }
        }*/
    }
}
