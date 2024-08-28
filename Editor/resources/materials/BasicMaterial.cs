using Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.resources.materials
{
    public class BasicMaterial : Material
    {
        public Color DiffuseColor { get; set; }
        public float Shininess { get; set; }

        public BasicMaterial() : base("basic")
        {
        }

        public override void ApplyProperties()
        {
            Shader.SetColor("uDiffuseColor", DiffuseColor);
            Shader.SetFloat("uShininess", Shininess);
        }
    }
}
