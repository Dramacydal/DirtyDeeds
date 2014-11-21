using DD.D2Enums;
using DD.D2Structs;
using System.Linq;
using WhiteMagic;
using WhiteMagic.WinAPI;
using DD.Extensions;

namespace DD.Breakpoints
{
    // 1.12 0xAC236 6FB5C236
    // 1.13d 0x96736 6FB46736
    public class ItemNameBreakPoint : D2BreakPoint
    {
        public ItemNameBreakPoint(D2Game game) : base(game, "d2client.dll", 0x96736) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            ctx.Al = pd.ReadByte(ctx.Ebp + 0x12A);
            ctx.Eip += 6;

            var pString = ctx.Edi;
            var pItem = ctx.Ebx;
            var str = pd.ReadUTF16String(pString);
            //"ÿc";
            //str = "ÿc1" + str;

            var item = pd.Read<UnitAny>(pItem);
            var itemData = pd.Read<ItemData>(item.pItemData);

            var changed = false;

            var appendix = "";

            if (Game.Settings.ItemNameHack.ShowItemCode)
            {
                var pTxt = Game.GetItemText(item.dwTxtFileNo);
                var txt = pd.Read<ItemTxt>(pTxt);
                appendix += "(" + txt.GetCode() + ")";
            }

            if (Game.Settings.ItemNameHack.ShowEth && (itemData.dwFlags & 0x400000) != 0)
                appendix += "{E}";

            var runeNumber = item.RuneNumber();
            if (runeNumber > 0 && Game.Settings.ItemNameHack.ShowRuneNumber)
                appendix += "(" + runeNumber + ")";

            if (Game.Settings.ItemNameHack.ShowSockets)
            {
                var cnt = Game.GetItemSockets(pItem, item.dwUnitId);

                if (cnt > 0 && cnt != uint.MaxValue)
                    appendix += "(" + cnt + ")";
            }

            if (Game.Settings.ItemNameHack.ShowItemLevel && itemData.dwItemLevel > 1)
                appendix += "(L" + itemData.dwItemLevel + ")";

            if (Game.Settings.ItemNameHack.ShowItemPrice)
            {
                var price = Game.GetItemPrice(pItem, item.dwUnitId);

                if (price > 0 && price != uint.MaxValue)
                    appendix += "($" + price + ")";
            }

            if (Game.Settings.ItemNameHack.ChangeItemColor)
            {
                var itemInfo = ItemStorage.GetInfo(item.dwTxtFileNo);
                if (itemInfo != null)
                {
                    var sock = Game.GetItemSockets(pItem, item.dwUnitId);
                    var configEntries = Game.ItemProcessingSettings.GetMatches(itemInfo, sock,
                        (itemData.dwFlags & 0x400000) != 0, (ItemQuality)itemData.dwQuality).Where(it => it.Color != D2Color.Default);
                    if (configEntries.Count() != 0)
                    {
                        var entry = configEntries.First();
                        str = "ÿc" + entry.Color.GetCode() + str;
                        changed = true;
                    }
                }
            }

            if (appendix != "")
            {
                str += " " + appendix;
                changed = true;
            }

            if (changed)
                pd.WriteUTF16String(pString, str);

            return true;
        }
    }
}
