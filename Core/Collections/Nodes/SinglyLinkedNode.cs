
namespace Core.Collections.Nodes
{
    public class SinglyLinkedNode<TValue>(TValue value, SinglyLinkedNode<TValue>? next)
    {
        public TValue Value { get; set; } = value;
        public SinglyLinkedNode<TValue>? Next { get; set; } = next;

        public SinglyLinkedNode(TValue value) : this(value, null) { }
    }
}
