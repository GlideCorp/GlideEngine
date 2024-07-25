
namespace Core.Locations
{
    public class Tree
    {
        // FEATURE: remove filter part when appending new items
        private Node Root { get; init; }
        public Location Filter { get; init; }

        public Tree()
        {
            Root = new();
            Filter = Location.Empty;
        }
        public Tree(string path)
        {
            Root = new();
            Filter = new(path);
        }

        public Tree(Location filter)
        {
            Root = new();
            Filter = filter;
        }

        public bool CanInsert(Trackable trackable) { return trackable.Location.Match(Filter); }

        private Node Explore(Location location)
        {
            Node current = Root;
            for (int i = 0; i < location.Path.Length; i++)
            {
                current.Append(location.Path[i], out current);
            }

            return current;
        }

        public bool Insert(Trackable trackable)
        {
            if (!CanInsert(trackable)) { return false; }

            Node current = Explore(trackable.Location);
            current.Insert(trackable);
            return true;
        }

        public bool Remove(Trackable trackable)
        {
            if (!CanInsert(trackable)) { return false; }

            Node current = Explore(trackable.Location);
            return current.Remove(trackable);
        }

        public IEnumerable<Trackable> RetriveValues()
        {
            return Root.RetriveValues(recursive: true);
        }

        public IEnumerable<Trackable> RetriveValues(Location location)
        {
            Node current = Explore(location);
            return current.RetriveValues(recursive: true);
        }
    }
}
