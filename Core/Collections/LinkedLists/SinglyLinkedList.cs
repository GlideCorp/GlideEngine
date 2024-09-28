
using Core.Collections.LinkedLists.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.LinkedLists
{
    public class SinglyLinkedList<TValue>() : SinglyLinkedList<TValue, TValue>(new DefaultFilter<TValue>())
        where TValue : IComparable<TValue>, IEquatable<TValue>;

    public class SinglyLinkedList<TKey, TValue>(IFilter<TKey, TValue> defaultFilter) : ILinkedList<TKey, TValue, SinglyLinkedNode<TValue>>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; } = defaultFilter;

        public SinglyLinkedNode<TValue>? FirstNode { get; set; } = null;
        public SinglyLinkedNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; private set; } = 0;
        public bool IsPacked { get; set; } = false;

        public TValue this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < Count);
                if (index == 0) { return FirstNode!.Value; }

                if (index == Count - 1) { return LastNode!.Value; }
               
                SinglyLinkedNode<TValue> current = FirstNode!.Next!;
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

            FirstNode = new(value, next: FirstNode);
            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            SinglyLinkedNode<TValue> newNode = new(value);
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

            SinglyLinkedNode<TValue> previousNode = FirstNode!;
            for (int i = 1; i < index; i++) { previousNode = previousNode.Next!; }
            previousNode.Next = new(value, next: previousNode.Next);
        }

        private void RemoveZero()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        private void RemoveFirstNoChecks()
        {
            FirstNode = FirstNode!.Next;
            Count--;
        }

        private void RemoveLastNoChecks(SinglyLinkedNode<TValue> previous)
        {
            LastNode = previous;
            previous.Next = null;
            Count--;
        }

        private void RemoveNextNoChecks(SinglyLinkedNode<TValue> previous)
        {
            previous.Next = previous.Next!.Next;
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

            SinglyLinkedNode<TValue> previous = FirstNode!;
            while (previous.Next is not null) { previous = previous.Next; }
            RemoveLastNoChecks(previous);
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

            SinglyLinkedNode<TValue> previous = FirstNode;
            while (previous.Next != LastNode)
            {
                if (filter.Match(previous.Next!.Value)) { RemoveNextNoChecks(previous); return; }
                previous = previous.Next;
            }

            if (filter.Match(LastNode!.Value)) { RemoveLastNoChecks(previous); }
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

            SinglyLinkedNode<TValue> previous = FirstNode!;
            if (index == Count - 1)
            {
                while (previous.Next is not null) { previous = previous.Next; }
                RemoveLastNoChecks(previous);
            }
            else
            {
                for (int i = 1; i < index; i++) { previous = previous.Next!; }
                RemoveNextNoChecks(previous);
            }
        }

        public bool Search(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultFilter.Key = key;
            return Search(DefaultFilter, out value);
        }

        public bool Search(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            SinglyLinkedNode<TValue>? current = FirstNode;

            while (current is not null)
            {
                if (filter.Match(current.Value))
                {
                    value = current.Value!;
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

        public void Pack() { IsPacked = true; }

        public IEnumerable<TValue> Filter(TKey key)
        {
            DefaultFilter.Key = key;
            return Filter(DefaultFilter);
        }

        public IEnumerable<TValue> Filter(IFilter<TKey, TValue> filter)
        {
            SinglyLinkedNode<TValue>? current = FirstNode;

            while (current is not null)
            {
                if (filter.Match(current.Value)) { yield return current.Value; }
                current = current.Next;
            }
        }
    }
}
