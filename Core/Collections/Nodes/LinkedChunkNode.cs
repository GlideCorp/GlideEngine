
namespace Core.Collections.Nodes
{
    public class LinkedChunkNode<TValue>
    {
        public TValue[] Values { get; set; }
        public int NextItemIndex { get; set; }

        public LinkedChunkNode<TValue>? Previous { get; set; }
        public LinkedChunkNode<TValue>? Next { get; set; }

        public LinkedChunkNode(int size, TValue value)
        {
            Values = new TValue[size];
            Values[0] = value;
            NextItemIndex = 1;

            Previous = null;
            Next = null;
        }

        public LinkedChunkNode(int size, TValue value, LinkedChunkNode<TValue>? previous, LinkedChunkNode<TValue>? next)
        {
            Values = new TValue[size];
            Values[0] = value;
            NextItemIndex = 1;

            Previous = previous;
            Next = next;
        }
    }
}
