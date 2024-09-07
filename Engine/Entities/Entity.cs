
using Core.Traceable;

namespace Engine.Entities
{
    public abstract class Entity : Trackable
    {
        //protected SortedTree Registry { get; init; }

        public Entity(string name) : base($"entity:{name}")
        {
            //Registry = new();
        }

        public virtual void Load() { }

        public virtual void Start() { }

        public virtual void Update() { }

        public virtual void Draw()
        {
            //TODO: Check if meshComponent is present and draw automatically mesh
        }

        public virtual void Destroy() { }

        public void Add(Component component)
        {
            //Registry.Insert(component);
        }

        public void Add(Behaviour behaviour)
        {
            //Registry.Insert(behaviour);
        }

        public bool TryGetComponent<T>(out T? component) where T : Component
        {
            DirectoryFilter directoryFilter = new("component");
            /*
            foreach (var element in Registry.ListDepthValues(directoryFilter))
            {
                if (element.GetType() == typeof(T))
                {
                    component = (T)element;
                    return true;
                }
            }
            */
            component = null;
            return false;
        }

        public bool TryGetBehaviour<T>(out T? behaviour) where T : Behaviour
        {
            DirectoryFilter directoryFilter = new("behaviour");

            /*
            foreach (var element in Registry.ListDepthValues(directoryFilter))
            {
                if (element.GetType() == typeof(T))
                {
                    behaviour = (T)element;
                    return true;
                }
            }
            */

            behaviour = null;
            return false;
        }

        public Component[] GetAllComponents()
        {
            DirectoryFilter directoryFilter = new("behaviour");
            //return Registry.ListDepthValues(directoryFilter).Cast<Component>().ToArray();
            return null;
        }

        public Behaviour[] GetAllBehaviours()
        {
            DirectoryFilter directoryFilter = new("behaviour");
            //return Registry.ListDepthValues(directoryFilter).Cast<Behaviour>().ToArray();
            return null;
        }

        public override string ToString()
        {
            return GetType().Name.ToString();
        }
    }
}
