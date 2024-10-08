﻿using Silk.NET.OpenGL;
using Core.Maths.Vectors;

namespace Engine.Rendering.Effects
{
    public static class PostProcessing
    {        
        static int Index { get; set; }
        static FrameBuffer CurrentFrameBuffer { get => FrameBuffers[Index]; }
        static FrameBuffer[] FrameBuffers { get; set; }

        public static Stack<ScreenEffect> EffectsStack { get; private set; }
        
        static PostProcessing()
        {
            Vector2Int frameBufferSize = Application.FramebufferSize;

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
            Graphics.CopyFrameBuffer(Renderer.FrameBuffer, CurrentFrameBuffer, ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

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

        internal static void ResizeBuffers(Vector2Int newSize)
        {
            FrameBuffers[0].Dispose();
            FrameBuffers[1].Dispose();
            FrameBuffers[0] = new FrameBuffer(newSize.X, newSize.Y);
            FrameBuffers[1] = new FrameBuffer(newSize.X, newSize.Y);
        }
    }
}
