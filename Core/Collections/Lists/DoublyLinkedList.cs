
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class DoublyLinkedList<TKey, TValue>() : IList<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private DoublyLinkedNode<TValue>? _first = null;

        public int Count { get; private set; }

        #region Insert
        public void InsertFirst(TValue value)
        {
            if (_first is null) { _first = new(value); }
            else
            {
                DoublyLinkedNode<TValue> newNode = new(value, previous: _first.Previous, next: _first);
                _first.Previous.Next = newNode;
                _first.Previous = newNode;
                _first = newNode;
            }

            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (_first is null) { _first = new(value); }
            else
            {
                DoublyLinkedNode<TValue> newNode = new(value, previous: _first.Previous, next: _first);
                _first.Previous.Next = newNode;
                _first.Previous = newNode;
            }

            Count++;
        }

        public void Insert(TValue value)
        {
            if (_first is null) { _first = new(value); }
            else
            {
                DoublyLinkedNode<TValue> newNode = new(value, previous: _first.Previous, next: _first);
                _first.Previous.Next = newNode;
                _first.Previous = newNode;
            }

            Count++;
        }
        #endregion

        #region Remove
        public void RemoveFirst(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            DoublyLinkedNode<TValue> current = _first;
            do
            {
                if (!matcher.Match(current.Value))
                {
                    current = current.Next;
                    continue;
                }

                Count--;
                if (Count == 0) { _first = _first.Previous = _first.Next = null!; }
                else
                {
                    current.Previous.Next = current.Next;
                    current.Next.Previous = current.Previous;
                    if (current == _first) { _first = _first.Next; }
                    current.Previous = current.Next = null!;
                }
                return;
            } while (current != _first);
        }

        public void RemoveLast(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            DoublyLinkedNode<TValue> current = _first.Previous;
            do
            {
                if (!matcher.Match(current.Value))
                {
                    current = current.Previous;
                    continue;
                }

                Count--;
                if (Count == 0) { _first = _first.Previous = _first.Next = null!; }
                else
                {
                    current.Previous.Next = current.Next;
                    current.Next.Previous = current.Previous;
                    if (current == _first) { _first = _first.Next; }
                    current.Previous = current.Next = null!;
                }

                return;
            } while (current != _first.Previous);
        }

        public void RemoveAll(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            DoublyLinkedNode<TValue> current = _first;
            do
            {
                if (matcher.Match(current.Value))
                {
                    Count--;
                    if (Count == 0)
                    {
                        _first = _first.Previous = _first.Next = null!;
                        return;
                    }

                    DoublyLinkedNode<TValue> toRemove = current;
                    current = current.Next;

                    toRemove.Previous.Next = toRemove.Next;
                    toRemove.Next.Previous = toRemove.Previous;
                    if (toRemove == _first) { _first = _first.Next; }
                    toRemove.Previous = toRemove.Next = null!;
                }
                else { current = current.Next; }

            } while (current != _first);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            DoublyLinkedNode<TValue> current = _first;
            do
            {
                if (!matcher.Match(current.Value))
                {
                    current = current.Next;
                    continue;
                }

                Count--;
                if (Count == 0) { _first = _first.Previous = _first.Next = null!; }
                else
                {
                    current.Previous.Next = current.Next;
                    current.Next.Previous = current.Previous;
                    if (current == _first) { _first = _first.Next; }
                    current.Previous = current.Next = null!;
                }
                return;
            } while (current != _first);
        }
        #endregion

        #region Find
        public bool FindFirst(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (_first is not null)
            {
                DoublyLinkedNode<TValue> current = _first;
                do
                {
                    if (matcher.Match(current.Value))
                    {
                        value = current.Value;
                        return true;
                    }

                    current = current.Next;
                } while (current != _first);
            }

            value = default;
            return false;
        }

        public bool FindLast(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (_first is not null)
            {
                DoublyLinkedNode<TValue> current = _first.Previous;
                do
                {
                    if (matcher.Match(current.Value))
                    {
                        value = current.Value;
                        return true;
                    }

                    current = current.Previous;
                } while (current != _first.Previous);
            }

            value = default;
            return false;
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (_first is not null)
            {
                DoublyLinkedNode<TValue> current = _first;
                do
                {
                    if (matcher.Match(current.Value))
                    {
                        value = current.Value;
                        return true;
                    }

                    current = current.Next;
                } while (current != _first);
            }

            value = default;
            return false;
        }
        #endregion

        public int CountMatches(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return 0; }

            int count = 0;
            DoublyLinkedNode<TValue> current = _first;
            do
            {
                if (matcher.Match(current.Value)) { count++; }
                current = current.Next;

            } while (current != _first);

            return count;
        }

        public IEnumerable<TValue> Traverse()
        {
            if (_first is null) { yield break; }

            DoublyLinkedNode<TValue> current = _first;
            do
            {
                yield return current.Value;
                current = current.Next;

            } while (current != _first);
        }

        public IEnumerable<TValue> TraverseInverse()
        {
            if (_first is null) { yield break; }

            DoublyLinkedNode<TValue> current = _first.Previous;
            do
            {
                yield return current.Value;
                current = current.Previous;

            } while (current != _first.Previous);
        }

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { yield break; }

            DoublyLinkedNode<TValue> current = _first;
            do
            {
                if (matcher.Match(current.Value)) { yield return current.Value; }
                current = current.Next;
            } while (current != _first);
        }
    }
}

