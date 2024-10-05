
using Core.Collections.Interfaces;

namespace Core.Collections.Nodes
{
    public class DoublyLinkedChunkNode<TValue> : INode<TValue[]>
    {
        public TValue[] Value { get; set; }
        public int NextValueIndex { get; set; }

        public DoublyLinkedChunkNode<TValue>? Previous { get; set; }
        public DoublyLinkedChunkNode<TValue>? Next { get; set; }

        public DoublyLinkedChunkNode(int size, TValue value)
        {
            Value = new TValue[size];
            Value[0] = value;
            NextValueIndex = 1;

            Previous = null;
            Next = null;
        }

        public DoublyLinkedChunkNode(int size, TValue value, DoublyLinkedChunkNode<TValue>? previous, DoublyLinkedChunkNode<TValue>? next)
        {
            Value = new TValue[size];
            Value[0] = value;
            NextValueIndex = 1;

            Previous = previous;
            Next = next;
        }
    }
}
