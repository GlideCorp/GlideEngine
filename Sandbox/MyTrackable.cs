using Core.Locations;

namespace Sandbox
{
    public class MyTrackable(int value) : Trackable(name: "Test", path: "a:b:c:d")
    {
        public int Value { get; set; } = value;
    }
}
