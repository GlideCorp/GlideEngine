using Silk.NET.OpenGL;

namespace Engine.Rendering
{
    //TODO: Valutare se conviene tenere perennemente un riferimento a App.Gl
    public static class Graphics
    {
        public static void Clear()
        {
            GL Gl = App.Gl;

            Gl.Clear(ClearBufferMask.ColorBufferBit);
        }

        /*Da usare soltanto quando vogliamo forzare il rendering di una mesh con una shader particolare
            Ad esempio per lo shadowMapping*/
        public static void Draw(Mesh mesh, Shader shader)
        {
            shader.Use();
            Draw(mesh);
        }

        public static unsafe void Draw(Mesh mesh)
        {
            GL Gl = App.Gl;

            Gl.BindVertexArray(mesh.VAO);
            Gl.DrawElements(PrimitiveType.Triangles, mesh.IndicesCount, DrawElementsType.UnsignedInt, (void*)0);
        }
    }
}
