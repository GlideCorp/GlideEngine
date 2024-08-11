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

    //TODO: Rework mesh creation into:
    //          -Set mesh values/properties like mesh postprocessing flags
    //          -Set mesh vertices or/and indices
    //          -meshICreated.Build()
    //  This should make mesh much more flexile and more importantly reusable if needed
    public class Mesh
    {
        public uint VAO {  get; private set; }
        public uint VerticesCount { get; private set; }
        public uint IndicesCount { get; private set; }

        public static VertexLayout VertexLayout => new(
            new VertexElement(0, VertexAttribPointerType.Float, 3),
            new VertexElement(1, VertexAttribPointerType.Float, 3)
        );

        public Mesh(List<Vertex> vertices, List<uint> indices)
        {
            unsafe
            {
                VAO = Application.Context.GenVertexArray();
                Application.Context.BindVertexArray(VAO);

                uint VBO = Application.Context.GenBuffer();
                Application.Context.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);

                fixed (Vertex* data = vertices.ToArray())
                {
                    Application.Context.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Count * VertexLayout.Stride), data, BufferUsageARB.StaticDraw);
                }

                /*			         Element 1						  Element 2					 */
                /*       | {Position: vec3, Normal: vec3} | {Position: vec3, Normal: vec3} | ... */
                /*Stride:|<---- VertexLayout.Stride  ---->|                 */
                /*Offset:  Pos: 0, Normal: PosOffset+Pos.Size             */

                uint offset = 0;
                foreach (VertexElement element in VertexLayout.Elements)
                {
                    Application.Context.VertexAttribPointer(element.Index, (int)element.Count, element.Type, false, VertexLayout.Stride, (void*)offset);
                    Application.Context.EnableVertexAttribArray(element.Index);

                    offset += element.Count * VertexElement.GetSizeOf(element.Type);
                }

                uint IBO = Application.Context.GenBuffer();
                Application.Context.BindBuffer(BufferTargetARB.ElementArrayBuffer, IBO);

                fixed (uint* buf = indices.ToArray())
                {
                    Application.Context.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)indices.Count * sizeof(int), buf, BufferUsageARB.StaticDraw);
                }

                Application.Context.BindVertexArray(0);
                Application.Context.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
                Application.Context.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
            }

            VerticesCount = (uint)vertices.Count;
            IndicesCount = (uint)indices.Count;
        }

    }
}
