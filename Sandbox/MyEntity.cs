using Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    public class MyEntity : Entity
    {
    }

    public class MyComp : Component
    {
        public int Val;
        public MyComp() : base("test", "test")
        {
            Val = Random.Shared.Next(0, 100);
        }

        public override string ToString()
        {
            return $"{base.ToString()} {{ Val:{Val} }}";
        }
    }

    public class MyBehaviour : Behaviour
    {
        public MyBehaviour() : base("test", "test", [])
        {
        }
    }
}
