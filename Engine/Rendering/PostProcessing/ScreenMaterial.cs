using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering.PostProcessing
{
    public class ScreenMaterial : Material
    {
        public FrameBuffer? ScreenBuffer;

        public ScreenMaterial(Shader shader) : base(shader)
        {
        }

        public override void Apply()
        {
            base.Apply();

            if(ScreenBuffer == null ) { return; }

            ScreenBuffer.Color.Bind(0);
            ScreenBuffer.Depth.Bind(1);
            Shader.SetInt("uColorBuffer", ScreenBuffer.Color.CurrentUnit);
            Shader.SetInt("uDepthBuffer", ScreenBuffer.Depth.CurrentUnit);
        }
    }
}
