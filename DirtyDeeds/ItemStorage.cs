using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DD.Extensions;

namespace DD
{
    using CodeByIdDictionary = ConcurrentDictionary<uint, string>;
    using IdByCodeDictionary = ConcurrentDictionary<string, uint>;
    using ItemDictionary = ConcurrentDictionary<uint, ItemInfo>;
    using NameList = List<string>;

    public enum ItemBodyLocation
    {
        Armor = 0,
        Weapon = 1,
        Other = 2,
    }

    public enum ItemRarity
    {
        Normal = 0,
        Exceptional = 1,
        Elite = 2,
        Quest = 3,
        Misc = 4,
    }

    public enum ItemWeaponType
    {
        Axe = 0,
        Bow = 1,
        Crossbow = 2,
        Dagger = 3,
        Javelin = 4,
        Mace = 5,
        Polearm = 6,
        Scepter = 7,
        Spear = 8,
        Staff = 9,
        Sword = 10,
        Thrown = 11,
        Wand = 12,
        AmaJavelin = 13,
        AssKatar = 14,
        SorcOrb = 15,
        AmaBow = 16,
        ThrowingPotion = 17,
        Ammunition = 18,
        WeaponOther = 19,
    }

    public enum ItemArmorType
    {
        Helm = 0,
        BodyArmor = 1,
        Shield = 2,
        Gloves = 3,
        Boots = 4,
        Belt = 5,
        BarbHelm = 6,
        DruidHelm = 7,
        PalaShield = 8,
        NecroShield = 9,
        Ring = 10,
        Amulet = 11,
        Circlet = 12,
        ArmorOther = 13,
    }

    public enum ItemMiscType
    {
        Gem = 0,
        Jewel = 1,
        Rune = 2,
        Charm = 3,
        Misc = 4,
        Potion = 5,
        MiscOther = 6,
    }

    public class ItemInfo
    {
        public uint Id;
        public string Code;

        public ItemBodyLocation BodyLocation = ItemBodyLocation.Other;
        public ItemRarity Rarity = ItemRarity.Misc;

        public ItemWeaponType WeaponType = ItemWeaponType.WeaponOther;
        public ItemArmorType ArmorType = ItemArmorType.ArmorOther;
        public ItemMiscType MiscType = ItemMiscType.MiscOther;

        public string Name = "";
        public NameList PossibleUniques = new NameList();
        public NameList PossibleSets = new NameList();

        public bool IsBijou()
        {
            return BodyLocation == ItemBodyLocation.Armor &&
                (ArmorType == ItemArmorType.Ring || ArmorType == ItemArmorType.Amulet);
        }

        public bool IsRune()
        {
            return MiscType == ItemMiscType.Rune;
        }

        public uint RuneNumber()
        {
            if (!IsRune())
                return 0;

            return Id - 610 + 1;
        }

        public bool CanBeTelekinesised()
        {
            return MiscType == ItemMiscType.Potion ||
                Id == 523 ||    // gold
                Id == 529 ||    // scroll of town portal
                Id == 530;      // scroll of itentify
        }
    }

    public class ItemStorage
    {
        public static ItemDictionary ItemInfos { get { return itemInfos; } }

        public static ItemInfo GetInfo(uint dwTxtFileNo) { return itemInfos[dwTxtFileNo]; }
        public static ItemInfo GetInfo(string code) { return itemInfos[GetIdByCode(code)]; }

        public static string GetCodeById(uint dwTxtFileNo) { return codeByIds[dwTxtFileNo]; }
        public static uint GetIdByCode(string code) { return idByCodes[code]; }

        protected static uint maxTxtCode = 658;
        protected static ItemDictionary itemInfos = new ItemDictionary();
        protected static CodeByIdDictionary codeByIds = new CodeByIdDictionary();
        protected static IdByCodeDictionary idByCodes = new IdByCodeDictionary();

        private static string armorFile = @"data\armor.txt";
        private static string codesFile = @"data\Itemcodes.txt";
        private static string miscFile = @"data\misc.txt";
        private static string setFile = @"data\SetItems.txt";
        private static string uniqueFile = @"data\UniqueItems.txt";
        private static string weaponsFile = @"data\weapons.txt";

        static ItemStorage()
        {
            Initialize();
        }

        protected static void LoadItemCodes()
        {
            var r = new StreamReader(codesFile);

            var hasErrors = false;
            while (!r.EndOfStream)
            {
                var a = r.ReadLine().Split(' ');

                try
                {
                    codeByIds.Add(Convert.ToUInt32(a[0]), a[1]);
                    idByCodes.Add(a[1], Convert.ToUInt32(a[0]));
                }
                catch
                {
                    hasErrors = true;
                }
            }

            r.Close();

            if (hasErrors)
                MessageBox.Show("Bad " + codesFile + " file format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected static List<KeyValuePair<string, string>> LoadDictionary(string fileName)
        {
            var l = new List<KeyValuePair<string, string>>();
            var r = new StreamReader(fileName);

            while (!r.EndOfStream)
            {
                var a = r.ReadLine().Split('\t');
                l.Add(new KeyValuePair<string, string>(a[1], a[0]));
            }

            r.Close();

            return l;
        }

        protected static List<KeyValuePair<string, string>> LoadSets()
        {
            return LoadDictionary(setFile);
        }

        protected static List<KeyValuePair<string, string>> LoadUniques()
        {
            return LoadDictionary(uniqueFile);
        }

        protected static List<KeyValuePair<string, string>> LoadItemNames()
        {
            var l = new List<KeyValuePair<string, string>>();
            l.AddRange(LoadDictionary(armorFile));
            l.AddRange(LoadDictionary(miscFile));
            l.AddRange(LoadDictionary(weaponsFile));

            return l;
        }

        private static void Initialize()
        {
            LoadItemCodes();
            var sets = LoadSets();
            var uniques = LoadUniques();
            var names = LoadItemNames();

            for (uint i = 1; i <= maxTxtCode + 1; ++i)
            {
                var itemInfo = new ItemInfo();
                itemInfo.Id = i - 1;

                if (i >= 1 && i <= 86 || //normal weapons
                    //i >= 89 && i <= 92 || //quest weapons
                    i >= 94 && i <= 173 || //exc weapons
                    //i == 174 || i == 175 || //khalim's crap
                    i >= 176 && i <= 196 || //katars
                    i >= 197 && i <= 276 || //elite weapons
                    i >= 277 && i <= 306 || //soso and ama weapons
                    i == 527 || i == 529 || //arrows and bolts
                    i >= 81 && i <= 86) //potions
                    itemInfo.BodyLocation = ItemBodyLocation.Weapon;
                else if (i >= 307 && i <= 352 || //normal armor
                    i >= 353 && i <= 398 || //exc armor
                    i >= 399 && i <= 418 || //normal dudu baba pala necro njip-specific
                    i >= 419 && i <= 422 || //circlets
                    i >= 423 && i <= 468 || //elite armors
                    i >= 469 && i <= 508 || //exc and elite dudu baba pala necro class-specific
                    i == 521 || i == 523) //amulet and ring
                    itemInfo.BodyLocation = ItemBodyLocation.Armor;
                else
                    itemInfo.BodyLocation = ItemBodyLocation.Other;

                if (i >= 1 && i <= 80 || //normal weapons
                    i >= 176 && i <= 182 || //normal katars
                    i >= 307 && i <= 352 || //normal armor
                    i >= 399 && i <= 418 || //normal class-specific
                    i == 419 || i == 420) //circlet and coronet
                    itemInfo.Rarity = ItemRarity.Normal;
                else if (i >= 94 && i <= 173 || //exc weapons
                    i >= 287 && i <= 296 || //exc soso and ama weap
                    i >= 353 && i <= 398 || //exc armor
                    i >= 469 && i <= 488 || //exc dudu baba pala necro class-specific
                    i == 421) //tiara
                    itemInfo.Rarity = ItemRarity.Exceptional;
                else if (i >= 190 && i <= 196 || //elite katars
                    i >= 197 && i <= 276 || //elite weap
                    i >= 297 && i <= 306 || //elite soso and ama weap
                    i == 422 || //diadem
                    i >= 423 && i <= 468 || //elite armor
                    i >= 489 && i <= 508) //elite dudu baba pala necro class-specific
                    itemInfo.Rarity = ItemRarity.Elite;
                else if (i >= 87 && i <= 93 || //
                    i == 174 || i == 175 || //khalim's crap
                    i == 522 || //top of the staff
                    i == 525 || i == 526 || //infus, key to stones
                    i >= 546 && i <= 556 || //
                    i == 645 || //malahs potion
                    i >= 648 && i <= 654) //pandemonium items
                    itemInfo.Rarity = ItemRarity.Quest;
                else
                    itemInfo.Rarity = ItemRarity.Misc;


                if (itemInfo.BodyLocation == ItemBodyLocation.Weapon)
                {
                    if (i >= 1 && i <= 10 ||
                        i >= 94 && i <= 103 ||
                        i >= 197 && i <= 206)
                        itemInfo.WeaponType = ItemWeaponType.Axe;
                    else if (i >= 11 && i <= 14 ||
                        i >= 104 && i <= 107 || //+93
                        i >= 207 && i <= 210) //+103
                        itemInfo.WeaponType = ItemWeaponType.Wand;
                    else if (i >= 15 && i <= 25 ||
                        i >= 108 && i <= 118 ||
                        i >= 211 && i <= 221)
                        itemInfo.WeaponType = ItemWeaponType.Mace;
                    else if (i >= 26 && i <= 39 ||
                        i >= 119 && i <= 132 ||
                        i >= 222 && i <= 235)
                        itemInfo.WeaponType = ItemWeaponType.Sword;
                    else if (i >= 40 && i <= 43 ||
                        i >= 133 && i <= 136 ||
                        i >= 236 && i <= 239)
                        itemInfo.WeaponType = ItemWeaponType.Dagger;
                    else if (i >= 44 && i <= 47 ||
                        i >= 137 && i <= 140 ||
                        i >= 240 && i <= 243)
                        itemInfo.WeaponType = ItemWeaponType.Thrown;
                    else if (i >= 48 && i <= 52 ||
                        i >= 141 && i <= 145 ||
                        i >= 244 && i <= 248)
                        itemInfo.WeaponType = ItemWeaponType.Javelin;
                    else if (i >= 53 && i <= 57 || //+93
                        i >= 146 && i <= 150 || //+103
                        i >= 249 && i <= 253)
                        itemInfo.WeaponType = ItemWeaponType.Spear;
                    else if (i >= 58 && i <= 63 ||
                        i >= 151 && i <= 156 ||
                        i >= 254 && i <= 259)
                        itemInfo.WeaponType = ItemWeaponType.Polearm;
                    else if (i >= 64 && i <= 68 ||
                        i >= 157 && i <= 161 ||
                        i >= 260 && i <= 264)
                        itemInfo.WeaponType = ItemWeaponType.Staff;
                    else if (i >= 69 && i <= 76 ||
                        i >= 162 && i <= 169 ||
                        i >= 265 && i <= 272)
                        itemInfo.WeaponType = ItemWeaponType.Bow;
                    else if (i >= 77 && i <= 80 ||
                        i >= 170 && i <= 173 ||
                        i >= 273 && i <= 276)
                        itemInfo.WeaponType = ItemWeaponType.Crossbow;
                    else if (i >= 176 && i <= 196)
                        itemInfo.WeaponType = ItemWeaponType.AssKatar;
                    else if (i >= 277 && i <= 281 ||
                        i >= 287 && i <= 291 ||
                        i >= 297 && i <= 301)
                        itemInfo.WeaponType = ItemWeaponType.SorcOrb;
                    else if (i == 282 || i == 283 ||
                        i == 292 || i == 293 ||
                        i == 302 || i == 303)
                        itemInfo.WeaponType = ItemWeaponType.AmaBow;
                    else if (i >= 284 && i <= 286 ||
                        i >= 294 && i <= 296 ||
                        i >= 304 && i <= 306)
                        itemInfo.WeaponType = ItemWeaponType.AmaJavelin;
                    else if (i >= 81 && i <= 86)
                        itemInfo.WeaponType = ItemWeaponType.ThrowingPotion;
                    else if (i == 527 || i == 529) //bolts and arrows
                        itemInfo.WeaponType = ItemWeaponType.Ammunition;
                }
                else if (itemInfo.BodyLocation == ItemBodyLocation.Armor)
                {
                    if (i >= 307 && i <= 313 || //+46
                       i >= 353 && i <= 359 || //+70
                       i >= 423 && i <= 429 ||
                       i == 350 || i == 396 ||
                       i == 466)
                        itemInfo.ArmorType = ItemArmorType.Helm;
                    else if (i >= 314 && i <= 328 ||
                       i >= 360 && i <= 374 ||
                       i >= 430 && i <= 444)
                        itemInfo.ArmorType = ItemArmorType.BodyArmor;
                    else if (i >= 329 && i <= 334 ||
                       i == 351 || i == 352 ||
                       i >= 375 && i <= 380 ||
                       i == 397 || i == 398 ||
                       i >= 445 && i <= 450 ||
                       i == 467 || i == 468)
                        itemInfo.ArmorType = ItemArmorType.Shield;
                    else if (i >= 335 && i <= 339 ||
                       i >= 381 && i <= 385 ||
                       i >= 451 && i <= 455)
                        itemInfo.ArmorType = ItemArmorType.Gloves;
                    else if (i >= 340 && i <= 344 ||
                       i >= 386 && i <= 390 ||
                        i >= 456 && i <= 460)
                        itemInfo.ArmorType = ItemArmorType.Boots;
                    else if (i >= 345 && i <= 349 ||
                       i >= 391 && i <= 395 ||
                       i >= 461 && i <= 465)
                        itemInfo.ArmorType = ItemArmorType.Belt;
                    else if (i >= 399 && i <= 403 || //+70
                       i >= 469 && i <= 473 || //+20
                       i >= 489 && i <= 493)
                        itemInfo.ArmorType = ItemArmorType.DruidHelm;
                    else if (i >= 404 && i <= 408 ||
                       i >= 474 && i <= 478 ||
                       i >= 494 && i <= 498)
                        itemInfo.ArmorType = ItemArmorType.BarbHelm;
                    else if (i >= 409 && i <= 413 ||
                       i >= 479 && i <= 483 ||
                       i >= 499 && i <= 503)
                        itemInfo.ArmorType = ItemArmorType.PalaShield;
                    else if (i >= 414 && i <= 418 ||
                       i >= 484 && i <= 488 ||
                       i >= 504 && i <= 508)
                        itemInfo.ArmorType = ItemArmorType.NecroShield;

                    if (i >= 419 && i <= 422)
                        itemInfo.ArmorType = ItemArmorType.Circlet;
                    else if (i == 521)
                        itemInfo.ArmorType = ItemArmorType.Amulet;
                    else if (i == 523)
                        itemInfo.ArmorType = ItemArmorType.Ring;
                }
                else if (itemInfo.BodyLocation == ItemBodyLocation.Other)
                {
                    if (i >= 514 && i <= 518 ||
                        i >= 588 && i <= 597)
                        itemInfo.MiscType = ItemMiscType.Potion;
                    else if (i >= 558 && i <= 587 ||
                        i >= 598 && i <= 602)
                        itemInfo.MiscType = ItemMiscType.Gem;
                    else if (i >= 604 && i <= 606)
                        itemInfo.MiscType = ItemMiscType.Charm;
                    else if (i >= 611 && i <= 643)
                        itemInfo.MiscType = ItemMiscType.Rune;
                    else if (i == 644)
                        itemInfo.MiscType = ItemMiscType.Jewel;
                    else
                        itemInfo.MiscType = ItemMiscType.Misc;
                }
                else
                    itemInfo.MiscType = ItemMiscType.Misc;

                itemInfo.Code = codeByIds[i - 1];

                var tempSets = sets.FindAll(it => it.Key == itemInfo.Code);
                foreach (var tmp in tempSets)
                    itemInfo.PossibleSets.Add(tmp.Value);

                var tempUniques = uniques.FindAll(it => it.Key == itemInfo.Code);
                foreach (var tmp in tempUniques)
                    itemInfo.PossibleUniques.Add(tmp.Value);

                var tempName = names.Find(it => it.Key == itemInfo.Code);
                if (tempName.Value != "")
                    itemInfo.Name = tempName.Value;

                itemInfos.Add(i - 1, itemInfo);
            }
        }

        /*public void LoadCodes(D2Game game)
        {
            var itemCodes = new ConcurrentDictionary<uint, string>();

            game.SuspendThreads();

            var maxItemTxt = game.Debugger.ReadUInt(
                game.Debugger.GetModuleAddress("d2common.dll") + D2Common.pMaxItemText);

            for (uint i = 0; i <= maxItemTxt; ++i)
            {
                var pText = game.Debugger.Call(
                    game.Debugger.GetModuleAddress("d2common.dll") + D2Common.GetItemText,
                    WhiteMagic.CallingConventionEx.StdCall,
                    i);

                if (pText == 0)
                    continue;

                var txt = game.Debugger.Read<ItemTxt>(pText);

                itemCodes.Add(i, txt.GetCode());
            }

            game.ResumeThreads();

            var w = new StreamWriter("itemcodes.txt");
            for (uint i = 0; i <= maxTxtCode; ++i)
                w.WriteLine("{0} {1}", i, itemCodes[i]);

            w.Close();
        }*/
    }
}
