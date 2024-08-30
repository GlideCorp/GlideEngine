using Engine.Rendering.Effects;
using Engine.Utilities;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Drawing;
using System.Numerics;

namespace Engine.Rendering
{
    //TODO: Valutare se conviene tenere perennemente un riferimento a App.Gl
    public static class Graphics
    {
        public static Color ClearColor
        {
            set
            {
                Application.Context.ClearColor(value);
            }
        }

        public static void Clear()
        {
            Application.Context.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /*Da usare soltanto quando vogliamo forzare il rendering di una mesh con una shader particolare
            Ad esempio per lo shadowMapping*/
        public static void Draw(Mesh mesh, Material material)
        {
            material.Shader.Use();
            material.ApplyProperties();
            Draw(mesh);
        }

        public static unsafe void Draw(Mesh mesh)
        {
            if (mesh.IndicesCount > 0)
            {
                Application.Context.BindVertexArray(mesh.VAO);
                Application.Context.DrawElements(PrimitiveType.Triangles, mesh.IndicesCount, DrawElementsType.UnsignedInt, (void*)0);
            }
            else
            {
                DrawPrimitive(PrimitiveType.Triangles, mesh);
            }
        }


        public static void DrawPrimitive(PrimitiveType primitiveType, Mesh mesh)
        {
            Application.Context.DrawArrays(primitiveType, 0, mesh.VerticesCount);
        }

        public static void DrawPrimitive(PrimitiveType primitiveType, Mesh mesh, Shader shader)
        {
            shader.Use();
            DrawPrimitive(primitiveType, mesh);
        }

        public static void Blit(FrameBuffer source, FrameBuffer destination, ScreenMaterial material)
        {
            Application.Context.BindFramebuffer(FramebufferTarget.ReadFramebuffer, source.FrameBufferID);
            Application.Context.BindFramebuffer(FramebufferTarget.DrawFramebuffer, destination.FrameBufferID);

            material.ScreenBuffer = source;
            Draw(MeshPrimitives.Quad, material);

            CopyFrameBuffer(source, destination, ClearBufferMask.DepthBufferBit);
            /*
            Application.Context.BlitFramebuffer(0, 0, source.Width, source.Height,
                                                0, 0, destination.Width, destination.Height,
                                                ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);
             */
            Application.Context.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            
            /*
            Application.Context.BlitFramebuffer(0, 0, source.Width, source.Height,
                                                0, 0, destination.Width, destination.Height,
                                                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            */
        }

        public static void CopyFrameBuffer(FrameBuffer source, FrameBuffer destination, ClearBufferMask copyMask)
        {
            CopyFrameBuffer(source.FrameBufferID, destination.FrameBufferID,
                            new Vector2D<int>(0, 0), new Vector2D<int>(source.Width, source.Height),
                            new Vector2D<int>(0, 0), new Vector2D<int>(destination.Width, destination.Height),
                            copyMask);
        }

        public static void CopyFrameBuffer(uint source, FrameBuffer destination, ClearBufferMask copyMask)
        {
            CopyFrameBuffer(source, destination.FrameBufferID,
                            new Vector2D<int>(0, 0), new Vector2D<int>(destination.Width, destination.Height),
                            new Vector2D<int>(0, 0), new Vector2D<int>(destination.Width, destination.Height),
                            copyMask);
        }

        public static void CopyFrameBuffer(FrameBuffer source, uint destination, ClearBufferMask copyMask)
        {
            CopyFrameBuffer(source.FrameBufferID, destination,
                            new Vector2D<int>(0, 0), new Vector2D<int>(source.Width, source.Height),
                            new Vector2D<int>(0, 0), new Vector2D<int>(source.Width, source.Height),
                            copyMask);
        }
        public static void CopyFrameBuffer(uint source, uint destination, Vector2D<int>source0, Vector2D<int> source1, Vector2D<int> dest0, Vector2D<int> dest1, ClearBufferMask copyMask)
        {
            //Application.Context.BindFramebuffer(FramebufferTarget.ReadFramebuffer, source);
            //Application.Context.BindFramebuffer(FramebufferTarget.DrawFramebuffer, destination);

            Application.Context.BlitNamedFramebuffer(source, destination,
                                                        source0.X, source0.Y, source1.X, source1.Y,
                                                        dest0.X, dest0.Y, dest1.X, dest1.Y,
                                                        copyMask, BlitFramebufferFilter.Nearest);
            /*
            Application.Context.BlitFramebuffer(0, 0, source.Width, source.Height,
                                                0, 0, source.Width, source.Height,
                                                copyMask, BlitFramebufferFilter.Nearest);
            */
        }
    }
}
