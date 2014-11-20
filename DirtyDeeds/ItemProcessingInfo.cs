using DD.D2Enums;
using System.Collections.Generic;

namespace DD
{
    public class ItemProcessingInfo
    {
        // parameters
        public List<uint> TxtIds = new List<uint>();
        public List<string> Codes = new List<string>();

        public List<uint> SocketNum = new List<uint>();
        public List<bool> IsEth = new List<bool>();
        public List<ItemRarity> Rarity = new List<ItemRarity>();

        public List<ItemArmorType> ArmorTypes = new List<ItemArmorType>();
        public List<ItemWeaponType> WeaponTypes = new List<ItemWeaponType>();
        public List<ItemMiscType> MiscTypes = new List<ItemMiscType>();

        public List<ItemQuality> Quality = new List<ItemQuality>();

        // visualization
        public D2Color Color = D2Color.Default;
        public bool Hide = false;

        // tracker settings
        public bool Log = false;
        public bool Pick = false;
        public bool NoTele = false;
        public int PickRadius = 0;

        public bool Empty()
        {
            return Codes.Count == 0 && SocketNum.Count == 0 &&
                IsEth.Count == 0 && Rarity.Count == 0 &&
                ArmorTypes.Count == 0 && WeaponTypes.Count == 0 &&
                MiscTypes.Count == 0 && Quality.Count == 0;
        }
    }
}
