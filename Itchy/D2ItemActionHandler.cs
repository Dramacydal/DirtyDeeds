using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteMagic;

namespace Itchy
{
    /*
    public struct Bit
    {
        private readonly bool _value;

        Bit(bool value) { _value = value; }
        Bit(byte value) { _value = value != 0; }
        public bool Value { get { return _value; } }
        public static implicit operator byte(Bit b) { return (byte)(b.Value ? 1 : 0); }
        public static implicit operator bool(Bit b) { return b.Value; }
        public static implicit operator Bit(bool b) { return new Bit(b); }
        public static implicit operator Bit(byte b) { return new Bit(b); }
    }

    public class BinaryPacket : BinaryReader
    {
        private byte _bitpos = 8;
        private byte _curbitval;

        public BinaryPacket(byte[] packet) :
            base(new MemoryStream(packet), Encoding.ASCII)
        {
        }

        public Bit ReadBit()
        {
            ++_bitpos;

            if (_bitpos > 7)
            {
                _bitpos = 0;
                _curbitval = ReadByte();
            }

            var bit = ((_curbitval >> (7 - _bitpos)) & 1) != 0;
            return bit;
        }

        public void ResetBitReader()
        {
            _bitpos = 8;
        }

        public uint ReadBits(int bits)
        {
            uint value = 0;
            for (var i = bits - 1; i >= 0; --i)
                if (ReadBit())
                    value |= (uint)(1 << i);

            return value;
        }
    }
    */

    //TODO: 2 unknowns left... are there imbue and craft / transmute action types ?
    public enum ItemActionType
    {
        AddToGround = 0,
        GroundToCursor = 1,			// only sent if item goes to cursor (GS packet 0x0A removes item from ground)
        DropToGround = 2,
        OnGround = 3,
        PutInContainer = 4,
        RemoveFromContainer = 5,
        Equip = 6,
        /// <summary>
        /// Sent for the equipped item when changing from a two handed weapon to a single handed weapon or vice versa.
        /// The item must be equiped on the "empty" hand or a regular SwapBodyItem will be sent instead.
        /// Empty hand meaning left hand if currently wearing a two handed weapon or the empty hand if wearing a single hand item.
        /// The result will be the new item being equipped and the old going to cursor.
        /// </summary>
        IndirectlySwapBodyItem = 7,
        Unequip = 8,
        SwapBodyItem = 9,
        AddQuantity = 0x0A,
        AddToShop = 0x0B,
        RemoveFromShop = 0x0C,
        SwapInContainer = 0x0D,
        PutInBelt = 0x0E,
        RemoveFromBelt = 0x0F,
        SwapInBelt = 0x10,
        /// <summary>
        /// Sent for the secondary hand's item going to inventory when changing from a dual item setup to a two handed weapon.
        /// </summary>
        AutoUnequip = 0x11,
        RemoveFromHireling = 0x12,	// sent along with a 9d 08 packet... Also Item on cursor when entering game ?? MiscToCursor??
        ItemInSocket = 0x13,
        UNKNOWN1 = 0x14,
        UpdateStats = 0x15,			// put item in socket; for each potion that drops in belt when lower one is removed...
        UNKNOWN2 = 0x16,
        WeaponSwitch = 0x17,
    }

    /// <summary>
    /// This is used by GS Packet 0x9C - 0x9D, and is pretty weird...
    /// </summary>
    public enum ItemCategory
    {
        Helm = 0,
        Armor = 1,
        /// <summary>
        /// Most weapons, including Crossbows
        /// </summary>
        Weapon = 5,
        /// <summary>
        /// Bows (not crossbows), sometimes shield (if equiped in LeftHand?)
        /// </summary>
        Weapon2 = 6,
        /// <summary>
        /// Shields can some somtimes be Weapon2...
        /// </summary>
        Shield = 7,
        /// <summary>
        /// Class specific items !?
        /// </summary>
        Special = 10,
        /// <summary>
        /// BaseMiscItems and gloves, boots...
        /// </summary>
        Misc = 16,
    }

    [Flags]
    public enum ItemFlag : uint
    {
        None = 0,
        Equipped = 1,
        // UNKNOWN		= 2,
        // UNKNOWN		= 4,
        InSocket = 8,
        Identified = 0x10,		 // Not undentified, really...
        x20 = 0x20,		 // Has to do with aura / state change !?
        SwitchedIn = 0x40,
        SwitchedOut = 0x80,
        Broken = 0x100,
        // UNKNOWN		= 0x200,
        Potion = 0x400,	 // Set for Mana, Healing and Rejuvenation potions, but not always !?!
        Socketed = 0x800,
        // UNKNOWN		= 0x1000,	 // WasPickedUp ? NOT !
        InStore = 0x2000,	 // Not always set when in store !?
        NotInSocket = 0x4000,	 // Illegal Equip ?
        // UNKNOWN		= 0x8000,
        Ear = 0x10000,	 // Has different packet structure
        StartItem = 0x20000,	 // Item character started with (meaning it's worthless)
        //UNKNOWN		= 0x40000,
        //UNKNOWN		= 0x80000,
        //UNKNOWN		= 0x100000,
        SimpleItem = 0x200000,	 // No ILevel
        Ethereal = 0x400000,
        Any = 0x800000,	 // Which means ??
        Personalized = 0x1000000,
        Gamble = 0x2000000, // Item a town folk is offering for gambling (same purpose as SimpleItem: no ILevel+ info)
        Runeword = 0x4000000,
        x8000000 = 0x8000000, // InducesTempStatusChange ??
        MASK = 0xFE36DF9, // Known / used values
    }

    public enum ItemContainer
    {
        Equipment = 0x00,	// Player or Merc Equipment
        Ground = 0x01,
        Inventory = 0x02,
        TraderOffer = 0x04,
        ForTrade = 0x06,
        Cube = 0x08,
        Stash = 0x0A,
        // Not a buffer... if (buffer == Equipement && Location == EquipmentLocation.NotApplicable)
        Belt = 0x0C,
        // Not a buffer... if (buffer == Equipement && destination == ItemDestination.Item)
        Item = 0x0E,
        //NPC buffers are flagged with 0x80 so they are different
        ArmorTab = 0x82,
        WeaponTab1 = 0x84,
        WeaponTab2 = 0x86,
        MiscTab = 0x88,
        //ArmorTabBottom	= 0x83, // Buffer merged with ArmorTab
        //WeaponTab1Bottom	= 0x85, // Buffer merged with WeaponTab1
        //MiscTabBottom		= 0x89, // Buffer merged with WeaponTab2
    }

    public enum EquipmentLocation
    {
        NotApplicable = 0,
        Helm = 1,
        Amulet = 2,
        Armor = 3,
        RightHand = 4,
        LeftHand = 5,
        RightHandRing = 6,
        LeftHandRing = 7,
        Belt = 8,
        Boots = 9,
        Gloves = 10,
        RightHandSwitch = 11,
        LeftHandSwitch = 12,
    }

    /*public enum ItemType : byte
    {
        Helm = 0,
        Armor = 1,
        Weapon = 5,
        Bow = 6,
        Shield = 7,
        Expansion = 0xA,
        Other = 0x10
    }

    public class ItemSniffInfoOld
    {
        public byte messageId;
        public ItemAction action;

        public ItemType itemType;
        public uint itemId;
        public bool isSocketsFull;
        public bool isIdentified;
        public bool isSwitchIn;
        public bool isSwitchOut;
        public bool isBroken;
        public bool fromBelt;
        public bool hasSockets;
        public bool isJustGenerated;
        public bool isEar;
        public bool isStartItem;
        public bool isMiscItem;
        public bool isEth;
        public bool isPersonalized;
        public bool isGamble;
        public bool isRuneword;
        public ushort mpqVersionField;
        public byte location;
        public ushort positionX;
        public ushort positionY;
        public string itemCode;
        public uint goldAmount;
        public ItemQuality quality;
        public byte usedSockets;
        public byte ilvl;
        public bool graphicBool;
        public bool classBool;
    }*/

    public class ByteConverter
    {
        public static int GetBits(byte[] bytes, ref int offset, int length)
        {
            offset += length;
            return GetBits(bytes, offset - length, length);
        }

        public static int GetBits(byte[] bytes, int offset, int length)
        {
            int bytesLen = bytes.Length * 8;
            if (offset < 0 || !(offset < bytesLen))
                throw new ArgumentOutOfRangeException("offset");
            if (length < 1 || length > 32 || (offset + length > bytesLen))
                throw new ArgumentOutOfRangeException("length");

            int result = 0;
            int bytePos = offset / 8;
            int bitPos = offset % 8;
            byte b = bytes[bytePos];
            int byteBits;
            int totBits = 0;

            while (length > 0)
            {
                if (bitPos == 8)
                {
                    b = bytes[++bytePos];
                    bitPos = 0;
                }
                byteBits = Math.Min(length, 8 - bitPos);
                result |= GetBits(b, bitPos, byteBits) << totBits;
                bitPos += byteBits;
                totBits += byteBits;
                length -= byteBits;
            }
            return result;
        }

        public static int GetBits(byte val, int offset, int length)
        {
            //TODO: support BE: if (bigEndian) offset = 8 - offset - length;
            return ((val & (((1 << (offset + length)) - 1) & ~((1 << offset) - 1))) >> offset);
        }
    }

    public class ItemActionInfo
    {
        public ItemActionType action = ItemActionType.AddToGround;

        public ItemFlag flags = ItemFlag.None;

        public string code = "";
        public ItemQuality quality = ItemQuality.Normal;

        public int x = 0;
        public int y = 0;
        public byte iLvl = 0;
        public int sockets = 0;

        public int goldCount = 0;

        public bool IsEth { get { return (flags & ItemFlag.Ethereal) != 0; } }
    }

    public partial class D2Game
    {
        public ItemActionInfo ReadItemAction(byte[] data)
        {
            var i = new ItemActionInfo();

            i.action = (ItemActionType)data[1];
            var category = (ItemCategory)data[3];
            var uid = BitConverter.ToUInt32(data, 4);

            var pOffset = data[0] == 0x9D ? 13 : 8;

            i.flags = (ItemFlag)BitConverter.ToUInt32(data, pOffset);

            var version = data[pOffset += 4];

            ++pOffset;

            if (i.action != ItemActionType.AddToGround &&
                i.action != ItemActionType.DropToGround &&
                i.action != ItemActionType.OnGround)
                return null;

            i.x = (BitConverter.ToUInt16(data, pOffset) + 131072) / 32;
            i.y = (BitConverter.ToUInt16(data, pOffset += 2) + 131072) / 32;
            pOffset += 2;

            //TODO: Unknown bit
            pOffset = pOffset * 8 + 1;

            var container = (ItemContainer)ByteConverter.GetBits(data, ref pOffset, 4);

            if ((i.flags & ItemFlag.Ear) != 0)
            {
                var charClass = ByteConverter.GetBits(data, ref pOffset, 3);
                var level = (ushort)ByteConverter.GetBits(data, ref pOffset, 7);
                var builder = new System.Text.StringBuilder();
                int mChar;
                while ((mChar = ByteConverter.GetBits(data, ref pOffset, 7)) != 0)
                    builder.Append((char)mChar);
                var name = builder.ToString();
                return i;
            }

            i.code =  String.Concat(
                (char) ByteConverter.GetBits(data, ref pOffset, 8),
                (char) ByteConverter.GetBits(data, ref pOffset, 8),
                (char) ByteConverter.GetBits(data, ref pOffset, 8));

            pOffset += 4;

            if (i.code == "gld")
            {
                pOffset += 4;

                if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                    i.goldCount = ByteConverter.GetBits(data, ref pOffset, 20);
                else
                    i.goldCount = ByteConverter.GetBits(data, ref pOffset, 18);

                return i;
            }

            var location = EquipmentLocation.NotApplicable;

            // Buffer to container mapping (sanitize NPC tabs IDs and coords, changed belt location to X, Y, etc.)
            if ((i.flags & ItemFlag.InStore) == ItemFlag.InStore // Flag is not always set for shop items !?!
                || i.action == ItemActionType.AddToShop || i.action == ItemActionType.RemoveFromShop)
            {
                int buff = (int)container | 0x80;
                if (i.y < 2 && (buff == 0x83 || buff == 0x85 || buff == 0x89))
                {
                    buff -= 1;
                    i.y += 8;
                }
                container = (ItemContainer)buff;
            }
            else if (container == ItemContainer.Equipment)
            {
                if (location == EquipmentLocation.NotApplicable)
                {
                    if ((i.flags & ItemFlag.InSocket) != 0)
                        container = ItemContainer.Item;
                    else
                    {
                        container = ItemContainer.Belt;
                        i.y = i.x / 4;
                        i.x = i.x % 4;
                    }
                }
                else
                {
                    // Hides them in dump... not needed anyway
                    i.y = -1;
                    i.x = -1;
                }
            }

            // TODO: I don't know what this is (always 2 ??)
            var unknown1 = (byte)ByteConverter.GetBits(data, ref pOffset, 4);

            // Used Sockets : 3
            byte usedSockets = 0;
            if ((i.flags & ItemFlag.Socketed) == ItemFlag.Socketed)
                usedSockets = (byte)ByteConverter.GetBits(data, pOffset, 3);
            pOffset += 3;

            // Ends here if Simple or Gamble Item
            if ((i.flags & ItemFlag.SimpleItem) != 0
            || (i.flags & ItemFlag.Gamble) != 0)
            {
                return i;
            }

            // ILevel : 7
            i.iLvl = (byte)ByteConverter.GetBits(data, ref pOffset, 7);

            // Quality : 4
            i.quality = (ItemQuality)ByteConverter.GetBits(data, ref pOffset, 4);

            // Graphic : 1 : 3
            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
               ByteConverter.GetBits(data, ref pOffset, 3);

            //TODO: ClassInfo : 1 : 11
            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                ByteConverter.GetBits(data, ref pOffset, 11);

            // Quality information
            if ((i.flags & ItemFlag.Identified) != 0)
            {
                switch (i.quality)
                {
                    case ItemQuality.Inferior:
                        /*this.prefix = new ItemAffix(ItemAffixType.InferiorPrefix,
                            ByteConverter.GetBits(data, ref pOffset, 3));*/
                        ByteConverter.GetBits(data, ref pOffset, 3);
                        break;

                    case ItemQuality.Superior:
                        //this.prefix = new ItemAffix(ItemAffixType.SuperiorPrefix, 0);
                        //TODO: quality type	
                        //	00 = AR
                        //	01 = Max Dmg
                        //	02 = AC
                        //	03 = AR + Max Dmg
                        //	04 = Durability
                        //	05 = Durability + AR
                        //	06 = Durability + Max Dmg
                        //	07 = Durability + AC 
                        pOffset += 3;
                        break;

                    case ItemQuality.Magic:
                        /*this.prefix = new ItemAffix(ItemAffixType.MagicPrefix,
                            ByteConverter.GetBits(data, ref pOffset, 11));
                        this.suffix = new ItemAffix(ItemAffixType.MagicSuffix,
                            ByteConverter.GetBits(data, ref pOffset, 11));*/
                        ByteConverter.GetBits(data, ref pOffset, 11);
                        ByteConverter.GetBits(data, ref pOffset, 11);
                        break;

                    case ItemQuality.Rare:
                    case ItemQuality.Craft:
                        /*this.prefix = new ItemAffix(ItemAffixType.RarePrefix,
                            ByteConverter.GetBits(data, ref pOffset, 8));
                        this.suffix = new ItemAffix(ItemAffixType.RareSuffix,
                            ByteConverter.GetBits(data, ref pOffset, 8));*/
                        ByteConverter.GetBits(data, ref pOffset, 8);
                        ByteConverter.GetBits(data, ref pOffset, 8);
                        for (int j = 0; j < 3; ++j)
                        {
                            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                                ByteConverter.GetBits(data, ref pOffset, 11);
                                //this.magicPrefixes[i] = new ItemAffix(ItemAffixType.MagicPrefix,
                                    //ByteConverter.GetBits(data, ref pOffset, 11));
                            if (ByteConverter.GetBits(data, ref pOffset, 1) == 1)
                                ByteConverter.GetBits(data, ref pOffset, 11);
                                //this.magicSuffixes[i] = new ItemAffix(ItemAffixType.MagicSuffix,
                                    //ByteConverter.GetBits(data, ref pOffset, 11));
                        }
                        break;

                    case ItemQuality.Set:
                        //this.setItem = BaseSetItem.Get(ByteConverter.GetBits(data, ref pOffset, 12));
                        ByteConverter.GetBits(data, ref pOffset, 12);
                        break;

                    case ItemQuality.Unique:
                        if (i.code != "std") // TODO: add UniqueItem entry to parse mod (req lvl 90)
                            ByteConverter.GetBits(data, ref pOffset, 12);
                            //this.uniqueItem = BaseUniqueItem.Get(ByteConverter.GetBits(data, ref pOffset, 12));
                        break;
                }
            }

            // Personalized Name : 7 * (NULLSTRING Length)
            if ((i.flags & ItemFlag.Personalized) != 0)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                int mChar;
                while ((mChar = ByteConverter.GetBits(data, ref pOffset, 7)) != 0)
                    builder.Append((char)mChar);
                var personalizedName = builder.ToString();
            }

            // Runeword Info : 16
            if ((i.flags & ItemFlag.Runeword) != 0)
            {
                //HACK: this is probably wrong, but works for all the runewords I tested so far...
                //TODO: Need to test cases where runewordUnknown != 5 and where ID is around 100
                //TODO: remove these fields once testing is done
                var runewordID = ByteConverter.GetBits(data, ref pOffset, 12);
                var runewordUnknown = ByteConverter.GetBits(data, ref pOffset, 4);
            }

            var itemInfo = ItemStorage.GetInfo(i.code);
            if (itemInfo == null)
                return null;

            if (itemInfo.BodyLocation == ItemBodyLocation.Armor &&
                itemInfo.ArmorType != ItemArmorType.Amulet &&
                itemInfo.ArmorType != ItemArmorType.Ring)
            {
                var defense = ByteConverter.GetBits(data, ref pOffset, 11) - 10;
            }

            var maxDurability = 0;
            var currentDurability = 0;
            if (itemInfo.BodyLocation == ItemBodyLocation.Armor &&
                itemInfo.ArmorType != ItemArmorType.Amulet &&
                itemInfo.ArmorType != ItemArmorType.Ring ||
                itemInfo.BodyLocation == ItemBodyLocation.Weapon)
            {
                maxDurability = ByteConverter.GetBits(data, ref pOffset, 8);

                // 0 durability means indestructible, but only found on old items (having the "zod bug"...)
                if (maxDurability > 0)
                {
                    currentDurability = ByteConverter.GetBits(data, ref pOffset, 8);

                    //TODO: unknown. Indestructible bit ??
                    ByteConverter.GetBits(data, ref pOffset, 1);
                }
            }

            // Total Sockets : 4
            if ((i.flags & ItemFlag.Socketed) == ItemFlag.Socketed)
                i.sockets = ByteConverter.GetBits(data, ref pOffset, 4);

            return i;
        }

        /*public ItemSniffInfoOld ReadItemOld(BinaryPacket p)
        {
            var i = new ItemSniffInfoOld();

            i.messageId = (byte)p.ReadBits(8);
            //if (messageId != 0x9C)
            //return true;

            i.action = (ItemAction)p.ReadBits(8);

            if (i.action != ItemAction.NewGround &&
                i.action != ItemAction.OldGround &&
                i.action != ItemAction.Drop)
                return null;

            var messageSize = p.ReadBits(8);
            i.itemType = (ItemType)p.ReadBits(8);
            i.itemId = p.ReadBits(32);

            i.isSocketsFull = p.ReadBit();
            p.ReadBits(3);
            i.isIdentified = p.ReadBit();
            p.ReadBit();
            i.isSwitchIn = p.ReadBit();
            i.isSwitchOut = p.ReadBit();
            i.isBroken = p.ReadBit();
            p.ReadBit();
            i.fromBelt = p.ReadBit();
            i.hasSockets = p.ReadBit();
            p.ReadBit();
            i.isJustGenerated = p.ReadBit();
            p.ReadBits(2);
            i.isEar = p.ReadBit();
            i.isStartItem = p.ReadBit();
            p.ReadBits(3);
            i.isMiscItem = p.ReadBit();
            i.isEth = p.ReadBit();
            p.ReadBit();
            i.isPersonalized = p.ReadBit();
            i.isGamble = p.ReadBit();
            i.isRuneword = p.ReadBit();
            p.ReadBits(4);
            p.ReadBit();

            i.location = (byte)p.ReadBits(3);
            i.positionX = (ushort)p.ReadBits(16);
            i.positionY = (ushort)p.ReadBits(16);

            if (i.isEar)
                return i;

            var arr = new byte[4];
            arr[0] = (byte)p.ReadBits(8);
            arr[1] = (byte)p.ReadBits(8);
            arr[2] = (byte)p.ReadBits(8);
            arr[3] = (byte)p.ReadBits(8);

            i.itemCode = Encoding.ASCII.GetString(arr).Replace(" ", "");

            if (i.itemCode == "gld")
            {
                if (!p.ReadBit())
                    i.goldAmount = p.ReadBits(12);
                else
                    i.goldAmount = p.ReadBits(32);
                i.quality = ItemQuality.Normal;

                return i;
            }

            if (i.itemCode == "ibk" || i.itemCode == "tbk" || i.itemCode == "key")
            {
                i.quality = ItemQuality.Normal;
                return i;
            }

            i.usedSockets = (byte)p.ReadBits(3);
            i.ilvl = (byte)p.ReadBits(7);
            i.quality = (ItemQuality)p.ReadBits(4);

            i.graphicBool = p.ReadBit();
            if (i.graphicBool)
                p.ReadBits(3);
            i.classBool = p.ReadBit();
            if (i.classBool)
                p.ReadBits(7);

            if (i.isIdentified)
            {
                if (i.quality == ItemQuality.Inferior || i.quality == ItemQuality.Superior)
                    p.ReadBits(3);
                else if (i.quality == ItemQuality.Magic)
                    p.ReadBits(22);
                else if (i.quality == ItemQuality.Rare || i.quality == ItemQuality.Craft)
                {
                    p.ReadBits(16);
                    for (var j = 0; j < 3; ++j)
                    {
                        if (p.ReadBit())
                            p.ReadBits(11);
                        if (p.ReadBit())
                            p.ReadBits(11);
                    }
                }
                else if (i.quality == ItemQuality.Set || i.quality == ItemQuality.Unique)
                    p.ReadBits(12);
            }

            if (i.isPersonalized)
            {
                var tmp = 0u;
                do
                {
                    tmp = p.ReadBits(7);
                }
                while (tmp != 0);
            }

            return i;
        }*/

        public bool ItemActionHandler(byte[] packet)
        {
            var i = ReadItemAction(packet);
            if (i == null)
                return true;

            var itemInfo = ItemStorage.GetInfo(i.code);
            if (itemInfo == null)
                return true;

            var configEntries = ItemSettings.GetMatch(itemInfo, (uint)i.sockets, i.IsEth, i.quality).Where(it => it.Track);

            if (i.quality == ItemQuality.Set || i.quality == ItemQuality.Unique || configEntries.Count() != 0)
            {
                var s = "Dropped " + itemInfo.Name;

                if (!itemInfo.IsBijou() && (i.quality == ItemQuality.Unique || i.quality == ItemQuality.Set))
                {
                    s += " (";
                    if (i.quality == ItemQuality.Unique)
                        for (int j = 0; j < itemInfo.PossibleUniques.Count; ++j)
                        {
                            s += itemInfo.PossibleUniques[j];
                            if (j != itemInfo.PossibleUniques.Count - 1)
                                s += ", ";
                        }
                    if (i.quality == ItemQuality.Set)
                        for (int j = 0; j < itemInfo.PossibleSets.Count; ++j)
                        {
                            s += itemInfo.PossibleSets[j];
                            if (j != itemInfo.PossibleSets.Count - 1)
                                s += ", ";
                        }
                    s += ")";
                }

                Log(i.quality.GetColor(), s);
            }


            //Log("Action: {0}", i.action);

            return true;
        }

        /*public bool ItemActionHandlerOld(byte[] packet)
        {
            var global = packet[0] == 0x9D;

            var mode = packet[1];
            var gid = BitConverter.ToUInt32(packet, 4);
            var dest = (packet[13] & 0x1C) >> 2;

            ulong icode = 0;

            switch (dest)
            {
                case 0:
                case 2:
                {
                    icode = BitConverter.ToUInt64(packet, 15) >> 0x4;
                    break;
                }
                case 3:
                case 4:
                case 6:
                {
                    if (!((mode == 0 || mode == 2) && dest == 3))
                    {
                        if (mode != 0xF && mode != 1 && mode != 12)
                            icode = BitConverter.ToUInt64(packet, 17) >> 0x1C;
                        else
                            icode = BitConverter.ToUInt64(packet, 15) >> 0x4;
                    }
                    else
                        icode = BitConverter.ToUInt64(packet, 17) >> 0x5;
                    break;
                }
            }

            var code = Encoding.ASCII.GetString(BitConverter.GetBytes(icode));
            code = code.Replace(" ", "");

            Log("Item {0} action. Code: {1} Gid: {2} Mode: {3} Dest: {4}",
                global ? "global" : "local",
                code,
                gid,
                mode,
                dest);

            return true;
        }*/
    }
}