
using Core.Collections.LinkedLists.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Core.Collections.LinkedLists
{
    public class DoublyLinkedList<TValue>() : DoublyLinkedList<TValue, TValue>(new DefaultFilter<TValue>())
        where TValue : IComparable<TValue>, IEquatable<TValue>;

    public class DoublyLinkedList<TKey, TValue>(IFilter<TKey, TValue> defaultFilter) : ILinkedList<TKey, TValue, DoublyLinkedNode<TValue>>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; } = defaultFilter;

        public DoublyLinkedNode<TValue>? FirstNode { get; set; } = null;
        public DoublyLinkedNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; protected set; } = 0;
        public bool IsPacked { get; protected set; } = false;

        public TValue this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < Count);
                if (index == 0) { return FirstNode!.Value; }

                if (index == Count - 1) { return LastNode!.Value; }

                DoublyLinkedNode<TValue> current = ReachNode(index);
                return current.Value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private DoublyLinkedNode<TValue> ReachNode(int index)
        {
            int lengthFromFirst = index;
            int lengthFromLast = Count - 1 - index;

            DoublyLinkedNode<TValue> current;

            if (lengthFromFirst < lengthFromLast)
            {
                current = FirstNode!.Next!;
                while (lengthFromFirst > 0)
                {
                    current = current.Next!;
                    lengthFromFirst--;
                }
            }
            else
            {
                current = LastNode!.Previous!;
                while (lengthFromLast > 0)
                {
                    current = current.Previous!;
                    lengthFromLast--;
                }
            }

            return current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertZero(TValue value)
        {
            FirstNode = LastNode = new(value);
            Count = 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InsertFirstNoCheck(TValue value)
        {
            DoublyLinkedNode<TValue> newNode = new(value, previous: null, next: FirstNode);
            FirstNode!.Previous = newNode;
            FirstNode = newNode;
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InsertNoCheck(DoublyLinkedNode<TValue> current, TValue value)
        {
            DoublyLinkedNode<TValue> newNode = new(value, previous: current.Previous, next: current);
            current.Previous!.Next = newNode;
            current.Previous = newNode;
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InsertLastNoCheck(TValue value)
        {
            DoublyLinkedNode<TValue> newNode = new(value, previous: LastNode, next: null);
            LastNode!.Next = newNode;
            LastNode = newNode;
            Count++;
        }

        public void InsertFirst(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            InsertFirstNoCheck(value);
        }

        public void InsertLast(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            InsertLastNoCheck(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(TValue value) { InsertFirst(value); }

        public void InsertAt(TValue value, int index)
        {
            Debug.Assert(index >= 0 && index <= Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (index == 0)
            {
                if (Count == 0) { InsertZero(value); }
                else { InsertFirstNoCheck(value); }
                return;
            }

            if (index == Count) { InsertLastNoCheck(value); return; }

            DoublyLinkedNode<TValue> current = ReachNode(index);
            InsertNoCheck(current, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveZero()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveFirstNoChecks()
        {
            FirstNode = FirstNode!.Next!;
            FirstNode.Previous = null;
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveCurrentNoChecks(DoublyLinkedNode<TValue> current)
        {
            current.Previous!.Next = current.Next;
            current.Next!.Previous = current.Previous;
            current.Previous = current.Next = null;
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveLastNoChecks()
        {
            LastNode = LastNode!.Previous!;
            LastNode.Next = null;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(TKey key)
        {
            DefaultFilter.Key = key;
            Remove(DefaultFilter);
        }

        public void Remove(IFilter<TKey, TValue> filter)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            Predicate<TValue> match = filter.Match;

            if (match(FirstNode!.Value))
            {
                if (Count == 1) { RemoveZero(); }
                else { RemoveFirstNoChecks(); }
                return;
            }

            if (Count == 1) { return; }

            DoublyLinkedNode<TValue> current = FirstNode!.Next!;
            while (current != LastNode)
            {
                if (match(current.Value))
                {
                    RemoveCurrentNoChecks(current);
                    return;
                }
                current = current.Next!;
            }

            if (match(LastNode.Value)) { RemoveLastNoChecks(); }
        }

        public void RemoveAt(int index)
        {
            Debug.Assert(index >= 0 && index < Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (index == 0)
            {
                if (Count == 1) { RemoveZero(); }
                else { RemoveFirstNoChecks(); }
                return;
            }

            if (index == Count - 1) { RemoveLastNoChecks(); }

            DoublyLinkedNode<TValue> current = ReachNode(index);
            RemoveCurrentNoChecks(current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Search(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultFilter.Key = key;
            return Search(DefaultFilter, out value);
        }

        public bool Search(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            if (Count != 0)
            {
                Predicate<TValue> match = filter.Match;

                if (match(FirstNode!.Value))
                {
                    value = FirstNode!.Value!;
                    return true;
                }

                if (Count > 1)
                {
                    DoublyLinkedNode<TValue> current = FirstNode!.Next!;
                    while (current != LastNode)
                    {
                        value = current.Value!;
                        if (match(value)) { return true; }
                        current = current.Next!;
                    }

                    if (match(LastNode!.Value))
                    {
                        value = LastNode!.Value!;
                        return true;
                    }
                }
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
            Func<TValue, bool> match = filter.Match;

            while (current is not null)
            {
                TValue value = current.Value!;
                if (match(value)) { yield return value; }
                current = current.Next;
            }
        }
    }
}