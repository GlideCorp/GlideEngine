
namespace Core.Locations
{
    internal class LocationSlice
    {
        private Location Location { get; init; }

        public int From { get; init; }
        public int To { get; init; }

        public LocationSlice(Location location, int from, int to)
        {
            Location = location;
            From = from;
            To = to;
        }

        public LocationSlice(Location location, int from)
        {
            Location = location;
            From = from;
            To = location.Depth;
        }

        public IEnumerable<ReadOnlyMemory<char>> Slide()
        {
            for (int i = From; i < To; i++) { yield return Location[i]; }
        }
    }
}
