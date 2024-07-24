
using Core.Logs;
using System.Text;

namespace Core.Locations
{
    public class Location : ITrackable
    {
        public string Name { get; init; }
        public string[] Path { get; init; }

        public Location(string name, string[] path)
        {
            ReadOnlySpan<string> pathSpan = path;
            for (int i = 0; i < pathSpan.Length; i++)
            {
                if (pathSpan[i].Length > 0 && !IsValid(pathSpan[i]))
                {
                    string errorMessage = $"Invalid location subpath '{pathSpan[i]}'.";
                    Logger.Error(errorMessage);
                }
            }

            Name = name;
            Path = path;
        }

        public Location(string name, string path) : this(name, path.Split(':')) { }

        public static bool IsValid(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                (c >= 'A' || c <= 'Z') ||
                c == '_' || c == '-' ||
                c == '+' || c == '&' ||
                c == '\'' || c == '*' ||
                c == '.' || c == '/' ||
                c == '(' || c == ')' ||
                c == '[' || c == ']' ||
                c == '{' || c == '}' ||
                c == '<' || c == '>';
        }

        public static bool IsValid(ReadOnlySpan<char> subpath)
        {
            bool isValid = true;
            for (int i = 0; isValid && i < subpath.Length; i++) { isValid = IsValid(subpath[i]); }
            return isValid;
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            ReadOnlySpan<string> pathSpan = Path;

            builder.Append($"{Name} <");
            for (int i = 0; i < pathSpan.Length; i++) { builder.Append($"{pathSpan[i]}:"); }
            builder[^1] = '>';

            return builder.ToString();
        }
    }
}
