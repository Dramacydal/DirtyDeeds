using DD.D2Enums;
using DD.D2Structs;
using System.Linq;
using WhiteMagic;
using WhiteMagic.WinAPI;

namespace DD.Breakpoints
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
            else if (Game.Settings.Infravision.HideCorpses || Game.Settings.Infravision.HideDying || Game.Settings.Infravision.HideItems)
            {
                var unit = pd.Read<UnitAny>(pUnit);
                switch ((UnitType)unit.dwType)
                {
                    case UnitType.Monster:
                        {
                            if (Game.Settings.Infravision.HideCorpses &&
                                unit.dwMode == (uint)NpcMode.Dead ||
                                Game.Settings.Infravision.HideDying &&
                                unit.dwMode == (uint)NpcMode.Death)
                                hide = true;
                            break;
                        }
                    case UnitType.Item:
                        {
                            if (!Game.Settings.Infravision.HideItems)
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
