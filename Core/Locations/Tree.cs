
using System.Diagnostics.CodeAnalysis;

namespace Core.Locations
{
    public class Tree
    {
        private Node Root { get; init; }
        public Location Filter { get; init; }
        public int MaxDepth { get; init; }

        public Tree()
        {
            Root = new();
            Filter = Location.Empty;
            MaxDepth = int.MaxValue;
        }

        public Tree(int maxDepth)
        {
            Root = new();
            Filter = Location.Empty;
            MaxDepth = maxDepth;
        }

        public Tree(Location filter)
        {
            Root = new();
            Filter = filter;
            MaxDepth = int.MaxValue;
        }

        public Tree(Location filter, int maxDepth)
        {
            Root = new();
            Filter = filter;
            MaxDepth = maxDepth;
        }

        public bool CanInsert(Location location) { return location.Depth <= MaxDepth && location.Match(Filter); }

        private bool Explore(Location location, bool generate, [NotNullWhen(true)] out Node? node)
        {
            Node current = Root;
            LocationSlice slice = new(location, Filter.Depth);

            foreach (ReadOnlyMemory<char> subpath in slice.Slide())
            {
                if (current.Find(subpath, out int index, out Node? temp)) { current = temp; }
                else if (generate && current.Append(subpath, index, out temp)) { current = temp; }
                else
                {
                    node = null;
                    return false;
                }
            }

            node = current;
            return true;
        }

        public bool Insert(Trackable trackable)
        {
            if (!CanInsert(trackable.Location)) { return false; }

            if (Explore(trackable.Location, generate: true, out Node? current))
            {
                current.Value = trackable;
                return true;
            }

            return false;
        }

        public bool Remove(Location location)
        {
            if (!CanInsert(location)) { return false; }

            if (Explore(location, generate: false, out Node? current))
            {
                current.Value = null;
                return true;
            }

            return false;
        }

        public IEnumerable<Trackable> RetriveValues()
        {
            return Root.RetriveValues();
        }

        public IEnumerable<Trackable> RetriveValues(Location location)
        {
            if (CanInsert(location))
            {
                if (Explore(location, generate: false, out Node? current))
                {
                    return current.RetriveValues();
                }
            }

            return [];
        }
    }
}
