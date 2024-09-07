
using Core.Traceable;

namespace Engine.Entities
{
    public abstract class Component : Trackable
    {
        protected Component(string name) : base($"component:{name}")
        {
        }

        public override string ToString()
        {
            return GetType().Name.ToString();
        }
    }
}
