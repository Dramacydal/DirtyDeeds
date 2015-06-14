using DD.Game.D2Structs;
using DD.Extensions;
using System.Linq;
using WhiteMagic;
using WhiteMagic.WinAPI;
using DD.Game.Enums;

namespace DD.Game.Breakpoints
{
    public class InfravisionBreakPoint : D2BreakPoint
    {
        public InfravisionBreakPoint(D2Game game) : base(game, "d2client.dll", 0xB4A23) { }

        public override bool HandleException(ref CONTEXT ctx, ProcessDebugger pd)
        {
            // Esi - pUnit
            var hide = false;

            var pUnit = ctx.Esi;
            if (pUnit == 0)
                hide = true;
            else if (Game.Settings.Infravision.HideCorpses.IsEnabled() || Game.Settings.Infravision.HideDying.IsEnabled() || Game.Settings.Infravision.HideItems.IsEnabled())
            {
                var unit = pd.Read<UnitAny>(pUnit);
                switch ((UnitType)unit.dwType)
                {
                    case UnitType.Monster:
                        {
                            if (Game.Settings.Infravision.HideCorpses.IsEnabled() &&
                                unit.dwMode == (uint)NpcMode.Dead ||
                                Game.Settings.Infravision.HideDying.IsEnabled() &&
                                unit.dwMode == (uint)NpcMode.Death)
                                hide = true;
                            break;
                        }
                    case UnitType.Item:
                        {
                            if (!Game.Settings.Infravision.HideItems.IsEnabled())
                                break;

                            var itemInfo = ItemStorage.GetInfo(unit.dwTxtFileNo);
                            if (itemInfo != null)
                            {
                                var itemData = pd.Read<ItemData>(unit.pItemData);
                                var pTxt = Game.GetItemText(unit.dwTxtFileNo);
                                var txt = pd.Read<ItemTxt>(pTxt);

                                var sock = Game.GetItemSockets(pUnit, unit.dwUnitId);
                                var configEntries = Game.ItemProcessingSettings.GetMatches(itemInfo, sock,
                                    (itemData.dwFlags & 0x400000) != 0, (ItemQuality)itemData.dwQuality).Where(it => it.Hide);
                                if (configEntries.Count() != 0)
                                    hide = true;
                            }
                            break;
                        }
                }
            }

            ctx.Eax = hide ? 1u : 0u;

            ctx.Eip += 0x77;

            return true;
        }
    }
}
