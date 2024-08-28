
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class FrequentList<TKey, TValue>() : IList<TKey, TValue>
        where TKey : notnull
        where TValue : IEquatable<TKey>
    {
        private SinglyLinkedNode<TValue>? _first = null;
        private int _count = 0;

        public int Count => _count;

        #region Insert
        public void InsertFirst(TValue value)
        {
            SinglyLinkedNode<TValue> newNode = new(value, next: _first);
            _first = newNode;
            _count++;
        }

        public void InsertLast(TValue value) { throw new NotImplementedException(); }

        public void Insert(TValue value)
        {
            SinglyLinkedNode<TValue> newNode = new(value, next: _first);
            _first = newNode;
            _count++;
        }
        #endregion

        #region Remove
        public void RemoveFirst(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            if (matcher.Match(_first!.Value))
            {
                _first = _first.Next;
                _count--;
                return;
            }

            SinglyLinkedNode<TValue> node = _first;
            while (node.Next is not null)
            {
                if (matcher.Match(node.Next.Value))
                {
                    node.Next = node.Next.Next;
                    _count--;
                    return;
                }

                node = node.Next;
            }
        }

        public void RemoveLast(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }

        public void RemoveAll(IMatcher<TKey, TValue> matcher)
        {
            SinglyLinkedNode<TValue>? node = _first;
            while (node?.Next != null)
            {
                if (matcher.Match(node.Next.Value))
                {
                    node.Next = node.Next.Next;
                    _count--;
                }

                node = node.Next;
            }

            if (_first is null || !matcher.Match(_first!.Value)) { return; }
            _first = _first.Next;
            _count--;
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            if (matcher.Match(_first!.Value))
            {
                _first = _first.Next;
                _count--;
                return;
            }

            SinglyLinkedNode<TValue> node = _first;
            while (node.Next is not null)
            {
                if (matcher.Match(node.Next.Value))
                {
                    node.Next = node.Next.Next;
                    _count--;
                    return;
                }

                node = node.Next;
            }
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

            if (matcher.Match(_first.Value))
            {
                value = _first.Value;
                return true;
            }

            SinglyLinkedNode<TValue> node = _first;
            while (node.Next is not null)
            {
                if (matcher.Match(node.Next.Value))
                {
                    value = node.Next.Value;

                    SinglyLinkedNode<TValue> moveFirst = node.Next;
                    node.Next = moveFirst.Next;
                    moveFirst.Next = _first;
                    _first = moveFirst;

                    return true;
                }
                node = node.Next;
            }

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

            if (matcher.Match(_first.Value))
            {
                value = _first.Value;
                return true;
            }

            SinglyLinkedNode<TValue> node = _first;
            while (node.Next is not null)
            {
                if (matcher.Match(node.Next.Value))
                {
                    value = node.Next.Value;

                    SinglyLinkedNode<TValue> moveFirst = node.Next;
                    node.Next = moveFirst.Next;
                    moveFirst.Next = _first;
                    _first = moveFirst;

                    return true;
                }
                node = node.Next;
            }

            value = default;
            return false;
        }
        #endregion

        public int CountMatches(IMatcher<TKey, TValue> matcher)
        {
            SinglyLinkedNode<TValue>? node = _first;
            int count = 0;

            while (node is not null)
            {
                if (matcher.Match(node.Value)) { count++; }
                node = node.Next;
            }

            return count;
        }

        public IEnumerable<TValue> Traverse()
        {
            SinglyLinkedNode<TValue>? node = _first;

            while (node is not null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }

        public IEnumerable<TValue> TraverseInverse() { throw new NotImplementedException(); }

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher)
        {
            SinglyLinkedNode<TValue>? node = _first;

            while (node is not null)
            {
                if (matcher.Match(node.Value)) { yield return node.Value; }
                node = node.Next;
            }
        }
    }
}
