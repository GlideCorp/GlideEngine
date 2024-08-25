using Core.Logs;
using Engine.Utilities;
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
        static int Index { get; set; }
        static FrameBuffer CurrentFrameBuffer { get => FrameBuffers[Index]; }
        static FrameBuffer[] FrameBuffers { get; set; }

        public static Stack<ScreenEffect> EffectsStack { get; private set; }


        static NegativeEffect ne = new NegativeEffect();
        static ScreenDrawEffect sd = new ScreenDrawEffect();

        static PostProcessing()
        {
            Vector2D<int> frameBufferSize = Application.FramebufferSize;

            FrameBuffers = new FrameBuffer[2];
            FrameBuffers[0] = new FrameBuffer(frameBufferSize.X, frameBufferSize.Y);
            FrameBuffers[1] = new FrameBuffer(frameBufferSize.X, frameBufferSize.Y);
            Index = 0;

            EffectsStack = new Stack<ScreenEffect>();
            //EffectsStack.Push(new ScreenDrawEffect());
        }

        public static void Push(ScreenEffect effect)
        {
            EffectsStack.Push(effect);
        }

        public static ScreenEffect Pop()
        {
            if (EffectsStack.Count <= 1) { return null; }

            return EffectsStack.Pop();
        }

        public static void Execute()
        {
            Index = 0;
            Graphics.CopyFrameBuffer(Application.FrameBuffer, CurrentFrameBuffer, ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Stack<ScreenEffect> runTimeStack = new Stack<ScreenEffect>(EffectsStack);
            while (runTimeStack.Count > 0)
            {
                ScreenEffect effect = runTimeStack.Pop();

                effect.RenderImage(FrameBuffers[Index], FrameBuffers[(Index+1)%2]);

                Index = (Index + 1) % 2;
            }

            Graphics.CopyFrameBuffer(CurrentFrameBuffer, 0, ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            /* NON TOGLIERE!!! La cosa sopra e questa fanno la stessa cosa, ma in teoria la versione sopra è meglio perchè non disgna mesh aggiuntive.
             * Onestamente però non ne sono sicuro, quindi lasciala qui per un po per sicurezza
            
            Application.Context.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            sd.Material.ScreenBuffer = CurrentFrameBuffer;
            Graphics.Clear();
            Graphics.Draw(MeshPrimitives.Quad, sd.Material);
            */
        }

        public static void Resize(Vector2D<int> newSize)
        {
            FrameBuffers[0].Dispose();
            FrameBuffers[1].Dispose();
            FrameBuffers[0] = new FrameBuffer(newSize.X, newSize.Y);
            FrameBuffers[1] = new FrameBuffer(newSize.X, newSize.Y);
        }
    }
}
