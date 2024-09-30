
using Core.Collections.LinkedLists.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Core.Collections.LinkedLists
{
    public class SinglyLinkedList<TValue>() : SinglyLinkedList<TValue, TValue>(new DefaultFilter<TValue>())
        where TValue : IComparable<TValue>, IEquatable<TValue>;

    public class SinglyLinkedList<TKey, TValue>(IFilter<TKey, TValue> defaultFilter) : ILinkedList<TKey, TValue, SinglyLinkedNode<TValue>>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; } = defaultFilter;

        public SinglyLinkedNode<TValue>? FirstNode { get; set; } = null;
        public SinglyLinkedNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; protected set; } = 0;
        public bool IsPacked { get; protected set; } = false;

        public TValue this[int index]
        {
            get
            {
                Debug.Assert(index >= 0 && index < Count);
                if (index == 0) { return FirstNode!.Value; }

                if (index == Count - 1) { return LastNode!.Value; }

                SinglyLinkedNode<TValue> previous = ReachPreviousNode(index);
                return previous.Next!.Value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SinglyLinkedNode<TValue> ReachPreviousNode(int index)
        {
            SinglyLinkedNode<TValue> previous = FirstNode!;
            index--;

            while (index > 0)
            {
                previous = previous.Next!;
                index--;
            }

            return previous;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertZero(TValue value)
        {
            FirstNode = LastNode = new(value);
            Count = 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertFirstNoCheck(TValue value)
        {
            FirstNode = new(value, next: FirstNode);
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertNextNoCheck(SinglyLinkedNode<TValue> previous, TValue value)
        {
            previous.Next = new(value, next: previous.Next);
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertLastNoCheck(TValue value)
        {
            SinglyLinkedNode<TValue> newNode = new(value);
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

            SinglyLinkedNode<TValue> previous = ReachPreviousNode(index);
            InsertNextNoCheck(previous, value);
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
            FirstNode = FirstNode!.Next;
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveNextNoChecks(SinglyLinkedNode<TValue> previous)
        {
            previous.Next = previous.Next!.Next;
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveLastNoChecks(SinglyLinkedNode<TValue> previous)
        {
            LastNode = previous;
            previous.Next = null;
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

            SinglyLinkedNode<TValue> previous = ReachPreviousNode(index: Count - 1);
            RemoveLastNoChecks(previous);
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

            SinglyLinkedNode<TValue> previous = FirstNode!;
            SinglyLinkedNode<TValue> next = FirstNode!.Next!;

            while (next != LastNode)
            {
                TValue value = next.Value;
                if (match(value)) { RemoveNextNoChecks(previous); return; }
                previous = next;
                next = next.Next!;
            }

            if (match(LastNode!.Value)) { RemoveLastNoChecks(previous); }
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

            SinglyLinkedNode<TValue> previous = ReachPreviousNode(index);
            if (previous.Next == LastNode) { RemoveLastNoChecks(previous); }
            else { RemoveNextNoChecks(previous); }
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
                    SinglyLinkedNode<TValue> current = FirstNode!.Next!;
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
            SinglyLinkedNode<TValue>? current = FirstNode;
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
