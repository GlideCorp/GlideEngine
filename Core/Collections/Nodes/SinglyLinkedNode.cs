
using Core.Collections.Interfaces;

namespace Core.Collections.Nodes
{
    public class SinglyLinkedNode<TValue>(TValue value, SinglyLinkedNode<TValue>? next) : INode<TValue>
    {
        public TValue Value { get; set; } = value;
        public SinglyLinkedNode<TValue>? Next { get; set; } = next;

        public SinglyLinkedNode(TValue value) : this(value, null) { }
    }
}
