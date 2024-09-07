
using System;

namespace Core.Traceable
{
    public abstract class Trackable(TrackableDirectory directory) : IComparable<Trackable>
    {
        public TrackableDirectory Directory { get; init; } = directory;

        public override string ToString()
        {
            return $$"""
                {{nameof(Trackable)}}
                {
                    {{nameof(Directory)}}: {{Directory}}
                }
                """;
        }

        public int CompareTo(Trackable? other)
        {
            if (other is null) { return 1; }

            return Directory.CompareTo(other.Directory);
        }
    }
}
