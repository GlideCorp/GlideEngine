
using System.Runtime.InteropServices;
using Core.Maths.Vectors;

namespace Engine.Collections
{
    public class MeshDataBuffer()
    {
        public byte[] Vertices { get; set; } = [];
        public byte[] Indices { get; set; } = [];

        protected int VertexCursor { get; set; } = 0;
        protected int IndexCursor { get; set; } = 0;

        public int VertexCount { get; protected set; } = 0;
        public int IndexCount { get; protected set; } = 0;

        protected int GrowthFactor { get; set; } = 2;
        protected int ShrinkFactor { get; set; } = 2;

        private int Growth(int size) { return Math.Max(size * GrowthFactor, 2); }
        private int Shrink(int size) { return size / ShrinkFactor; }

        protected bool EnsureVertexSpace(int quantity)
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

        protected bool EnsureIndexSpace(int quantity)
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
            bool resized = EnsureVertexSpace(typeSize);

            Span<byte> span = Vertices.AsSpan(VertexCursor, typeSize);
            MemoryMarshal.Write(span, value);

            //BinaryPrimitives.WriteSingleBigEndian();
            MemoryMarshal.Cast<byte, T>(Vertices.AsSpan(VertexCursor, VertexCursor + typeSize));
            MemoryMarshal.Cast<byte, T>(Vertices.AsSpan(VertexCursor, VertexCursor + typeSize));

            VertexCursor += typeSize;
            VertexCount++;
            return resized;
        }

        public bool InsertIndex<T>(T value) where T : struct
        {
            int typeSize = Marshal.SizeOf<T>();
            bool resized = EnsureIndexSpace(typeSize);

            MemoryMarshal.Write(Vertices.AsSpan(IndexCursor, IndexCursor + typeSize), value);

            IndexCursor += typeSize;
            IndexCount++;
            return resized;
        }

        public bool InsertVertices<T>(Vector<T> vector)
            where T : struct, System.Numerics.INumber<T> { return InsertVertices(vector.Values); }

        public bool InsertIndices<T>(Vector<T> vector)
            where T : struct, System.Numerics.INumber<T> { return InsertIndices(vector.Values); }

        public bool InsertVertices<T>(T[] vertices) where T : struct
        {
            int typeSize = Marshal.SizeOf<T>();
            bool resized = EnsureVertexSpace(typeSize * vertices.Length);

            for (int i = 0; i < vertices.Length; i++)
            {
                MemoryMarshal.Write(Vertices.AsSpan(VertexCursor, VertexCursor + typeSize), vertices[i]);

                VertexCursor += typeSize;
                VertexCount++;
            }

            return resized;
        }

        public bool InsertIndices<T>(T[] indices) where T : struct
        {
            int typeSize = Marshal.SizeOf<T>();
            bool resized = EnsureIndexSpace(typeSize * indices.Length);

            for (int i = 0; i < indices.Length; i++)
            {
                MemoryMarshal.Write(Indices.AsSpan(IndexCursor, IndexCursor + typeSize), indices[i]);

                IndexCursor += typeSize;
                IndexCount++;
            }

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
