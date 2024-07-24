
namespace Core.Locations
{
    public class Tree()
    {
        private Node Root { get; init; } = new();

        private Node Explore(Location location)
        {
            Node current = Root;
            for (int i = 0; i < location.Path.Length; i++)
            {
                current.Append(location.Path[i], out current);
            }

            return current;
        }

        public void Insert(ITrackable trackable)
        {
            Node current = Explore(trackable.Location);
            current.Insert(trackable);
        }

        public void Remove(ITrackable trackable)
        {
            Node current = Explore(trackable.Location);
            current.Remove(trackable);
        }
    }
}
