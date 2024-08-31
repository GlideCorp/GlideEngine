
namespace Core.Collections.Nodes
{
    public class ChunkLinkedNode<TValue>
    {
        public TValue[] Values { get; set; }
        public int NextItemIndex { get; set; }

        public ChunkLinkedNode<TValue>? Previous;
        public ChunkLinkedNode<TValue>? Next;

        public ChunkLinkedNode(int size, TValue value)
        {
            Values = new TValue[size];
            Values[0] = value;
            NextItemIndex = 1;

            Previous = null;
            Next = null;
        }

        public ChunkLinkedNode(int size, TValue value, ChunkLinkedNode<TValue>? previous, ChunkLinkedNode<TValue>? next)
        {
            Values = new TValue[size];
            Values[0] = value;
            NextItemIndex = 1;

            Previous = previous;
            Next = next;
        }
    }
}
