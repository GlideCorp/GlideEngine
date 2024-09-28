namespace Core.Collections.LinkedLists.Nodes
{
    public class DoublyLinkedNode<TValue>(TValue value, DoublyLinkedNode<TValue>? previous, DoublyLinkedNode<TValue>? next)
    {
        public TValue Value { get; set; } = value;
        public DoublyLinkedNode<TValue>? Previous { get; set; } = previous;
        public DoublyLinkedNode<TValue>? Next { get; set; } = next;

        public DoublyLinkedNode(TValue value) : this(value, null, null) { }
    }
}
