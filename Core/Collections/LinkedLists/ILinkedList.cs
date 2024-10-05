
namespace Core.Collections.LinkedLists
{
    /// <summary>
    /// Linked list is a linear collection of data elements whose order is not given by their physical placement in memory. <br/>
    /// Instead, each element points to the next. It is a data structure consisting of a collection of nodes which together <br/>
    /// represent a sequence. This structure allows for efficient insertion or removal of elements from any position in the <br/>
    /// sequence during iteration. Because nodes are serially linked, accessing any node requires that the prior node be    <br/>
    /// accessed beforehand.
    /// <see href="https://en.wikipedia.org/wiki/Linked_list">Wikipedia</see>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TNode"></typeparam>
    public interface ILinkedList<TValue, TNode> : ICollection<TValue>
    {
        /// <summary>
        /// The first node of the linked list
        /// </summary>
        public TNode? FirstNode { get; set; }

        /// <summary>
        /// The last node of the linked list
        /// </summary>
        public TNode? LastNode { get; set; }
    }
}
