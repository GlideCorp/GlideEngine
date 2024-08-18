using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Rendering.PostProcessing
{
    public static class PostProcessing
    {
        private static TextureParameters _frameBufferParams;
        public static FrameBuffer CurrentFrameBuffer { get; private set; }
        public static FrameBuffer FrameBuffer1 { get; private set; }
        public static FrameBuffer FrameBuffer2 { get; private set; }

        public static Stack<Effect> EffectsStack { get; private set; }

        private static Mesh ScreenQuad { get; set; }

        static PostProcessing()
        {
            Vector2D<int> frameBufferSize = Application.FramebufferSize;
            _frameBufferParams = new(verticalWrap: TextureWrap.Clamp,
                                        horizontalWrap: TextureWrap.Clamp,
                                        minification: TextureFilter.Linear,
                                        magification: TextureFilter.Linear,
                                        mipmaps: false);

            FrameBuffer1 = new FrameBuffer(frameBufferSize.X, frameBufferSize.Y, _frameBufferParams);
            FrameBuffer2 = new FrameBuffer(frameBufferSize.X, frameBufferSize.Y, _frameBufferParams);
            CurrentFrameBuffer = FrameBuffer2;

            EffectsStack = new Stack<Effect>();
            EffectsStack.Push(new ScreenDrawEffect());

            ScreenQuad = new Mesh();
            ScreenQuad.Vertices = new List<Vertex>()
            {
                new(){ Position = new(1, 1, 0.0f)},
                new(){ Position = new (1, -1, 0.0f)},
                new(){ Position = new (-1, -1, 0.0f)},
                new(){ Position =  new(-1, 1, 0.0f)}
            };
            ScreenQuad.Indices = new List<uint>
            {
                0, 1, 3,  // first Triangle
                1, 2, 3   // second Triangle
            };
            ScreenQuad.Build();
        }

        public static void Push(Effect effect)
        {
            EffectsStack.Push(effect);
        }

        public static Effect Pop()
        {
            if (EffectsStack.Count <= 1) { return null; }

            return EffectsStack.Pop();
        }


        public static void Execute()
        {
            Graphics.Clear();
            Stack<Effect> runTimeStack = new Stack<Effect>(EffectsStack);

            while (runTimeStack.Count > 1)
            {
                Effect effect = runTimeStack.Pop();
                effect.Apply(CurrentFrameBuffer);

                CurrentFrameBuffer = (CurrentFrameBuffer == FrameBuffer1) ? FrameBuffer2 : FrameBuffer1;

                CurrentFrameBuffer.Bind();
                Graphics.Clear();
                Graphics.Draw(ScreenQuad);
                CurrentFrameBuffer.Unbind();
            }

            //L'ultimo effetto per comodità viene "applicato" direttamente a schermo
            Effect e = runTimeStack.Pop();
            Application.Context.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            e.Apply(CurrentFrameBuffer);
            Graphics.Clear();
            Graphics.Draw(ScreenQuad);
        }

        public static void Resize(Vector2D<int> newSize)
        {
            if (FrameBuffer1 == null) { return; }
            if (FrameBuffer2 == null) { return; }

            FrameBuffer1.Dispose();
            FrameBuffer2.Dispose();
            FrameBuffer1 = new FrameBuffer(newSize.X, newSize.Y, _frameBufferParams);
            FrameBuffer2 = new FrameBuffer(newSize.X, newSize.Y, _frameBufferParams);
        }
    }
}
