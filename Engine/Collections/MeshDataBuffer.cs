
using System.Runtime.InteropServices;

namespace Engine.Collections
{
    public class MeshDataBuffer()
    {
        public byte[] Vertices { get; set; } = [];
        public byte[] Indices { get; set; } = [];

        public int VertexCount { get; protected set; } = 0;
        public int IndexCount { get; protected set; } = 0;

        protected int GrowthFactor { get; set; } = 2;
        protected int ShrinkFactor { get; set; } = 2;

        private int Growth(int size) { return Math.Max(size * GrowthFactor, 2); }
        private int Shrink(int size) { return size / ShrinkFactor; }

        protected bool EnsureVerticesSpace(int quantity)
        {
            int newSize = Vertices.Length;
            while (Vertices.Length < VertexCount + quantity) { newSize = Growth(newSize); }

            if (newSize == Vertices.Length) { return false; }

            byte[] newVertices = new byte[newSize];

            Span<byte> vertexSpan = Vertices.AsSpan(0, VertexCount);
            Span<byte> newVertexSpan = newVertices.AsSpan(0, VertexCount);
            vertexSpan.CopyTo(newVertexSpan);

            Vertices = newVertices;
            return true;
        }

        protected bool EnsureIndicesSpace(int quantity)
        {
            int newSize = Indices.Length;
            while (Indices.Length < IndexCount + quantity) { newSize = Growth(newSize); }

            if (newSize == Indices.Length) { return false; }

            byte[] newIndices = new byte[newSize];

            Span<byte> indexSpan = Vertices.AsSpan(0, IndexCount);
            Span<byte> newIndexSpan = newIndices.AsSpan(0, IndexCount);
            indexSpan.CopyTo(newIndexSpan);

            Indices = newIndices;
            return true;
        }

        public bool InsertVertex<T>(T value) where T : struct
        {
            int typeSize = Marshal.SizeOf<T>();
            bool resized = EnsureVerticesSpace(typeSize);
            
            MemoryMarshal.Write(Vertices, value);
            VertexCount++;

            return resized;
        }

        public bool InsertIndex<T>(T value) where T : struct
        {
            int typeSize = Marshal.SizeOf<T>();
            bool resized = EnsureIndicesSpace(typeSize);

            MemoryMarshal.Write(Indices, value);
            IndexCount++;

            return resized;
        }

        public void ClearVertices()
        {
            Vertices = [];
            VertexCount = 0;
        }

        public void ClearIndices()
        {
            Indices = [];
            IndexCount = 0;
        }

        public void Clear()
        {
            ClearVertices();
            ClearIndices();
        }
    }
}
