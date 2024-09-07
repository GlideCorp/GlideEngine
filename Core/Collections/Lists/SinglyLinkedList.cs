
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Core.Collections.Nodes;

namespace Core.Collections.Lists
{
    public class SinglyLinkedList<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : ILinkedList<TKey, TValue, SinglyLinkedNode<TValue>>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        public SinglyLinkedNode<TValue>? FirstNode { get; protected set; } = null;
        public SinglyLinkedNode<TValue>? LastNode { get; protected set; } = null;

        public int Count { get; private set; } = 0;

        public void InsertFirst(TValue value)
        {
            Count++;
            FirstNode = new(value, FirstNode);
            LastNode ??= FirstNode;
        }

        public void InsertLast(TValue value)
        {
            Count++;
            if (FirstNode is null) { FirstNode = LastNode = new(value); }
            else
            {
                LastNode!.Next = new(value);
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
                SinglyLinkedNode<TValue> toRemove = FirstNode!;
                FirstNode = FirstNode!.Next;
                toRemove.Next = null;
            }
        }

        public void RemoveFirst() { if (Count > 0) { RemoveFirstNoChecks(); } }

        private void RemoveNext(SinglyLinkedNode<TValue> previous)
        {
            Count--;
            if (Count == 0) { FirstNode = LastNode = null; }
            else
            {
                SinglyLinkedNode<TValue> toRemove = previous.Next!;
                previous.Next = toRemove.Next;
                toRemove.Next = null;
            }
        }

        private void RemoveLast(SinglyLinkedNode<TValue> previous)
        {
            Count--;
            if (Count == 0) { FirstNode = LastNode = null; }
            else
            {
                LastNode = previous;
                previous.Next = null;
            }
        }

        public void Remove(TKey key)
        {
            DefaultMatcher.Key = key;
            Remove(DefaultMatcher);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (FirstNode is null) { return; }

            if (matcher.Match(FirstNode.Value)) { RemoveFirstNoChecks(); return; }

            SinglyLinkedNode<TValue> previous = FirstNode;
            while (previous.Next != LastNode)
            {
                if (matcher.Match(previous.Next!.Value)) { RemoveNext(previous); return; }
                previous = previous.Next;
            }

            if (matcher.Match(LastNode!.Value)) { RemoveLast(previous); }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            return Find(DefaultMatcher, out value);
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            SinglyLinkedNode<TValue>? current = FirstNode;

            while (current is not null)
            {
                if (matcher.Match(current.Value))
                {
                    value = current.Value!;
                    return true;
                }

                current = current.Next;
            }

            value = default;
            return false;
        }
    }
}
