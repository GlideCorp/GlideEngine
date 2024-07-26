using Core.Locations;

namespace Sandbox
{
    public class MyTrackable(int value) : Trackable("a:b:c:d:MyTrackable")
    {
        public int Value { get; set; } = value;
    }
}
