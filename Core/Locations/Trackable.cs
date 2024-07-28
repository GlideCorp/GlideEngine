
namespace Core.Locations
{
    public abstract class Trackable(Location location) : IComparable<Trackable>
    {
        public Location Location { get; init; } = location;

        public override string ToString()
        {
            return $$"""
                {{nameof(Trackable)}}
                {
                    {{nameof(Location)}}: {{Location}}
                }
                """;
        }

        public int CompareTo(Trackable? other)
        {
            if (other == null) { return 1; }

            return Location.CompareTo(other.Location); 
        }
    }
}
