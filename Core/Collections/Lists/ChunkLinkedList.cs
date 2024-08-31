
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class ChunkLinkedList<TKey, TValue>(int size, IMatcher<TKey, TValue> defaultMatcher) : ICollection<TKey, TValue>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        protected ChunkLinkedNode<TValue>? First { get; set; } = null;
        protected ChunkLinkedNode<TValue>? Last { get; set; } = null;
        public int Count { get; private set; } = 0;

        public void Insert(TValue value)
        {
            Count++;

            if (First is null) { First = Last = new(size, value); }
            else if (Last!.NextItemIndex == Last.Values.Length)
            {
                ChunkLinkedNode<TValue> newNode = new(size, value, previous: Last, next: null);
                Last.Next = newNode;
                Last = newNode;
            }
            else { Last.Values[Last.NextItemIndex++] = value; }
        }

        private bool TryRemove(ChunkLinkedNode<TValue> current, IMatcher<TKey, TValue> matcher)
        {
            for (int i = 0; i < current.NextItemIndex; i++)
            {
                if (!matcher.Match(current.Values[i])) { continue; }

                Count--;
                if (Count == 0) { First = Last = null; return true; }

                Last!.NextItemIndex--;
                current.Values[i] = Last.Values[Last.NextItemIndex];
                if (Last.NextItemIndex == 0)
                {
                    ChunkLinkedNode<TValue> toRemove = Last;
                    Last = Last.Previous;
                    Last!.Next = null;
                    toRemove.Previous = null;
                }
                else { Last.Values[Last.NextItemIndex] = default!; }
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
            ChunkLinkedNode<TValue>? current = First;
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
            ChunkLinkedNode<TValue>? current = First;
            while (current != null)
            {
                for (int i = 0; i < current.NextItemIndex; i++)
                {
                    if (matcher.Match(current.Values[i]))
                    {
                        value = current.Values[i]!;
                        return true;
                    }
                }
                current = current.Next;
            }

            value = default;
            return false;
        }
    }
}