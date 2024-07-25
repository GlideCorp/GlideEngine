
using Core.Logs;
using System.ComponentModel;
using System.Text;

namespace Core.Locations
{
    public class Location
        : IComparable<Location>
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
                    Logger.Error($"Invalid location SubPath '{pathSpan[i]}'");
                }
            }

            Path = path;
        }

        public Location(string path) : this(path.Split(':')) { }
        private Location() { Path = []; }

        public bool Match(Location filter)
        {
            bool match = filter.Path.Length < Path.Length;
            for (int i = 0; match && i < filter.Path.Length; i++) { match = Path[i] == filter.Path[i]; }
            return match;
        }

        public static bool IsValid(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                (c >= 'A' || c <= 'Z') ||
                (c >= '0' || c <= '9') ||
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

            if (pathSpan.Length > 0) { builder.Remove(builder.Length - 1, 1); }
            builder.Append('>');

            return builder.ToString();
        }

        public int CompareTo(Location? other)
        {
            if (other == null) { return 1; }

            int comp = Path.Length.CompareTo(other.Path.Length);
            for (int i = 0; comp == 0 && i < Path.Length; i++) { comp = Path[i].CompareTo(other.Path[i]); }
            return comp;
        }
    }
}
