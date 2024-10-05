
using Core.Collections.Interfaces;
using Core.Collections.Nodes;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Core.Collections.LinkedLists
{
    // DEFINITIONS
    public partial class DoublyLinkedList<TValue> : ILinkedList<TValue, DoublyLinkedNode<TValue>>
    {
        public DoublyLinkedNode<TValue>? FirstNode { get; set; } = null;
        public DoublyLinkedNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; protected set; } = 0;
        public bool IsPacked { get; protected set; } = false;
    }

    // PRIVATE FUNCTIONS
    public partial class DoublyLinkedList<TValue>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReachDirect(DoublyLinkedNode<TValue> current, int distance)
        {
            while (distance > 0)
            {
                current = current.Next!;
                distance--;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReachInverse(DoublyLinkedNode<TValue> current, int distance)
        {
            while (distance > 0)
            {
                current = current.Previous!;
                distance--;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private DoublyLinkedNode<TValue> ReachNode(int index)
        {
            Debug.Assert(index != 0 && index != Count - 1);
            int inverseDistance = Count - 1 - index;

            DoublyLinkedNode<TValue> current;

            if (index < inverseDistance) { current = FirstNode!.Next!; ReachDirect(current, index); }
            else { current = LastNode!.Previous!; ReachInverse(current, inverseDistance); }

            return current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ReachNode(Predicate<TValue> match, out DoublyLinkedNode<TValue> current)
        {
            current = FirstNode!.Next!;

            while (current != LastNode)
            {
                if (match(current.Value)) { return true; }
                current = current.Next!;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertFirstNoCheck(TValue value)
        {
            if (Count == 0) { FirstNode = LastNode = new(value); }
            else
            {
                Debug.Assert(Count > 0);
                DoublyLinkedNode<TValue> newNode = new(value, previous: null, next: FirstNode);
                FirstNode!.Previous = newNode;
                FirstNode = newNode;
            }

            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertNoCheck(DoublyLinkedNode<TValue> current, TValue value)
        {
            Debug.Assert(current != FirstNode);
            DoublyLinkedNode<TValue> newNode = new(value, previous: current.Previous, next: current);
            current.Previous!.Next = newNode;
            current.Previous = newNode;
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertLastNoCheck(TValue value)
        {
            if (Count == 0) { FirstNode = LastNode = new(value); }
            else
            {
                Debug.Assert(Count > 0);
                DoublyLinkedNode<TValue> newNode = new(value, previous: LastNode, next: null);
                LastNode!.Next = newNode;
                LastNode = newNode;
            }

            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveFirstNoChecks()
        {
            if (Count == 1) { FirstNode = LastNode = null; }
            else
            {
                Debug.Assert(Count > 1);
                FirstNode = FirstNode!.Next!;
                FirstNode.Previous = null;
            }

            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveCurrentNoChecks(DoublyLinkedNode<TValue> current)
        {
            Debug.Assert(Count > 2 && current != FirstNode && current != LastNode);
            current.Previous!.Next = current.Next;
            current.Next!.Previous = current.Previous;
            current.Previous = current.Next = null;
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveLastNoChecks()
        {
            if (Count == 1) { FirstNode = LastNode = null; }
            else
            {
                Debug.Assert(Count > 1);
                LastNode = LastNode!.Previous!;
                LastNode.Next = null;
            }

            Count--;
        }
    }

    // INSERT
    public partial class DoublyLinkedList<TValue>
    {
        public void InsertFirst(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }
            InsertFirstNoCheck(value);
        }

        public void InsertLast(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }
            InsertLastNoCheck(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(TValue value) { InsertFirst(value); }

        public void InsertAt(TValue value, int index)
        {
            Debug.Assert(index >= 0 && index <= Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (index == 0) { InsertFirstNoCheck(value); }
            else if (index == Count) { InsertLastNoCheck(value); }
            else
            {
                DoublyLinkedNode<TValue> previous = ReachNode(index);
                InsertNoCheck(previous, value);
            }
        }
    }

    // REMOVE
    public partial class DoublyLinkedList<TValue>
    {
        public void RemoveFirst()
        {
            if (IsPacked) { throw new InvalidOperationException(); }
            if (Count > 0) { RemoveFirstNoChecks(); }
        }

        public void RemoveLast()
        {
            if (IsPacked) { throw new InvalidOperationException(); }
            if (Count > 0) { RemoveLastNoChecks(); }
        }

        public void Remove(Predicate<TValue> match)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (match(FirstNode!.Value)) { RemoveFirstNoChecks(); }
            else if (Count > 1 && match(LastNode!.Value)) { RemoveLastNoChecks(); }
            else if (Count > 1 && ReachNode(match, out DoublyLinkedNode<TValue> current)) { RemoveCurrentNoChecks(current); }
        }

        public void RemoveAll(Predicate<TValue> match)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            while (Count > 0 && match(FirstNode!.Value)) { RemoveFirstNoChecks(); }

            if (Count == 0) { return; }

            DoublyLinkedNode<TValue> previous = FirstNode!;
            DoublyLinkedNode<TValue> next = previous.Next!;

            while (Count > 2 && next != LastNode)
            {
                if (match(next.Value)) { RemoveCurrentNoChecks(next); next = previous.Next!; }
                else { previous = next; next = next.Next!; }
            }

            if (Count > 0 && match(LastNode!.Value)) { RemoveLastNoChecks(); }
        }

        public void RemoveAt(int index)
        {
            Debug.Assert(index >= 0 && index < Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (index == 0) { RemoveFirstNoChecks(); }
            else if (index == Count - 1) { RemoveLastNoChecks(); }
            else
            {
                DoublyLinkedNode<TValue> current = ReachNode(index);
                RemoveCurrentNoChecks(current);
            }
        }
    }

    // SEARCH
    public partial class DoublyLinkedList<TValue>
    {
        public bool Search(Predicate<TValue> match, [NotNullWhen(true)] out TValue? value)
        {
            if (Count > 0)
            {
                if (match(FirstNode!.Value))
                {
                    value = FirstNode!.Value!;
                    return true;
                }

                if (Count > 1 && match(LastNode!.Value))
                {
                    value = LastNode.Value!;
                    return true;
                }

                if (Count > 2 && ReachNode(match, out DoublyLinkedNode<TValue>? current))
                {
                    value = current.Value!;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public void Filter<TCollection>(TCollection collection, Predicate<TValue> match) where TCollection : ICollection<TValue>
        {
            DoublyLinkedNode<TValue> current = FirstNode!;

            for (int i = 0; i < Count; i++)
            {
                if (match(current.Value)) { collection.Insert(current.Value); }
                current = current.Next!;
            }
        }
    }

    // OTHER
    public partial class DoublyLinkedList<TValue>
    {
        public TValue ValueAt(int index)
        {
            Debug.Assert(index >= 0 && index < Count);
            if (index == 0) { return FirstNode!.Value; }
            if (index == Count - 1) { return LastNode!.Value; }

            DoublyLinkedNode<TValue> current = ReachNode(index);
            return current.Value;
        }

        public void Clear()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        public void Pack() { IsPacked = true; }
    }
}