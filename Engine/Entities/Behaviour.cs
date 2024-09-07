
using Core.Traceable;

namespace Engine.Entities
{
    public abstract class Behaviour : Trackable
    {
        protected Behaviour(string name, params Component[] components) : base($"behaviour:{name}")
        {
        }

        public virtual void Update() { }

        public override string ToString()
        {
            return GetType().Name.ToString();
        }
    }
}
