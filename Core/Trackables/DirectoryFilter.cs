
namespace Core.Trackables
{
    public class DirectoryFilter
    {
        public const char All = '*';
        public const char Not = '!';
        public const char And = '&';
        public const char Or = '|';

        public TrackableDirectory From { get; set; }

        public int MyProperty { get; set; }

        public DirectoryFilter(string from)
        {
            From = new(from);
        }
    }
}
