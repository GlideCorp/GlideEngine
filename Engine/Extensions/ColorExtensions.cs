using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ColorExtensions
    {
        public static Vector3 ToVec3(this Color c)
        {
            return new Vector3(c.R, c.G, c.B);
        }

        public static Vector4 ToVec4(this Color c)
        {
            return new Vector4(c.R, c.G, c.B, c.A);
        }
    }
}
