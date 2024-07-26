
using Core.Logs;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Locations
{
    public class Location : IComparable<Location>
    {
        public const char Separator = ':';

        public static readonly Location Empty = new();

        private string[] PathPrivate { get; init; }
        internal ReadOnlyMemory<char> this[int index]
        {
            get
            {
                return PathPrivate[index].AsMemory();
            }
        }

        public ReadOnlySpan<string> Path { get { return PathPrivate; } }
        public int Depth { get { return PathPrivate.Length; } }

        public Location(ReadOnlySpan<char> path)
        {
            if (!IsValid(path, out string[]? subpath))
            {
                Logger.Error($"Error parsing path to location '{path}'");
                PathPrivate = [];
            }
            else { PathPrivate = subpath; }
        }

        public Location() { PathPrivate = []; }

        public bool Match(Location filter)
        {
            bool match = filter.Depth < Depth;
            for (int i = 0; match && i < filter.Depth; i++) { match = Path[i] == filter.Path[i]; }
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

        public static bool IsValid(ReadOnlySpan<char> path, [NotNullWhen(true)] out string[]? subpaths)
        {
            // check for empty string
            if (path.Length == 0)
            {
                subpaths = [];
                return true;
            }

            /*
             * store ranges:    (start, length)
             *                  (start, length)
             *                  (start, length)
             *                        ...
             *  
             */
            List<(int Start, int Length)> ranges = [];
            int start = 0, length = 0;

            while (start + length < path.Length)
            {
                int index = start + length;

                /*
                 * x        ...     x:
                 * ^                ^
                 * start            index
                 */
                if (path[index] == Separator)
                {
                    if (length == 0)
                    {
                        subpaths = null;
                        return false;
                    }

                    ranges.Add(new(start, length));

                    // skip the separator in the next cycle
                    start += length + 1;
                    length = 0;
                }
                else
                {
                    // check if char at index is allowed
                    if (!IsValid(path[index]))
                    {
                        subpaths = null;
                        return false;
                    }
                    length++;
                }
            }

            if (length > 0) { ranges.Add(new(start, length)); }

            // convert ranges to string array
            subpaths = new string[ranges.Count];
            for (int i = 0; i < ranges.Count; i++)
            {
                subpaths[i] = path.Slice(ranges[i].Start, ranges[i].Length).ToString();
            }
            return true;
        }

        public static bool IsValid(ReadOnlySpan<char> path) { return IsValid(path, out string[]? _); }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.Append('<');
            for (int i = 0; i < Depth; i++) { builder.Append($"{Path[i]}:"); }

            if (Depth > 0) { builder.Remove(builder.Length - 1, 1); }
            builder.Append('>');

            return builder.ToString();
        }

        public int CompareTo(Location? other)
        {
            if (other == null) { return 1; }

            int comp = Depth.CompareTo(other.Depth);
            for (int i = 0; comp == 0 && i < Depth; i++) { comp = Path[i].CompareTo(other.Path[i]); }
            return comp;
        }

        public static implicit operator Location(ReadOnlySpan<char> path) => new(path);
        public static implicit operator Location(string path) => new(path);
    }
}
