using Silk.NET.OpenGL;

namespace Engine.Rendering
{
    //TODO: Valutare se conviene tenere perennemente un riferimento a App.Gl
    public static class Graphics
    {
        public static void Clear()
        {
            Application.Context.Clear(ClearBufferMask.ColorBufferBit);
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
            Application.Context.BindVertexArray(mesh.VAO);
            Application.Context.DrawElements(PrimitiveType.Triangles, mesh.IndicesCount, DrawElementsType.UnsignedInt, (void*)0);
        }
    }
}
