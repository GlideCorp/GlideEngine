using Core.Maths.Vectors;
using Engine.Collections;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Runtime.InteropServices;

namespace Engine.Rendering
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vertex()
    {
        public Vector3Float Position { get; set; }
        public Vector3Float Normal { get; set; }
        public Vector2Float UV { get; set; }
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

        //public MeshDataBuffer Data { get; set; }
        
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
        
        public uint VerticesCount { get => (uint)Vertices.Count; }

        
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
        public uint IndicesCount { get => (uint)Indices.Count; }

        public Mesh()
        {
            //Data = new MeshDataBuffer();
        }

        public void Build()
        {
            //If trying to build an empty mesh dont do it, usefull if trying to rebuilding an already created mesh without setting vertices first
            if(VerticesCount == 0)
            {
                return;
            }

            unsafe
            {
                VAO = Application.Context.GenVertexArray();
                Application.Context.BindVertexArray(VAO);

                uint VBO = Application.Context.GenBuffer();
                Application.Context.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);

                Span<Vertex> vertexSpan = CollectionsMarshal.AsSpan(Vertices);
                ReadOnlySpan<Vertex> readOnlyVertexSpan = (ReadOnlySpan<Vertex>)vertexSpan;
                //ref Vertex data = ref MemoryMarshal.AsRef<Vertex>(readOnlyVertexSpan);
                Application.Context.BufferData(BufferTargetARB.ArrayBuffer, readOnlyVertexSpan, BufferUsageARB.StaticDraw);
                /*
                fixed (Vertex* data = Vertices.ToArray())
                {
                    Application.Context.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(Vertices.Count * VertexLayout.Stride), data, BufferUsageARB.StaticDraw);
                }
                */

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

                if (IndicesCount > 0)
                {
                    uint IBO = Application.Context.GenBuffer();
                    Application.Context.BindBuffer(BufferTargetARB.ElementArrayBuffer, IBO);

                    fixed (void* buf = Indices.ToArray())
                    {
                        Application.Context.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)IndicesCount * sizeof(uint), buf, BufferUsageARB.StaticDraw);
                    }
                }

                Application.Context.BindVertexArray(0);
                Application.Context.BindBuffer(BufferTargetARB.ArrayBuffer, 0);

                if (IndicesCount > 0)
                {
                    Application.Context.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
                }
            }

            //Vertices.Clear();
            //Indices.Clear();
        }

    }
}
