
using Core.Collections.Nodes;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class LinkedChunkList<TKey, TValue>(int chunkSize, IMatcher<TKey, TValue> defaultMatcher) : ILinkedList<TKey, TValue, LinkedChunkNode<TValue>>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        public LinkedChunkNode<TValue>? FirstNode { get; protected set; } = null;
        public LinkedChunkNode<TValue>? LastNode { get; protected set; } = null;
        public int Count { get; private set; } = 0;

        public void Insert(TValue value)
        {
            Count++;

            if (FirstNode is null) { FirstNode = LastNode = new(chunkSize, value); }
            else if (LastNode!.NextItemIndex == LastNode.Values.Length)
            {
                LastNode.Next = new(chunkSize, value, previous: LastNode, next: null);
                LastNode = LastNode.Next;
            }
            else { LastNode.Values[LastNode.NextItemIndex++] = value; }
        }

        private bool TryRemove(LinkedChunkNode<TValue> current, IMatcher<TKey, TValue> matcher)
        {
            Span<TValue> span = current.Values.AsSpan(0, current.NextItemIndex);
            for (int i = 0; i < span.Length; i++)
            {
                if (!matcher.Match(span[i])) { continue; }

                Count--;
                if (Count == 0) { FirstNode = LastNode = null; return true; }

                LastNode!.NextItemIndex--;
                span[i] = LastNode.Values[LastNode.NextItemIndex];
                if (LastNode.NextItemIndex == 0)
                {
                    LinkedChunkNode<TValue> toRemove = LastNode;
                    LastNode = LastNode.Previous;
                    LastNode!.Next = null;
                    toRemove.Previous = null;
                }
                else { LastNode.Values[LastNode.NextItemIndex] = default!; }
                return true;
            }

            return false;
        }

        public void Remove(TKey key)
        {
            DefaultMatcher.Key = key;
            Remove(DefaultMatcher);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            LinkedChunkNode<TValue>? current = FirstNode;
            while (current != null)
            {
                if (TryRemove(current, matcher)) { return; }
                current = current.Next;
            }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            return Find(DefaultMatcher, out value);
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            LinkedChunkNode<TValue>? current = FirstNode;
            while (current != null)
            {
                Span<TValue> span = current.Values.AsSpan(0, current.NextItemIndex);
                for (int i = 0; i < span.Length; i++)
                {
                    if (!matcher.Match(span[i])) { continue; }
                    value = span[i]!;
                    return true;
                }
                current = current.Next;
            }

            value = default;
            return false;
        }
    }
}