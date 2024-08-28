
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class SinglyLinkedList<TKey, TValue>() : IList<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
    {
        private SinglyLinkedNode<TValue>? _first = null;
        private SinglyLinkedNode<TValue>? _last = null;

        public int Count { get; private set; }

        #region Insert
        public void InsertFirst(TValue value)
        {
            if (_first is null) { _first = _last = new(value); }
            else
            {
                _first = new(value, next: _first);
                _last!.Next = _first;
            }

            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (_first is null) { _first = _last = new(value); }
            else
            {
                SinglyLinkedNode<TValue> newNode = new(value, next: _first);
                _last!.Next = newNode;
                _last = newNode;
            }

            Count++;
        }

        public void Insert(TValue value)
        {
            if (_first is null) { _first = _last = new(value); }
            else
            {
                _first = new(value, next: _first);
                _last!.Next = _first;
            }

            Count++;
        }
        #endregion

        #region Remove
        public void RemoveFirst(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            SinglyLinkedNode<TValue> previous = _last!;
            do
            {
                if (matcher.Match(previous.Next.Value))
                {
                    Count--;

                    if (Count == 0)
                    {
                        _first = _first.Next = null!;
                        return;
                    }

                    SinglyLinkedNode<TValue> toRemove = previous.Next;
                    previous.Next = toRemove.Next;
                    if (toRemove == _first) { _first = _first.Next; }
                    toRemove.Next = null!;
                    return;
                }

                previous = previous.Next;
            } while (previous != _last);
        }

        public void RemoveLast(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }

        public void RemoveAll(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            SinglyLinkedNode<TValue> previous = _last!;
            do
            {
                if (matcher.Match(previous.Next.Value))
                {
                    Count--;

                    if (Count == 0)
                    {
                        _first = _first.Next = null!;
                        return;
                    }

                    SinglyLinkedNode<TValue> toRemove = previous.Next;
                    previous.Next = toRemove.Next;
                    if (toRemove == _first) { _first = _first.Next; }
                    toRemove.Next = null!;
                }
                else { previous = previous.Next; }

            } while (previous != _last);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            SinglyLinkedNode<TValue> previous = _last!;
            do
            {
                if (matcher.Match(previous.Next.Value))
                {
                    Count--;

                    if (Count == 0)
                    {
                        _first = _first.Next = null!;
                        return;
                    }

                    SinglyLinkedNode<TValue> toRemove = previous.Next;
                    previous.Next = toRemove.Next;
                    if (toRemove == _first) { _first = _first.Next; }
                    toRemove.Next = null!;
                    return;
                }

                previous = previous.Next;
            } while (previous != _last);
        }
        #endregion

        #region Find
        public bool FindFirst(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (_first is null)
            {
                value = default;
                return false;
            }

            SinglyLinkedNode<TValue> previous = _last!;
            do
            {
                if (matcher.Match(previous.Next.Value))
                {
                    value = previous.Next.Value;
                    return true;
                }

                previous = previous.Next;
            } while (previous != _last);

            value = default;
            return false;
        }

        public bool FindLast(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (_first is null)
            {
                value = default;
                return false;
            }

            SinglyLinkedNode<TValue> previous = _last!;
            do
            {
                if (matcher.Match(previous.Next.Value))
                {
                    value = previous.Next.Value;
                    return true;
                }

                previous = previous.Next;
            } while (previous != _last);

            value = default;
            return false;
        }
        #endregion

        public int CountMatches(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return 0; }

            int count = 0;
            SinglyLinkedNode<TValue> previous = _last!;
            do
            {
                if (matcher.Match(previous.Next.Value)) { count++; }

                previous = previous.Next;
            } while (previous != _last);

            return count;
        }

        public IEnumerable<TValue> Traverse()
        {
            if (_first is null) { yield break; }

            SinglyLinkedNode<TValue> current = _first;
            do
            {
                yield return current.Value;
                current = current.Next;
            } while (current != _first);
        }

        public IEnumerable<TValue> TraverseInverse() { throw new NotImplementedException(); }

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { yield break; }

            SinglyLinkedNode<TValue> current = _first;
            do
            {
                if (matcher.Match(current.Value)) { yield return current.Value; }
                current = current.Next;
            } while (current != _first);
        }
    }
}
