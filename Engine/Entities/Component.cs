using Core.Locations;
using Core.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
