using Core.Locations;
using Core.Logs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public abstract class Entity
    {
        protected Tree Registry { get; init; }

        public Entity() 
        {
            Registry = new Tree();
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
            bool result = Registry.Insert(component);

            if (!result) 
            {
                Logger.Error($"{component} is already present into {this} at location {component.Location}");
            }
        }

        public void Add(Behaviour behaviour) 
        {
            bool result = Registry.Insert(behaviour);

            if (!result)
            {
                Logger.Error($"{behaviour} is already present into {this} at location {behaviour.Location}");
            }
        }

        public bool TryGetComponent<T>(out T? component) where T:Component 
        {
            Location componentLocation = new("component");
            var values = Registry.RetriveValues(componentLocation);

            foreach (var element in values)
            {
                if(element.GetType() == typeof(T))
                {
                    component = (T)element;
                    return true;
                }
            }

            component = null;
            return false; 
        }

        public bool TryGetBehaviour<T>(out T? behaviour) where T:Behaviour
        {
            Location behaviourLocation = new("behaviour");
            var values = Registry.RetriveValues(behaviourLocation);

            foreach (var element in values)
            {
                if (element.GetType() == typeof(T))
                {
                    behaviour = (T)element;
                    return true;
                }
            }

            behaviour = null;
            return false;
        }

        public Component[] GetAllComponents()
        {
            Location componentsLocation = new("component");
            return Registry.RetriveValues(componentsLocation).Cast<Component>().ToArray();
        }

        public Behaviour[] GetAllBehaviours()
        {
            Location behavioursLocation = new("behaviour");
            return Registry.RetriveValues(behavioursLocation).Cast<Behaviour>().ToArray();
        }

        public override string ToString()
        {
            return GetType().Name.ToString();
        }
    }
}
