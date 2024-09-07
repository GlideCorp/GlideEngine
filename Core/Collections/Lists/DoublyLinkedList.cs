
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class DoublyLinkedList<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : ILinkedList<TKey, TValue, DoublyLinkedNode<TValue>>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        public DoublyLinkedNode<TValue>? FirstNode { get; protected set; } = null;
        public DoublyLinkedNode<TValue>? LastNode { get; protected set; } = null;

        public int Count { get; private set; } = 0;

        public void InsertFirst(TValue value)
        {
            Count++;

            if (FirstNode is null) { FirstNode = LastNode = new(value); }
            else
            {
                FirstNode.Previous = new(value, previous: null, next: FirstNode);
                FirstNode = FirstNode.Previous;
            }
        }

        public void InsertLast(TValue value)
        {
            Count++;

            if (LastNode is null) { FirstNode = LastNode = new(value); }
            else
            {
                LastNode.Next = new(value, previous: LastNode, next: null);
                LastNode = LastNode.Next;
            }
        }

        public void Insert(TValue value) { InsertFirst(value); }

        private void RemoveFirstNoChecks()
        {
            Count--;
            if (Count == 0) { FirstNode = LastNode = null; }
            else
            {
                DoublyLinkedNode<TValue> toRemove = FirstNode!;
                FirstNode = toRemove.Next;
                FirstNode!.Previous = toRemove.Next = null;
            }
        }

        public void RemoveFirst() { if (Count > 0) { RemoveFirstNoChecks(); } }

        private void RemoveCurrentNoChecks(DoublyLinkedNode<TValue> current)
        {
            Count--;
            if (Count == 0) { FirstNode = LastNode = null; }
            else
            {
                current.Previous!.Next = current.Next;
                current.Next!.Previous = current.Previous;
                current.Previous = current.Next = null;
            }
        }

        private void RemoveLastNoChecks()
        {
            Count--;
            if (Count == 0) { FirstNode = LastNode = null; }
            else
            {
                DoublyLinkedNode<TValue> toRemove = LastNode!;
                LastNode = toRemove.Previous;
                toRemove.Previous = LastNode!.Next = null;
            }
        }

        public void RemoveLast() { if (Count > 0) { RemoveLastNoChecks(); } }

        public void Remove(TKey key)
        {
            DefaultMatcher.Key = key;
            Remove(DefaultMatcher);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (FirstNode is null) { return; }

            if (matcher.Match(FirstNode.Value)) { RemoveFirstNoChecks(); return; }
            if (FirstNode.Next is null) { return; }

            DoublyLinkedNode<TValue> current = FirstNode.Next;
            while (current != LastNode)
            {
                if (matcher.Match(current.Value)) { RemoveCurrentNoChecks(current); return; }
                current = current.Next!;
            }

            if (matcher.Match(LastNode!.Value)) { RemoveLastNoChecks(); }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            return Find(DefaultMatcher, out value);
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            DoublyLinkedNode<TValue>? current = FirstNode;

            while (current is not null)
            {
                if (matcher.Match(current.Value))
                {
                    value = current.Value!;
                    return true;
                }

                current = current.Next!;
            }

            value = default;
            return false;
        }
    }
}