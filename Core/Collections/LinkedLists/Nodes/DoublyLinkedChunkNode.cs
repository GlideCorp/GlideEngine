namespace Core.Collections.LinkedLists.Nodes
{
    public class DoublyLinkedChunkNode<TValue>
    {
        public TValue[] Values { get; set; }
        public int NextValueIndex { get; set; }

        public DoublyLinkedChunkNode<TValue>? Previous { get; set; }
        public DoublyLinkedChunkNode<TValue>? Next { get; set; }

        public DoublyLinkedChunkNode(int size, TValue value)
        {
            Values = new TValue[size];
            Values[0] = value;
            NextValueIndex = 1;

            Previous = null;
            Next = null;
        }

        public DoublyLinkedChunkNode(int size, TValue value, DoublyLinkedChunkNode<TValue>? previous, DoublyLinkedChunkNode<TValue>? next)
        {
            Values = new TValue[size];
            Values[0] = value;
            NextValueIndex = 1;

            Previous = previous;
            Next = next;
        }
    }
}
