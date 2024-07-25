using Silk.NET.OpenGL;

namespace Engine.Rendering
{
    public struct VertexElement
    {
        public uint Index { get; init; }
        public VertexAttribPointerType Type { get; init; }
        public uint Count { get; init; }

        public VertexElement(uint index,  VertexAttribPointerType type, uint count)
        {
            Index = index;
            Type = type;
            Count = count;
        }

        public static uint GetSizeOf(VertexAttribPointerType type)
        {
            return type switch
            {
                VertexAttribPointerType.Byte => sizeof(sbyte),
                VertexAttribPointerType.UnsignedByte => sizeof(byte),
                VertexAttribPointerType.Short => sizeof(short),
                VertexAttribPointerType.UnsignedShort => sizeof(ushort),
                VertexAttribPointerType.Int => sizeof(int),
                VertexAttribPointerType.UnsignedInt => sizeof(uint),
                VertexAttribPointerType.Float => sizeof(float),
                VertexAttribPointerType.Double => sizeof(double),
                _ => 0,
            };
        }
    }

    public struct VertexLayout
    {
        public VertexElement[] Elements { get; init; }
        public uint Stride { get; init; }

        public VertexLayout(params VertexElement[] elements)
        {
            Elements = elements;

            for (int i = 0; i < elements.Length; i++)
            {
                Stride += elements[i].Count * VertexElement.GetSizeOf(elements[i].Type);
            }
        }
    }

}
