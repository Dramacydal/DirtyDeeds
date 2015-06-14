using System;
using System.Collections.Generic;
using System.Drawing;
using DD.Extensions;
using DD.Game;

namespace DD.Game.Log
{
    public enum LogType
    {
        None = 0,
        Warning = 1,
    }

    public class Logger
    {
        protected string header = "";
        protected Color headerColor = DefaultColor;

        public static volatile Logger Generic = new Logger("", Color.Empty);
        public static volatile Logger Chicken = new Logger("Chicken", Color.Red);
        public static volatile Logger MapHack = new Logger("Maphack", Color.LawnGreen);
        public static volatile Logger AutoTele = new Logger("Autotele", Color.Yellow);
        public static volatile Logger Pickit = new Logger("Pickit", Color.Violet);

        protected static Color DefaultColor = Color.Empty;
        protected static Color WarningColor = Color.Red;

        public Logger(string header, Color headerColor)
        {
            this.header = header;
            this.headerColor = headerColor;
        }

        public void Log(D2Game game, LogType logType, string message, params object[] args)
        {
            var overrideColor = Color.Empty;

            var argList = new List<object>(args);
            if (argList.Count > 0 && argList[0] is Color)
            {
                overrideColor = (Color)argList[0];
                argList.RemoveAt(0);
            }

            var time = DateTime.Now;
            var dateStr = string.Format("[{0:D2}:{1:D2}:{2:D2}] ", time.Hour, time.Minute, time.Second);

            game.Overlay.logTextBox.InvokeAppendLine(dateStr, logType == LogType.Warning ? WarningColor : DefaultColor);

            if (header != "")
                game.Overlay.logTextBox.InvokeAppendText(header + ": ", headerColor);

            game.Overlay.logTextBox.InvokeAppendText(message, logType == LogType.Warning ? WarningColor : (overrideColor != Color.Empty ? overrideColor : DefaultColor), argList.ToArray());
        }
    }
}
