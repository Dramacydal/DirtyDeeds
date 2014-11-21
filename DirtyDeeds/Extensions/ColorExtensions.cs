using DD.D2Enums;
using System.Drawing;

namespace DD.Extensions
{
    public static class ColorExtensions
    {
        public static char GetCode(this D2Color color)
        {
            switch (color)
            {
                case D2Color.Default:
                    return '0';
                case D2Color.Red:
                    return '1';
                case D2Color.Green:
                    return '2';
                case D2Color.Blue:
                    return '3';
                case D2Color.Tan:
                    return '4';
                case D2Color.Gray:
                    return '5';
                case D2Color.Black:
                    return '6';
                case D2Color.Gold:
                    return '7';
                case D2Color.Orange:
                    return '8';
                case D2Color.Yellow:
                    return '9';
                case D2Color.Gold2:
                    return '=';
                case D2Color.BoldWhite:
                    return '-';
                case D2Color.BoldWhite2:
                    return '+';
                case D2Color.DarkGreen:
                    return '<';
                case D2Color.Purple:
                    return ';';
            }

            return '0';
        }

        public static Color GetColor(this ItemQuality quality)
        {
            switch (quality)
            {
                case ItemQuality.Inferior:
                    return Color.Gray;
                case ItemQuality.Normal:
                case ItemQuality.Superior:
                    return Color.White;
                case ItemQuality.Magic:
                    return Color.DodgerBlue;
                case ItemQuality.Rare:
                    return Color.Gold;
                case ItemQuality.Set:
                    return Color.LimeGreen;
                case ItemQuality.Unique:
                case ItemQuality.Craft:
                    return Color.DarkOrange;
                default:
                    break;
            }

            return Color.White;
        }
    }
}
