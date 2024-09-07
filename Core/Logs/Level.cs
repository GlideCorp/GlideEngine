using System;
using System.Text;

namespace Core.Logs
{
    public enum Level
    {
        Info,
        Warning,
        Error,
    }

    public static class LevelExtension
    {
        private const string TimeFormat = "HH:mm:ss";
        private const string ResetColor = "\u001b[39m";
        private const string TimeColor = "\u001b[38;2;255;165;0m";
        private const string InfoColor = "\u001b[38;2;0;255;255m";
        private const string WarningColor = "\u001b[38;2;240;230;140m";
        private const string ErrorColor = "\u001b[38;2;255;69;0m";

        public static string FormatWithColors(this Level level, string message)
        {
            StringBuilder builder = new(
                TimeColor.Length +
                2 + TimeFormat.Length +// [HH:mm:ss]

                WarningColor.Length +// [HH:mm:ss]
                2 + 7 + // [level]

                ResetColor.Length + 2 +// : 
                message.Length);

            builder.Append($"{TimeColor}[{DateTime.Now.ToString(TimeFormat)}]");

            switch (level)
            {
                case Level.Info: builder.Append($"{InfoColor}[INFO]"); break;
                case Level.Warning: builder.Append($"{WarningColor}[WARNING]"); break;
                case Level.Error: builder.Append($"{ErrorColor}[ERROR]"); break;
            }

            builder.Append($"{ResetColor}: {message}");
            return builder.ToString();
        }

        public static string FormatWithoutColors(this Level level, string message)
        {
            StringBuilder builder = new(
                2 + TimeFormat.Length +// [HH:mm:ss]
                2 + 7 + // [level]
                2 + message.Length// : message
                );

            builder.Append($"[{DateTime.Now.ToString(TimeFormat)}]");

            switch (level)
            {
                case Level.Info: builder.Append($"[INFO]"); break;
                case Level.Warning: builder.Append($"[WARNING]"); break;
                case Level.Error: builder.Append($"[ERROR]"); break;
            }

            builder.Append($": {message}");
            return builder.ToString();
        }
    }
}
