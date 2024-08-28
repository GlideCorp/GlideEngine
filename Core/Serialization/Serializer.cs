using Core.Logs;
using System.Text.Json;

namespace Core.Serialization
{
    public class Serializer
    {
        private static Serializer? _instance = null;
        private static Serializer Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
        }

        private JsonSerializerOptions SerializationOptions { get; init; }

        private Serializer()
        {
            SerializationOptions = new()
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
        }

        public static string Serialize<T>(T @object)
        {
            return JsonSerializer.Serialize(@object, Instance.SerializationOptions);
        }

        public static T? Deserialize<T>(string text)
        {
            return JsonSerializer.Deserialize<T>(text, Instance.SerializationOptions);
        }

        public static bool Deserialize<T>(FileInfo file, out T? @object)
        {
            if (!file.Exists)
            {
                Logger.Error("File not found");
                @object = default;
                return false;
            }

            string jsonString = File.ReadAllText(file.FullName);
            try
            {
                @object = Deserialize<T>(jsonString); 
                return true;
            }
            catch (Exception)
            {
                Logger.Error($"Deserialize error for type '{typeof(T)}'");
                @object = default;
                return false;
            }
        }
    }
}
