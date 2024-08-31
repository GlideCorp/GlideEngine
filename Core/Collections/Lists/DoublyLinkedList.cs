
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class DoublyLinkedList<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : ICollection<TKey, TValue>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        protected DoublyLinkedNode<TValue>? First { get; set; } = null;
        protected DoublyLinkedNode<TValue>? Last { get; set; } = null;

        public int Count { get; private set; } = 0;

        public void InsertFirst(TValue value)
        {
            Count++;

            if (First is null) { First = Last = new(value); }
            else
            {
                DoublyLinkedNode<TValue> newNode = new(value, previous: null, next: First);
                First.Previous = newNode;
                First = newNode;
            }
        }

        public void InsertLast(TValue value)
        {
            Count++;

            if (First is null) { First = Last = new(value); }
            else
            {
                DoublyLinkedNode<TValue> newNode = new(value, previous: Last, next: null);
                Last!.Next = newNode;
                Last = newNode;
            }
        }

        public void Insert(TValue value) { InsertFirst(value); }

        private void RemoveFirst()
        {
            Count--;
            if (Count == 0) { First = Last = null; }
            else
            {
                DoublyLinkedNode<TValue> toRemove = First!;
                First = First!.Next;
                First!.Previous = null;
                toRemove.Next = null;
            }
        }

        private void RemoveCurrent(DoublyLinkedNode<TValue> current)
        {
            Count--;
            if (Count == 0) { First = Last = null; }
            else
            {
                current.Previous!.Next = current.Next;
                current.Next!.Previous = current.Previous;

                current.Next = null;
                current.Previous = null;
            }
        }

        private void RemoveLast()
        {
            Count--;
            if (Count == 0) { First = Last = null; }
            else
            {
                DoublyLinkedNode<TValue> toRemove = Last!;
                Last = Last!.Previous;
                Last!.Next = null;
                toRemove.Previous = null;
            }
        }

        public void Remove(TKey key)
        {
            if (First is null) { return; }
            DefaultMatcher.Key = key;

            if (DefaultMatcher.Match(First.Value)) { RemoveFirst(); return; }
            if (First.Next is null) { return; }

            DoublyLinkedNode<TValue> current = First.Next;
            while (current != Last)
            {
                if (DefaultMatcher.Match(current.Value)) { RemoveCurrent(current); return; }
                current = current.Next!;
            }

            if (DefaultMatcher.Match(Last!.Value)) { RemoveLast(); }
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (First is null) { return; }

            if (matcher.Match(First.Value)) { RemoveFirst(); return; }
            if (First.Next is null) { return; }

            DoublyLinkedNode<TValue> current = First.Next;
            while (current != Last)
            {
                if (matcher.Match(current.Value)) { RemoveCurrent(current); return; }
                current = current.Next!;
            }

            if (matcher.Match(Last!.Value)) { RemoveLast(); }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            DoublyLinkedNode<TValue>? current = First;

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
            DoublyLinkedNode<TValue>? current = First;

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