using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itchy.D2Enums;
using WhiteMagic;

namespace Itchy
{
    public static class Extensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color, params object[] args)
        {
            text = string.Format(text, args);
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

        public static void AppendLine(this RichTextBox box, string text, Color color, params object[] args)
        {
            box.AppendText("\n" + text, color == Color.Empty ? box.ForeColor : color, args);
        }

        public static void LogLine(this RichTextBox box, string text, Color color, params object[] args)
        {
            var time = DateTime.Now;
            var str = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", time.Hour, time.Minute, time.Second);
            box.Invoke((MethodInvoker)delegate { box.AppendLine(str + text, color, args); });
        }

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

        public static long MSecToNow(this DateTime date)
        {
            return (DateTime.Now.Ticks - date.Ticks) / TimeSpan.TicksPerMillisecond;
        }

        public static bool Passed(this DateTime date)
        {
            return date <= DateTime.Now;
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

        public static void Add<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            while (!dict.TryAdd(key, value)) { }
        }

        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            if (!dict.ContainsKey(key))
                return;

            TValue value;
            while (!dict.TryRemove(key, out value)) { }
        }

        public static uint ReadUInt(this ProcessDebugger pd, ModulePointer offs)
        {
            return pd.ReadUInt(pd.GetAddress(offs));
        }

        public static byte ReadByte(this ProcessDebugger pd, ModulePointer offs)
        {
            return pd.ReadByte(pd.GetAddress(offs));
        }

        public static int X(this Size sz)
        {
            return sz.Width;
        }

        public static int Y(this Size sz)
        {
            return sz.Height;
        }

        public static Point Clone(this Point p)
        {
            return new Point(p.X, p.Y);
        }
    }
}
