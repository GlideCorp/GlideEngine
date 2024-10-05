
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Interfaces
{
    /// <summary>
    /// Linked list is a linear collection of data elements whose order is not given by their physical placement in memory. <br/>
    /// Instead, each element points to the next. It is a data structure consisting of a collection of nodes which together <br/>
    /// represent a sequence. This structure allows for efficient insertion or removal of elements from any position in the <br/>
    /// sequence during iteration. Because nodes are serially linked, accessing any node requires that the prior node be    <br/>
    /// accessed beforehand.
    /// <see href="https://en.wikipedia.org/wiki/Linked_list">Wikipedia</see>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TNode"></typeparam>
    public interface ILinkedList<TValue, TNode> : ICollection<TValue>, IPackable
        where TNode : INode<TValue>
    {
        /// <summary>
        /// The first node of the linked list
        /// </summary>
        public TNode? FirstNode { get; set; }

        /// <summary>
        /// The last node of the linked list
        /// </summary>
        public TNode? LastNode { get; set; }

        /// <summary>
        /// Insert a value at the start of the list.
        /// </summary>
        /// <param name="value"></param>
        public void InsertFirst(TValue value);

        /// <summary>
        /// Insert a value at the end of the list.
        /// </summary>
        /// <param name="value"></param>
        public void InsertLast(TValue value);

        /// <summary>
        /// Insert the value at the specified index.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void InsertAt(TValue value, int index);

        /// <summary>
        /// Remove a value at the start of the list.
        /// </summary>
        public void RemoveFirst();

        /// <summary>
        /// Remove a value at the end of the list.
        /// </summary>
        public void RemoveLast();

        /// <summary>
        /// Remove the first value that matches the predicate.
        /// </summary>
        /// <param name="match"></param>
        public void Remove(Predicate<TValue> match);

        /// <summary>
        /// Remove all the values that match the predicate.
        /// </summary>
        /// <param name="match"></param>
        public void RemoveAll(Predicate<TValue> match);

        /// <summary>
        /// Remove the value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index);

        /// <summary>
        /// Search the first value that matches the predicate.
        /// </summary>
        /// <param name="match"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Search(Predicate<TValue> match, [NotNullWhen(true)] out TValue? value);

        /// <summary>
        /// Return all the matched values.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public void Filter<TCollection>(TCollection collection, Predicate<TValue> match) where TCollection : ICollection<TValue>;

        /// <summary>
        /// Returns the value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue ValueAt(int index);
    }
}
