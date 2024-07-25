using Core.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities
{
    public abstract class Behaviour : Trackable
    {
        protected Behaviour(string name, string path, params Component[] components) : base(name, $"behaviour:{path}")
        {
        }

        public virtual void Update() { }

        public override string ToString()
        {
            return GetType().Name.ToString();
        }
    }
}
