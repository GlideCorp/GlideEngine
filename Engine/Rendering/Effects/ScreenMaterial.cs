
namespace Engine.Rendering.Effects
{
    public class ScreenMaterial : Material
    {
        public FrameBuffer? ScreenBuffer;

        public ScreenMaterial(string shaderName) : base(shaderName)
        {
        }

        public override void ApplyProperties()
        {
            if(ScreenBuffer == null ) { return; }

            ScreenBuffer.Color.Bind(0);
            ScreenBuffer.Depth.Bind(1);
            Shader.SetInt("uColorBuffer", ScreenBuffer.Color.CurrentUnit);
            Shader.SetInt("uDepthBuffer", ScreenBuffer.Depth.CurrentUnit);
        }
    }
}
