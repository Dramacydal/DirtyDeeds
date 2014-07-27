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
    }
}
