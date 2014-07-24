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
    }
}
