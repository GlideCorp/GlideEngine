
namespace Core.Collections.Nodes
{
    public class SinglyLinkedNode<TValue>
        where TValue : notnull
    {
        public TValue Value;
        public SinglyLinkedNode<TValue> Next;

        public SinglyLinkedNode(TValue value)
        {
            Value = value;
            Next = this;
        }

        public SinglyLinkedNode(TValue value, SinglyLinkedNode<TValue> next)
        {
            Value = value;
            Next = next;
        }
    }
}
