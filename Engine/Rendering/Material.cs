using Engine.Utilities;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering
{
    public abstract class Material : IResource
    {

        public Shader Shader { get; private set; }

        public Material(string shaderName)
        {
            Shader = ShaderDatabase.Load(shaderName);
        }

        public abstract void ApplyProperties();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            Shader.Dispose();
        }
    }
}
