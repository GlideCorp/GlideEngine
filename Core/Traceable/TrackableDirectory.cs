
using System;
using System.Collections.Generic;
using Core.Logs;
using Core.Utilities;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Core.Traceable
{
    

    public class TrackableDirectory : IComparable<TrackableDirectory>
    {
        public const char DefaultSeparator = ':';
        public static readonly TrackableDirectory Empty = new("");

        public string[] Subdirectories { get; init; }

        public string this[int index] => Subdirectories[index];

        public TrackableDirectory(string directory)
        {
            if (!IsValid(directory, DefaultSeparator, out string[]? subdirectories))
            {
                Logger.Error($"Error parsing directory '{directory}' with '{DefaultSeparator}' separator.");
                Subdirectories = [];
            }
            else { Subdirectories = subdirectories; }
        }

        public TrackableDirectory(string directory, char separator)
        {
            if (!IsValid(directory, separator, out string[]? subdirectories))
            {
                Logger.Error($"Error parsing directory '{directory}' with '{separator}' separator.");
                Subdirectories = [];
            }
            else { Subdirectories = subdirectories; }
        }

        private static bool IsValid(string directory, char separator, [NotNullWhen(true)] out string[]? subdirectories)
        {
            // check for empty string
            if (directory.Length == 0)
            {
                subdirectories = [];
                return true;
            }

            List<string> subdirs = [];
            ReadOnlySpan<char> span = directory;
            int start = 0, length = 0;

            while (start + length < span.Length)
            {
                int index = start + length;

                /*
                 * x     ...     x:
                 * ^             ^
                 * start         index
                 */
                if (span[index] == separator)
                {
                    if (length == 0)
                    {
                        subdirectories = [.. subdirs];
                        return false;
                    }

                    subdirs.Add(span.Slice(start, length).ToString());

                    // skip the separator in the next cycle
                    start += length + 1;
                    length = 0;
                }
                else { length++; }
            }

            if (length > 0) { subdirs.Add(span.Slice(start, length).ToString()); }
            subdirectories = [.. subdirs];
            return true;
        }

        public bool IsSubdirectoryOf(TrackableDirectory directory)
        {
            return Slider.StartsWith(Subdirectories.AsReadOnly(), directory.Subdirectories.AsReadOnly());
        }

        public static implicit operator TrackableDirectory(string directory)
        {
            return string.IsNullOrEmpty(directory) ? Empty : new(directory);
        }

        public override string ToString()
        {
            StringBuilder builder = new();

            builder.Append('<');
            for (int i = 0; i < Subdirectories.Length; i++)
            {
                builder.Append($"{Subdirectories[i]}:");
            }
            builder[^1] = '>';

            return builder.ToString();
        }

        public int CompareTo(TrackableDirectory? other)
        {
            if (other is null) { return 1; }
            if (Subdirectories.Length == 0) { return 1; }
            if (other.Subdirectories.Length == 0) { return -1; }

            int length = Math.Min(Subdirectories.Length, other.Subdirectories.Length);
            int comp = string.Compare(Subdirectories[0], other.Subdirectories[0], StringComparison.Ordinal);
            for (int i = 1; comp == 0 && i < length; i++)
            {
                comp = string.Compare(Subdirectories[i], other.Subdirectories[i], StringComparison.Ordinal);
            }

            return comp;
        }
    }
}
