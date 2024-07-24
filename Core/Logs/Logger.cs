using System.Collections.Concurrent;

namespace Core.Logs
{
    public class Logger
    {
        private static Logger? _instance = null;
        private static Logger Instance
        {
            get
            {
                if (_instance is null) { throw new ArgumentNullException(); }
                return _instance;
            }
        }

        public static void Startup()
        {
            _instance = new();
        }

        public static void Shutdown()
        {
            Instance.Messages.CompleteAdding();
            Instance.BackgroundWorker.Wait();
            Instance.Messages.Dispose();

            _instance = null;
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
            else if (LatestInfo.Exists) // reset log file
            {
                using Stream stream = LatestInfo.OpenWrite();
                stream.SetLength(0);
            }

            BackgroundWorker = new(() => BackgroundWorkerBody(Messages));
            BackgroundWorker.Start();
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

        public static void Info(string message)
        {
#if DEBUG
            Instance.Log_Internal(message, Level.Info);
#endif
        }
        public static void Warning(string message)
        {
#if DEBUG || RELEASE
            Instance.Log_Internal(message, Level.Warning);
#endif
        }
        public static void Error(string message)
        {
#if DEBUG || RELEASE || DISTRO
            Instance.Log_Internal(message, Level.Error);
#endif
        }
    }
}
