
using Core.Logs;
using System.ComponentModel;
using System.Text;

namespace Core.Locations
{
    public class Location
    {
        public static readonly Location Empty = new();

        public string[] Path { get; init; }

        public Location(string[] path)
        {
            ReadOnlySpan<string> pathSpan = path;
            for (int i = 0; i < pathSpan.Length; i++)
            {
                if (pathSpan[i].Length == 0 || !IsValid(pathSpan[i]))
                {
                    string message = $"Invalid location SubPath '{pathSpan[i]}'";
                    Logger.Error(message);
                    throw new InvalidEnumArgumentException(message);
                }
            }

            Path = path;
        }

        public Location(string path) : this(path.Split(':')) { }

        public Location() { Path = []; }

        public bool Match(Location filter)
        {
            bool match = Path.Length == filter.Path.Length;
            for (int i = 0; match && i < Path.Length; i++) { match = Path[i] == filter.Path[i]; }
            return match;
        }

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

        public static bool IsValid(ReadOnlySpan<char> subPath)
        {
            bool isValid = true;
            for (int i = 0; isValid && i < subPath.Length; i++) { isValid = IsValid(subPath[i]); }
            return isValid;
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            ReadOnlySpan<string> pathSpan = Path;

            builder.Append('<');
            for (int i = 0; i < pathSpan.Length; i++) { builder.Append($"{pathSpan[i]}:"); }
            builder[^1] = '>';

            return builder.ToString();
        }
    }
}
