
using Core.Collections.LinkedLists.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.LinkedLists
{
    public class OptimizedDoublyLinkedList<TValue>() : OptimizedDoublyLinkedList<TValue, TValue>(new DefaultFilter<TValue>())
        where TValue : IComparable<TValue>, IEquatable<TValue>;

    /// <summary>
    /// Tries to optimize the data structure introducing node pooling and modifying the order of the values to accelerate operations. <br/>
    ///
    /// Node pooling aims to reduce allocations. The pool can be cleared by calling <see cref="Pack"/>.                             <br/>
    ///
    /// The order of the list is modified moving the searched elements in the front of the list, so subsequent                      <br/>
    /// <see cref="Search(TKey,out TValue?)"/> calls will be faster if some elements are searched for often.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="defaultFilter"></param>
    public class OptimizedDoublyLinkedList<TKey, TValue>(IFilter<TKey, TValue> defaultFilter) : ILinkedList<TKey, TValue, DoublyLinkedNode<TValue>>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; } = defaultFilter;

        public DoublyLinkedNode<TValue>? FirstNode { get; set; } = null;
        public DoublyLinkedNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; private set; } = 0;
        public bool IsPacked { get; set; } = false;

        protected DoublyLinkedNode<TValue>? PoolHead { get; set; } = null;

        public TValue this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < Count);
                if (index == 0) { return FirstNode!.Value; }

                if (index == Count - 1) { return LastNode!.Value; }

                DoublyLinkedNode<TValue> current = FirstNode!.Next!;
                for (int i = 1; i < index; i++) { current = current.Next!; }
                return current.Value;
            }
        }

        private DoublyLinkedNode<TValue> CreateNode(TValue value, DoublyLinkedNode<TValue>? previous, DoublyLinkedNode<TValue>? next)
        {
            DoublyLinkedNode<TValue> newNode;
            if (PoolHead is null) { newNode = new(value, previous, next); }
            else
            {
                newNode = PoolHead;
                PoolHead = PoolHead.Next;
                newNode.Value = value;
                newNode.Previous = previous;
                newNode.Next = next;
            }

            return newNode;
        }

        private void InsertZero(TValue value)
        {
            FirstNode = LastNode = CreateNode(value, previous: null, next: null);
            Count = 1;
        }

        public void InsertFirst(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            DoublyLinkedNode<TValue> newNode = CreateNode(value, previous: null, next: FirstNode);
            FirstNode!.Previous = newNode;
            FirstNode = newNode;
            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            DoublyLinkedNode<TValue> newNode = CreateNode(value, previous: LastNode, next: null);
            LastNode!.Next = newNode;
            LastNode = newNode;
            Count++;
        }

        public void Insert(TValue value) { InsertFirst(value); }

        public void InsertAt(TValue value, int index)
        {
            Debug.Assert(index >= 0 && index <= Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (index == 0)
            {
                if (Count == 0) { InsertZero(value); }
                else { InsertFirst(value); }
                return;
            }

            if (index == Count) { InsertLast(value); return; }

            DoublyLinkedNode<TValue> previousNode = FirstNode!;
            for (int i = 1; i < index; i++) { previousNode = previousNode.Next!; }
            DoublyLinkedNode<TValue> newNode = CreateNode(value, previousNode, next: previousNode.Next);
            previousNode.Next = newNode;
            newNode.Next!.Previous = newNode;
        }

        private void RemoveZero()
        {
            FirstNode!.Next = PoolHead;
            PoolHead = FirstNode;
            FirstNode = LastNode = null;
            Count = 0;
        }

        private void RemoveFirstNoChecks()
        {
            DoublyLinkedNode<TValue> first = FirstNode!;
            FirstNode = FirstNode!.Next!;
            FirstNode.Previous = null;

            first.Next = PoolHead;
            PoolHead = first;
            Count--;
        }

        private void RemoveLastNoChecks()
        {
            LastNode!.Next = PoolHead;
            PoolHead = LastNode;

            LastNode = LastNode!.Previous!;
            LastNode.Next = null;

            PoolHead.Previous = null;
            Count--;
        }

        private void RemoveNextNoChecks(DoublyLinkedNode<TValue> previous)
        {
            DoublyLinkedNode<TValue> toRemove = previous.Next!;
            previous.Next = toRemove.Next;
            toRemove.Next!.Previous = previous;

            toRemove.Previous = null;
            toRemove.Next = PoolHead;
            PoolHead = toRemove;
            Count--;
        }

        public void RemoveFirst()
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (Count == 1) { RemoveZero(); return; }

            RemoveFirstNoChecks();
        }

        public void RemoveLast()
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (Count == 1) { RemoveZero(); return; }

            RemoveLastNoChecks();
        }

        public void Remove(TKey key)
        {
            DefaultFilter.Key = key;
            Remove(DefaultFilter);
        }

        public void Remove(IFilter<TKey, TValue> filter)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (filter.Match(FirstNode!.Value))
            {
                if (Count == 1) { RemoveZero(); }
                else { RemoveFirstNoChecks(); }
                return;
            }

            DoublyLinkedNode<TValue> previous = FirstNode;
            while (previous.Next != LastNode)
            {
                if (filter.Match(previous.Next!.Value)) { RemoveNextNoChecks(previous); return; }
                previous = previous.Next;
            }

            if (filter.Match(LastNode!.Value)) { RemoveLastNoChecks(); }
        }

        public void RemoveAt(int index)
        {
            Debug.Assert(index >= 0 && index < Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (index == 0)
            {
                if (Count == 1) { RemoveZero(); }
                else { RemoveFirst(); }
                return;
            }

            if (index == Count - 1) { RemoveLastNoChecks(); return; }

            DoublyLinkedNode<TValue> previous = FirstNode!;
            for (int i = 1; i < index; i++) { previous = previous.Next!; }
            RemoveNextNoChecks(previous);
        }

        public bool Search(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultFilter.Key = key;
            return Search(DefaultFilter, out value);
        }

        public bool Search(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            DoublyLinkedNode<TValue>? current = FirstNode;

            while (current is not null)
            {
                if (filter.Match(current.Value))
                {
                    value = current.Value!;
                    current.Value = FirstNode!.Value;
                    FirstNode.Value = value;
                    return true;
                }

                current = current.Next;
            }

            value = default;
            return false;
        }

        public void Clear()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        public void Pack()
        {
            IsPacked = true;
            PoolHead = null;
        }

        public IEnumerable<TValue> Filter(TKey key)
        {
            DefaultFilter.Key = key;
            return Filter(DefaultFilter);
        }

        public IEnumerable<TValue> Filter(IFilter<TKey, TValue> filter)
        {
            DoublyLinkedNode<TValue>? current = FirstNode;

            while (current is not null)
            {
                if (filter.Match(current.Value)) { yield return current.Value; }
                current = current.Next;
            }
        }
    }
}
