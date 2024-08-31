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
        public Vector2D<float> UV { get; set; }
    }

    //TODO: Eventually add mesh-props like:
    //          -mesh properties like mesh postprocessing flags
    //          -mesh indices stride size (if float or half, ecc...)
    //  This should make mesh much more flexile and more importantly reusable if needed
    public class Mesh
    {
        public uint VAO {  get; private set; }

        public static VertexLayout VertexLayout => new(
            new VertexElement(0, VertexAttribPointerType.Float, 3),
            new VertexElement(1, VertexAttribPointerType.Float, 3),
            new VertexElement(2, VertexAttribPointerType.Float, 2)
        );

        private List<Vertex>? _vertices;
        public List<Vertex> Vertices 
        {
            get
            {
                if (_vertices is null)
                {
                    _vertices = new List<Vertex>();
                }

                return _vertices;
            }
            set
            {
                if(_vertices is null)
                {
                    _vertices = new List<Vertex>(value);
                }
            }
        }
        public uint VerticesCount { get; private set; }


        private List<uint>? _indices;
        public List<uint> Indices
        {
            get
            {
                if (_indices is null)
                {
                    _indices = new List<uint>();
                }

                return _indices;
            }
            set
            {
                if(_indices is null)
                {
                    _indices = new List<uint>(value);
                }
            }
        }
        public uint IndicesCount { get; private set; }

        public Mesh()
        {
        }

        public void Build()
        {
            //If trying to build an empty mesh dont do it, usefull if trying to rebuilding an already created mesh without setting vertices first
            if(Vertices.Count == 0)
            {
                return;
            }

            unsafe
            {
                VAO = Application.Context.GenVertexArray();
                Application.Context.BindVertexArray(VAO);

                uint VBO = Application.Context.GenBuffer();
                Application.Context.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);

                fixed (Vertex* data = Vertices.ToArray())
                {
                    Application.Context.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(Vertices.Count * VertexLayout.Stride), data, BufferUsageARB.StaticDraw);
                }

                /*			         Element 1						  Element 2					 */
                /*       | {Position: vec3, Normal: vec3} | {Position: vec3, Normal: vec3} | ... */
                /*Stride:|<---- VertexLayout.Stride  ---->|                 */
                /*Offset:  Pos: 0, Normal: PosOffset+Pos.Size             */

                uint offset = 0;
                foreach (VertexElement element in VertexLayout.Elements)
                {
                    Application.Context.EnableVertexAttribArray(element.Index);
                    Application.Context.VertexAttribPointer(element.Index, (int)element.Count, element.Type, false, VertexLayout.Stride, (void*)offset);

                    offset += element.Count * VertexElement.GetSizeOf(element.Type);
                }

                if (Indices.Count > 0)
                {
                    uint IBO = Application.Context.GenBuffer();
                    Application.Context.BindBuffer(BufferTargetARB.ElementArrayBuffer, IBO);

                    fixed (uint* buf = Indices.ToArray())
                    {
                        Application.Context.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)Indices.Count * sizeof(int), buf, BufferUsageARB.StaticDraw);
                    }
                }

                Application.Context.BindVertexArray(0);
                Application.Context.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

                if (Indices.Count > 0)
                {
                    Application.Context.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
                }
            }

            VerticesCount = (uint)Vertices.Count;
            IndicesCount = (uint)Indices.Count;

            Vertices.Clear();
            Indices.Clear();
        }

    }
}
