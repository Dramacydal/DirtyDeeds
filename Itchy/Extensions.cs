using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itchy
{
    public static class Extensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            if (color == Color.Empty)
            {
                box.AppendText(text);
                return;
            }

            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;

            box.SelectionStart = box.TextLength;
            box.ScrollToCaret();
        }

        public static void AppendLine(this RichTextBox box, string text, Color color)
        {
            box.AppendText("\n" + text, color == Color.Empty ? box.ForeColor : color);
        }

        public static void LogLine(this RichTextBox box, string text, Color color, params object[] args)
        {
            text = string.Format(text, args);

            var time = DateTime.Now;
            var str = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", time.Hour, time.Minute, time.Second);
            box.Invoke((MethodInvoker)delegate { box.AppendLine(str + text, color); });
        }

        public static char GetCode(this D2Color color)
        {
            switch (color)
            {
                case D2Color.Default:
                    return '0';
                case D2Color.Red:
                    return '1';
                case D2Color.Greed:
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

        public static long MSecToNow(this DateTime date)
        {
            return (DateTime.Now.Ticks - date.Ticks) / TimeSpan.TicksPerMillisecond;
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
                    return Color.Blue;
                case ItemQuality.Rare:
                    return Color.Yellow;
                case ItemQuality.Set:
                    return Color.LimeGreen;
                case ItemQuality.Unique:
                    return Color.DarkGoldenrod;
                case ItemQuality.Craft:
                    return Color.Orange;
                default:
                    break;
            }

            return Color.White;
        }
    }
}
