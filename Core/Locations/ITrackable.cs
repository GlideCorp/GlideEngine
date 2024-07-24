
namespace Core.Locations
{
    public interface ITrackable : IComparer<ITrackable>
    {
        public string Name { get; }
        public Location Location { get; }
    }
}
