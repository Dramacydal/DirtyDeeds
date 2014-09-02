using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IniParser;
using Itchy.D2Enums;

namespace Itchy
{
    [Serializable]
    public class HackSettings
    {
        public HackSettings() { }

        public bool Enabled = false;
        public static int Cost { get { return 1; } }
    }

    [Serializable]
    public class KeySettings
    {
        public KeySettings() { }

        public Keys Key = Keys.None;
    }

    [Serializable]
    public class ItemTrackerSettings : HackSettings
    {
        public bool EnablePickit = false;
        public bool UseTelekinesis = false;
        public bool EnableTelepick = false;
        public bool TeleBack = false;
        public bool TownPick = false;

        public bool LogRunes = false;
        public bool LogSets = false;
        public bool LogUniques = false;
        public bool LogItems = false;
        public KeySettings ReactivatePickit = new KeySettings();
    }

    [Serializable]
    public class PacketReceivedHackSettings : HackSettings
    {
        public bool BlockFlash = false;
        public bool FastTele = false;
        public bool FastPortal = false;
        public ItemTrackerSettings ItemTracker = new ItemTrackerSettings();
    }

    [Serializable]
    public class FastPortalSettings : KeySettings
    {
        public bool GoToTown = false;
    }

    [Serializable]
    public class ChickenSettings : HackSettings
    {
        public bool ChickenToTown = false;
        public bool ChickenOnHostile = false;
        public double ChickenLifePercent = 0.0f;
        public double ChickenManaPercent = 0.0f;
    }

    [Serializable]
    public class ItemNameHackSettings : HackSettings
    {
        public bool ShowEth = false;
        public bool ShowItemLevel = false;
        public bool ShowItemPrice = false;
        public bool ShowRuneNumber = false;
        public bool ShowSockets = false;
        public bool ShowItemCode = false;
        public bool ChangeItemColor = false;
    }

    [Serializable]
    public class ViewInventorySettings : HackSettings
    {
        public new static int Cost { get { return 3; } }

        public Keys ViewInventoryKey = Keys.None;
    }

    [Serializable]
    public class InfravisionSettings : HackSettings
    {
        public bool HideCorpses = false;
        public bool HideDying = false;
        public bool HideItems = false;
    }

    [Serializable]
    public class GameSettings
    {
        public GameSettings() { }

        public HackSettings LightHack = new HackSettings();
        public HackSettings WeatherHack = new HackSettings();
        public PacketReceivedHackSettings ReceivePacketHack = new PacketReceivedHackSettings();
        public ItemNameHackSettings ItemNameHack = new ItemNameHackSettings();
        public ViewInventorySettings ViewInventory = new ViewInventorySettings();
        public InfravisionSettings Infravision = new InfravisionSettings();

        public KeySettings RevealAct = new KeySettings();
        public KeySettings OpenStash = new KeySettings();
        public KeySettings OpenCube = new KeySettings();
        public KeySettings FastExit = new KeySettings();
        public FastPortalSettings FastPortal = new FastPortalSettings();
        public ChickenSettings Chicken = new ChickenSettings();
    }

    [Serializable]
    public class OverlaySettings
    {
        public OverlaySettings() { }

        public float LogFontSize = 0.0f;

        public Point SettingsPosition = new Point();
        public Point LogPosition = new Point();
        public Point StatsPosition = new Point();
    }

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

    public class ItemProcessingSettings
    {
        private List<ItemProcessingInfo> infoList = new List<ItemProcessingInfo>();
        private readonly IniReader ini;

        private static readonly char configSeparator = ',';

        public ItemProcessingSettings(string path)
        {
            ini = new IniReader(path);
        }

        public void Load()
        {
            infoList.Clear();
            ini.Parse();

            foreach (var sect in ini.Sections)
            {
                var info = new ItemProcessingInfo();
                var disabled = false;
                foreach (var key in sect.Keys)
                {
                    var value = key.Value.ToLower();
                    if (value == "")
                        continue;

                    switch (key.Key.ToLower())
                    {
                        case "disabled":
                        {
                            disabled = IsTrue(value);
                            break;
                        }
                        case "code":
                        {
                            var args = value.Split(configSeparator);
                            foreach (var code in args)
                                info.Codes.Add(code.ToLower());

                            foreach (var code in info.Codes)
                                info.TxtIds.Add(ItemStorage.GetIdByCode(code));
                            break;
                        }
                        case "color":
                        {
                            D2Color col;
                            if (!Enum.TryParse<D2Color>(value, true, out col))
                                break;
                            info.Color = col;
                            break;
                        }
                        case "hide":
                        {
                            info.Hide = IsTrue(value);
                            break;
                        }
                        case "log":
                        {
                            info.Log = IsTrue(value);
                            break;
                        }
                        case "pick":
                        {
                            info.Pick = IsTrue(value);
                            break;
                        }
                        case "notele":
                        {
                            info.NoTele = IsTrue(value);
                            break;
                        }
                        case "pickradius":
                        {
                            try
                            {
                                var val = Convert.ToInt32(value);
                                if (val > 0)
                                    info.PickRadius = val;
                            }
                            catch (Exception) { }
                            break;
                        }
                        case "sock":
                        {
                            var args = value.Split(configSeparator);
                            foreach (var arg in args)
                            {
                                try
                                {
                                    var val = Convert.ToByte(arg);
                                    if (val >= 0 && val <= 6)
                                        info.SocketNum.Add(val);
                                }
                                catch (Exception) { }
                            }
                            break;
                        }
                        case "eth":
                        {
                            var val = IsTrue(value);
                            info.IsEth.Add(val);
                            break;
                        }
                        case "rarity":
                        {
                            var args = value.Split(configSeparator);
                            foreach (var arg in args)
                            {
                                ItemRarity rarity;
                                if (!Enum.TryParse<ItemRarity>(value, true, out rarity))
                                    break;

                                info.Rarity.Add(rarity);
                            }
                            break;
                        }
                        case "type":
                        {
                            var args = value.Split(configSeparator);
                            foreach (var arg in args)
                            {
                                ItemArmorType arm;
                                if (Enum.TryParse<ItemArmorType>(arg, true, out arm))
                                {
                                    info.ArmorTypes.Add(arm);
                                    continue;
                                }
                                ItemWeaponType wep;
                                if (Enum.TryParse<ItemWeaponType>(arg, true, out wep))
                                {
                                    info.WeaponTypes.Add(wep);
                                    continue;
                                }
                                ItemMiscType misc;
                                if (Enum.TryParse<ItemMiscType>(arg, true, out misc))
                                {
                                    info.MiscTypes.Add(misc);
                                    continue;
                                }
                            }
                            break;
                        }
                        case "quality":
                        {
                            var args = value.Split(configSeparator);
                            foreach (var arg in args)
                            {
                                ItemQuality quality;
                                if (!Enum.TryParse<ItemQuality>(value, true, out quality))
                                    continue;

                                info.Quality.Add(quality);
                            }
                            break;
                        }
                    }

                    if (disabled)
                        break;
                }

                if (!disabled && !info.Empty())
                    infoList.Add(info);
            }
        }

        private static bool IsTrue(string value)
        {
            return value.ToLower() != "0" && value.ToLower() != "false";
        }

        public List<ItemProcessingInfo> GetMatches(ItemInfo info, uint sock, bool isEth, ItemQuality quality)
        {
            return infoList.FindAll(e => !e.Empty() &&
                (e.TxtIds.Count == 0 || e.TxtIds.Contains(info.Id)) &&
                (e.SocketNum.Count == 0 || e.SocketNum.Contains(sock)) &&
                (e.IsEth.Count == 0 || e.IsEth.Contains(isEth)) &&
                (e.Rarity.Count == 0 || e.Rarity.Contains(info.Rarity)) &&
                (e.Quality.Count == 0 || e.Quality.Contains(quality)) &&
                (info.BodyLocation == ItemBodyLocation.Armor &&
                    (e.ArmorTypes.Count == 0 || e.ArmorTypes.Contains(info.ArmorType)) ||
                info.BodyLocation == ItemBodyLocation.Weapon &&
                    (e.WeaponTypes.Count == 0 || e.WeaponTypes.Contains(info.WeaponType)) ||
                info.BodyLocation == ItemBodyLocation.Other &&
                    (e.MiscTypes.Count == 0 || e.MiscTypes.Contains(info.MiscType))));
        }
    }
}
