using System.Drawing;
using System.Windows.Forms;

namespace DD.Extensions
{
    public static class RichTextBoxExtensions
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

        public static void InvokeAppendText(this RichTextBox box, string text, Color color, params object[] args)
        {
            box.Invoke((MethodInvoker)delegate { box.AppendText(text, color, args); });
        }

        public static void InvokeAppendLine(this RichTextBox box, string text, Color color, params object[] args)
        {
            box.Invoke((MethodInvoker)delegate { box.AppendLine(text, color, args); });
        }
    }
}
