
using Core.Logs;

public class Program
{
    public static void Main(string[] args)
    {
        Task.Delay(2000).Wait();
        Logger.Info("Ciaoo");

        Task.Delay(2000).Wait();
        Logger.Warning("Ciaoo");

        Task.Delay(2000).Wait();
        Logger.Error("Ciaoo");
    }
}
