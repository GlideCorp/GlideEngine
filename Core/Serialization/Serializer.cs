using Core.Logs;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public static T? Deserialize<T>(FileInfo file)
        {
            if (!file.Exists)
            {
                Logger.Error("File not found");
                return default;
            }

            string jsonString = File.ReadAllText(file.FullName);
            return Deserialize<T>(jsonString);
        }
    }
}
