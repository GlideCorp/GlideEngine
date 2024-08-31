
using System.Diagnostics.CodeAnalysis;
using Core.Collections.Nodes;

namespace Core.Collections.Lists
{
    public class SinglyLinkedList<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : ICollection<TKey, TValue>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        protected SinglyLinkedNode<TValue>? First { get; set; } = null;
        protected SinglyLinkedNode<TValue>? Last { get; set; } = null;

        public int Count { get; private set; } = 0;

        public void InsertFirst(TValue value)
        {
            Count++;

            if (First is null) { First = Last = new(value); }
            else { First = new(value, First); }
        }

        public void InsertLast(TValue value)
        {
            Count++;

            if (First is null) { First = Last = new(value); }
            else
            {
                Last!.Next = new(value);
                Last = Last.Next;
            }
        }

        public void Insert(TValue value) { InsertFirst(value); }

        private void RemoveFirst()
        {
            Count--;
            if (Count == 0) { First = Last = null; }
            else
            {
                SinglyLinkedNode<TValue> toRemove = First!;
                First = First!.Next;
                toRemove.Next = null;
            }
        }

        private void RemoveNext(SinglyLinkedNode<TValue> previous)
        {
            Count--;
            if (Count == 0) { First = Last = null; }
            else
            {
                SinglyLinkedNode<TValue> toRemove = previous.Next!;
                previous.Next = previous.Next!.Next;
                toRemove.Next = null;
            }
        }

        private void RemoveLast(SinglyLinkedNode<TValue> previous)
        {
            Count--;
            if (Count == 0) { First = Last = null; }
            else
            {
                Last = previous;
                previous.Next = null;
            }
        }

        public void Remove(TKey key)
        {
            if (First is null) { return; }
            DefaultMatcher.Key = key;

            if (DefaultMatcher.Match(First.Value)) { RemoveFirst(); return; }

            SinglyLinkedNode<TValue> previous = First;
            while (previous.Next != Last)
            {
                if (DefaultMatcher.Match(previous.Next!.Value)) { RemoveNext(previous); return; }
                previous = previous.Next;
            }

            if (DefaultMatcher.Match(Last!.Value)) { RemoveLast(previous); }
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (First is null) { return; }

            if (matcher.Match(First.Value)) { RemoveFirst(); return; }

            SinglyLinkedNode<TValue> previous = First;
            while (previous.Next != Last)
            {
                if (matcher.Match(previous.Next!.Value)) { RemoveNext(previous); return; }
                previous = previous.Next;
            }

            if (matcher.Match(Last!.Value)) { RemoveLast(previous); }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            SinglyLinkedNode<TValue>? current = First;

            while (current is not null)
            {
                if (DefaultMatcher.Match(current.Value))
                {
                    value = current.Value!;
                    return true;
                }

                current = current.Next;
            }

            value = default;
            return false;
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            SinglyLinkedNode<TValue>? current = First;

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
