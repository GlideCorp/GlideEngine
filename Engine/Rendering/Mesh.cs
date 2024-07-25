using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;

namespace Engine.Rendering
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex()
    {
        public Vector3D<float> Position { get; set; }
        public Vector3D<float> Normal { get; set; }
    }

    public class Mesh
    {
        public uint VAO {  get; private set; }
        public uint IndicesCount { get; private set; }

        public static VertexLayout VertexLayout => new(
            new VertexElement(0, VertexAttribPointerType.Float, 3),
            new VertexElement(1, VertexAttribPointerType.Float, 3)
        );

        public Mesh(List<Vertex> vertices, List<uint> indices)
        {
            GL Gl = App.Gl;

            unsafe
            {
                VAO = Gl.GenVertexArray();
                Gl.BindVertexArray(VAO);

                uint VBO = Gl.GenBuffer();
                Gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);

                fixed (Vertex* data = vertices.ToArray())
                {
                    Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Count * VertexLayout.Stride), data, BufferUsageARB.StaticDraw);
                }

                /*			         Element 1						  Element 2					 */
                /*       | {Position: vec3, Normal: vec3} | {Position: vec3, Normal: vec3} | ... */
                /*Stride:|<---- VertexLayout.Stride  ---->|                 */
                /*Offset:  Pos: 0, Normal: PosOffset+Pos.Size             */

                uint offset = 0;
                foreach (VertexElement element in VertexLayout.Elements)
                {
                    Gl.VertexAttribPointer(element.Index, (int)element.Count, element.Type, false, VertexLayout.Stride, (void*)offset);
                    Gl.EnableVertexAttribArray(element.Index);

                    offset += element.Count * VertexElement.GetSizeOf(element.Type);
                }

                uint IBO = Gl.GenBuffer();
                Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, IBO);

                fixed (uint* buf = indices.ToArray())
                {
                    Gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)indices.Count * sizeof(int), buf, BufferUsageARB.StaticDraw);
                }

                Gl.BindVertexArray(0);
                Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
                Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
            }

            IndicesCount = (uint)indices.Count;
        }

    }
}
