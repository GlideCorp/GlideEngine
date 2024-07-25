namespace Core.Locations
{
    public abstract class Trackable
        : IComparable<Trackable>
    {
        public string Name { get; init; } 
        public Location Location { get; init; } 

        public Trackable(string name, Location location)
        {
            Name = name;
            Location = location;
        }

        public Trackable(string name, string path)
        {
            Name = name;
            Location = new(path);
        }

        public override string ToString()
        {
            return $$"""
                {{nameof(Trackable)}}
                {
                    {{nameof(Name)}}: <{{Name}}>,
                    {{nameof(Location)}}: {{Location}}
                }
                """;
        }

        public int CompareTo(Trackable? other)
        {
            if (other == null) { return 1; }

            int comp = Name.CompareTo(other.Name);
            if (comp == 0) { comp = Location.CompareTo(other.Location); }
            return comp;
        }
    }
}
