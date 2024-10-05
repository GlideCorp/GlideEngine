
using Core.Collections.Interfaces;
using Core.Collections.Nodes;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Core.Collections.LinkedLists
{
    // DEFINITIONS
    public partial class SinglyLinkedList<TValue>() : ILinkedList<TValue, SinglyLinkedNode<TValue>>
    {
        public SinglyLinkedNode<TValue>? FirstNode { get; set; } = null;
        public SinglyLinkedNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; protected set; } = 0;
        public bool IsPacked { get; protected set; } = false;
    }

    // PRIVATE FUNCTIONS
    public partial class SinglyLinkedList<TValue>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SinglyLinkedNode<TValue> ReachPreviousNode(int index)
        {
            Debug.Assert(index != 0 && index != Count - 1);
            SinglyLinkedNode<TValue> previous = FirstNode!;

            while (index > 1) { previous = previous.Next!; index--; }
            return previous;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ReachPreviousNode(Predicate<TValue> match, out SinglyLinkedNode<TValue> previous)
        {
            Debug.Assert(Count > 2);
            previous = FirstNode!;
            SinglyLinkedNode<TValue> next = previous.Next!;

            while (next != LastNode)
            {
                if (match(next.Value)) { return true; }
                previous = next;
                next = next.Next!;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertFirstNoCheck(TValue value)
        {
            if (Count == 0) { FirstNode = LastNode = new(value); }
            else { Debug.Assert(Count > 0); FirstNode = new(value, next: FirstNode); }
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertNextNoCheck(SinglyLinkedNode<TValue> previous, TValue value)
        {
            Debug.Assert(previous != LastNode);
            previous.Next = new(value, next: previous.Next);
            Count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void InsertLastNoCheck(TValue value)
        {
            if (Count == 0) { FirstNode = LastNode = new(value); }
            else
            {
                Debug.Assert(Count > 0);
                SinglyLinkedNode<TValue> newNode = new(value);
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
                FirstNode = FirstNode!.Next;
            }

            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveNextNoChecks(SinglyLinkedNode<TValue> previous)
        {
            Debug.Assert(previous.Next != LastNode);
            previous.Next = previous.Next!.Next;
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveLastNoChecks(SinglyLinkedNode<TValue> previous)
        {
            if (Count == 1) { FirstNode = LastNode = null; }
            else
            {
                Debug.Assert(Count > 1 && previous.Next == LastNode);
                LastNode = previous;
                previous.Next = null;
            }

            Count--;
        }
    }

    // INSERT
    public partial class SinglyLinkedList<TValue>
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
                SinglyLinkedNode<TValue> previous = ReachPreviousNode(index);
                InsertNextNoCheck(previous, value);
            }
        }
    }

    // REMOVE
    public partial class SinglyLinkedList<TValue>
    {
        public void RemoveFirst()
        {
            if (IsPacked) { throw new InvalidOperationException(); }
            if (Count > 0) { RemoveFirstNoChecks(); }
        }

        public void RemoveLast()
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count > 0)
            {
                SinglyLinkedNode<TValue> previous = ReachPreviousNode(index: Count - 1);
                RemoveLastNoChecks(previous);
            }
        }

        public void Remove(Predicate<TValue> match)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (match(FirstNode!.Value)) { RemoveFirstNoChecks(); }
            else if (Count > 1 && match(LastNode!.Value))
            {
                SinglyLinkedNode<TValue> lastPrevious = Count == 2 ? FirstNode! : ReachPreviousNode(index: Count - 1);
                RemoveLastNoChecks(lastPrevious);
            }
            else if (Count > 1 && ReachPreviousNode(match, out SinglyLinkedNode<TValue> previous)) { RemoveNextNoChecks(previous); }
        }

        public void RemoveAll(Predicate<TValue> match)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            while (Count > 0 && match(FirstNode!.Value)) { RemoveFirstNoChecks(); }

            if (Count == 0) { return; }

            SinglyLinkedNode<TValue> previous = FirstNode!;
            SinglyLinkedNode<TValue> next = previous.Next!;

            while (Count > 2 && next != LastNode)
            {
                if (match(next.Value)) { RemoveNextNoChecks(previous); next = previous.Next!; }
                else { previous = next; next = next.Next!; }
            }

            if (Count > 1 && match(LastNode!.Value)) { RemoveLastNoChecks(previous); }
        }

        public void RemoveAt(int index)
        {
            Debug.Assert(index >= 0 && index < Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (index == 0) { RemoveFirstNoChecks(); }
            else if (index == Count - 1)
            {
                SinglyLinkedNode<TValue> previous = Count == 2 ? FirstNode! : ReachPreviousNode(index);
                RemoveLastNoChecks(previous);
            }
            else
            {
                SinglyLinkedNode<TValue> previous = ReachPreviousNode(index);
                RemoveNextNoChecks(previous);
            }
        }
    }

    // SEARCH
    public partial class SinglyLinkedList<TValue>
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

                switch (Count)
                {
                    case > 1 when match(LastNode!.Value):
                        value = LastNode.Value!;
                        return true;
                    case > 2 when ReachPreviousNode(match, out SinglyLinkedNode<TValue> previous):
                        value = previous.Next!.Value!;
                        return true;
                }
            }

            value = default;
            return false;
        }

        public void Filter<TCollection>(TCollection collection, Predicate<TValue> match) where TCollection : ICollection<TValue>
        {
            SinglyLinkedNode<TValue> current = FirstNode!;

            for (int i = 0; i < Count; i++)
            {
                if (match(current.Value)) { collection.Insert(current.Value); }
                current = current.Next!;
            }
        }
    }

    // OTHER
    public partial class SinglyLinkedList<TValue>
    {
        public TValue ValueAt(int index)
        {
            Debug.Assert(index >= 0 && index < Count);
            if (index == 0) { return FirstNode!.Value; }
            if (index == Count - 1) { return LastNode!.Value; }

            SinglyLinkedNode<TValue> previous = ReachPreviousNode(index);
            return previous.Next!.Value;
        }

        public void Clear()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        public void Pack() { IsPacked = true; }
    }
}
