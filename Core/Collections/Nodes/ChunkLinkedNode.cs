
namespace Core.Collections.Nodes
{
    public class ChunkLinkedNode<TValue>
    {
        public TValue[] Values { get; set; }
        public int Cursor { get; set; }

        public ChunkLinkedNode<TValue>? Previous;
        public ChunkLinkedNode<TValue>? Next;

        public ChunkLinkedNode(int size, TValue value)
        {
            Values = new TValue[size];
            Values[0] = value;
            Cursor = 1;

            Previous = null;
            Next = null;
        }

        public ChunkLinkedNode(int size, TValue value, ChunkLinkedNode<TValue>? previous, ChunkLinkedNode<TValue>? next)
        {
            Values = new TValue[size];
            Values[0] = value;
            Cursor = 1;

            Previous = previous;
            Next = next;
        }
    }
}
