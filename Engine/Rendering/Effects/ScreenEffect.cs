﻿
namespace Engine.Rendering.Effects
{
    public abstract class ScreenEffect
    {
        private ScreenMaterial MainMaterial { get; set; }

        protected ScreenEffect(string effectName)
        {
            MainMaterial = new ScreenMaterial(effectName);
        }

        public virtual void RenderImage(FrameBuffer source, FrameBuffer destination)
        {
            MainMaterial.ScreenBuffer = source;
            Graphics.Blit(source, destination, MainMaterial);
        }
    }

    public class DrawDepthEffect : ScreenEffect
    {
        public DrawDepthEffect() : base("pp_linearDepth")
        {
        }
    }

    public class SimpleFogEffect : ScreenEffect
    {
        public SimpleFogEffect() : base("pp_simpleFog")
        {
        }
    }
    public class ScreenDrawEffect : ScreenEffect
    {
        public ScreenDrawEffect() : base("pp_screenDraw")
        {
        }
    }
}
