using Engine.Utilities;

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
