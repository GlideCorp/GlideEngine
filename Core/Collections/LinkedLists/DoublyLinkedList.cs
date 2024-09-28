
using Core.Collections.LinkedLists.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.LinkedLists
{
    public class DoublyLinkedList<TValue>() : DoublyLinkedList<TValue, TValue>(new DefaultFilter<TValue>())
        where TValue : IComparable<TValue>, IEquatable<TValue>;

    public class DoublyLinkedList<TKey, TValue>(IFilter<TKey, TValue> defaultFilter) : ILinkedList<TKey, TValue, DoublyLinkedNode<TValue>>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; } = defaultFilter;

        public DoublyLinkedNode<TValue>? FirstNode { get; set; } = null;
        public DoublyLinkedNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; private set; } = 0;
        public bool IsPacked { get; set; } = false;

        public TValue this[int index]
        {
            get
            {
                Debug.Assert(index < Count);
                if (index == 0) { return FirstNode!.Value; }

                if (index == Count - 1) { return LastNode!.Value; }

                DoublyLinkedNode<TValue> current = FirstNode!.Next!;
                for (int i = 1; i < index; i++) { current = current.Next!; }
                return current.Value;
            }
        }

        private void InsertZero(TValue value)
        {
            FirstNode = LastNode = new(value);
            Count = 1;
        }

        public void InsertFirst(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            DoublyLinkedNode<TValue> newNode = new(value, previous: null, next: FirstNode) { Previous = FirstNode };
            FirstNode = newNode;
            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            DoublyLinkedNode<TValue> newNode = new(value, previous: LastNode, next: null);
            LastNode!.Next = newNode;
            LastNode = newNode;
            Count++;
        }

        public void Insert(TValue value) { InsertFirst(value); }

        public void InsertAt(TValue value, int index)
        {
            Debug.Assert(index <= Count);
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
            DoublyLinkedNode<TValue> newNode = new(value, previousNode, next: previousNode.Next);
            previousNode.Next = newNode;
            newNode.Next!.Previous = newNode;
        }

        private void RemoveZero()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        private void RemoveFirstNoChecks()
        {
            FirstNode = FirstNode!.Next!;
            FirstNode.Previous = null;
            Count--;
        }

        private void RemoveLastNoChecks()
        {
            LastNode = LastNode!.Previous!;
            LastNode.Next = null;
            Count--;
        }

        private void RemoveCurrentNoChecks(DoublyLinkedNode<TValue> current)
        {
            current.Previous!.Next = current.Next;
            current.Next!.Previous = current.Previous;
            current.Previous = current.Next = null;
            Count--;
        }

        public void RemoveFirst()
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if(Count == 0) { return; }

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

            if (Count == 1) { return; }

            DoublyLinkedNode<TValue> current = FirstNode.Next!;
            while (current != LastNode)
            {
                if (filter.Match(current.Value)) { RemoveCurrentNoChecks(current); return; }
                current = current.Next!;
            }

            if (filter.Match(LastNode!.Value)) { RemoveLastNoChecks(); }
        }

        public void RemoveAt(int index)
        {
            Debug.Assert(index <= Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (index == 0)
            {
                if (Count == 1) { RemoveZero(); }
                else { RemoveFirst(); }
                return;
            }

            if (index == Count - 1) { RemoveLastNoChecks(); return; }

            DoublyLinkedNode<TValue> current = FirstNode!.Next!;
            for (int i = 1; i < index; i++) { current = current.Next!; }
            RemoveCurrentNoChecks(current);
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
                    return true;
                }

                current = current.Next!;
            }

            value = default;
            return false;
        }

        public void Clear()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        public void Pack() { IsPacked = true; }

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