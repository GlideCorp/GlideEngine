
namespace Core.Collections.Nodes
{
    public class DoublyLinkedNode<TValue> 
        where TValue : notnull
    {
        public TValue Value;
        public DoublyLinkedNode<TValue> Previous;
        public DoublyLinkedNode<TValue> Next;

        public DoublyLinkedNode(TValue value)
        {
            Value = value;
            Previous = this;
            Next = this;
        }

        public DoublyLinkedNode(TValue value, DoublyLinkedNode<TValue> previous, DoublyLinkedNode<TValue> next)
        {
            Value = value;
            Previous = previous;
            Next = next;
        }
    }
}
