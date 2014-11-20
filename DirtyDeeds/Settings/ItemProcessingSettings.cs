using DD.D2Enums;
using DD.Ini;
using System;
using System.Collections.Generic;

namespace DD.Settings
{
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
                                catch { }
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
                                    catch { }
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
