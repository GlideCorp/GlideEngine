using System.Collections.Concurrent;

namespace Core.Logs
{
    // TODO: add static method to start and stop the logger
    public class Logger
    {
        private static Logger? _instance = null;
        public static Logger Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
        }

        public const string LatestPath = "Logs/Latest.log";
        public const int MaxQueueLength = 128;


        private static readonly FileInfo LatestInfo = new(LatestPath);

        private record Message(string Text, Level Level);
        private BlockingCollection<Message> Messages { get; init; }
        private Task BackgroundWorker { get; init; }

        private Logger()
        {
            Messages = new(new ConcurrentQueue<Message>(), MaxQueueLength);

            if (!LatestInfo.Directory!.Exists) { LatestInfo.Directory!.Create(); }
            else if (LatestInfo.Exists)// reset log file
            {
                using Stream stream = LatestInfo.OpenWrite();
                stream.SetLength(0);
            }

            BackgroundWorker = new(() => BackgroundWorkerBody(Messages));
            BackgroundWorker.Start();
        }

        ~Logger()
        {
            Messages.CompleteAdding();
            BackgroundWorker.Wait();
            Messages.Dispose();
        }

        private static void BackgroundWorkerBody(BlockingCollection<Message> messages)
        {
            while (!messages.IsCompleted || messages.Count > 0)
            {
                try
                {
                    Message message = messages.Take();

                    string coloredFormat = message.Level.FormatWithColors(message.Text);
                    string colorlessFormat = message.Level.FormatWithoutColors(message.Text);

                    Console.WriteLine(coloredFormat);
                    using StreamWriter writer = new(LatestInfo.FullName, true);
                    writer.WriteLine(colorlessFormat);
                }
                catch (Exception) { }
            }
        }


        private void Log_Internal(string text, Level level)
        {
            Message message = new(text, level);
            Messages.Add(message);
        }

        private void Info_Internal(string text) { Log_Internal(text, Level.Info); }
        private void Warning_Internal(string text) { Log_Internal(text, Level.Warning); }
        private void Error_Internal(string text) { Log_Internal(text, Level.Error); }

        private static void Log(string message, Level level) { Instance.Log_Internal(message, level); }
        public static void Info(string message) { Instance.Info_Internal(message); }
        public static void Warning(string message) { Instance.Warning_Internal(message); }
        public static void Error(string message) { Instance.Error_Internal(message); }
    }
}
