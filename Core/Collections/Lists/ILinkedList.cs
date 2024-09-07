
namespace Core.Collections.Lists
{
    public interface ILinkedList<TKey, TValue, out TNode> : ICollection<TKey, TValue>
    {
        public TNode? FirstNode { get; }
        public TNode? LastNode { get; }
    }
}
