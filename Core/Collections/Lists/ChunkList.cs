
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class ChunkList<TKey, TValue>(int size = 8) : IList<TKey, TValue>
        where TKey : notnull
    {
        private ChunkLinkedNode<TKey, TValue>? _first = null;
        private ChunkLinkedNode<TKey, TValue>? _last = null;
        private int _cacheIndex = 0;

        // TODO:
        // -remove null checks with circular nodes
        // +move last searched (also with Filter method) item into buffer node (first node) & remove InsertFirst implementation
        // +buffer circular index
        // -buffer size x2 | x4

        public int Count { get; private set; }

        #region Insert
        public void InsertFirst(TValue value) { throw new NotImplementedException(); }

        public void InsertLast(TValue value)
        {
            if (_last is null) { _first = _last = new(size, previous: null, next: null); }
            else if (_last.Cursor == _last.Values.Length)
            {
                ChunkLinkedNode<TKey, TValue> newNode = new(size, previous: _last, next: null);
                _last.Next = newNode;
                _last = newNode;
            }

            _last.Values[_last.Cursor++] = value;
            Count++;
        }

        public void Insert(TValue value)
        {
            if (_last is null) { _first = _last = new(size, previous: null, next: null); }
            else if (_last.Cursor == size)
            {
                ChunkLinkedNode<TKey, TValue> newNode = new(size, previous: _last, next: null); ;
                _last.Next = newNode;
                _last = newNode;
            }

            _last.Values[_last.Cursor++] = value;
            Count++;
        }
        #endregion

        #region Remove
        public void RemoveFirst(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            ChunkLinkedNode<TKey, TValue>? node = _first;
            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                for (int i = 0; i < valueSpan.Length; i++)
                {
                    if (!matcher.Match(valueSpan[i]!)) { continue; }

                    valueSpan[i] = _last!.Values[_last.Cursor - 1];

                    Count--;
                    if (_last.Cursor == 0)
                    {
                        if (_last == _first) { _last = _first = null; }
                        else
                        {
                            _last = _last.Previous!;
                            _last.Next = null;
                        }
                    }
                    else
                    {
                        _last.Cursor--;
                        _last.Values[_last.Cursor] = default;
                    }

                    return;
                }

                node = node.Next;
            }
        }

        public void RemoveLast(IMatcher<TKey, TValue> matcher)
        {
            if (_last is null) { return; }

            ChunkLinkedNode<TKey, TValue>? node = _last;
            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                for (int i = valueSpan.Length - 1; i >= 0; i--)
                {
                    if (!matcher.Match(valueSpan[i]!)) { continue; }

                    valueSpan[i] = _last.Values[_last.Cursor - 1];

                    Count--;
                    if (_last.Cursor == 0)
                    {
                        if (_last == _first) { _last = _first = null; }
                        else
                        {
                            _last = _last.Previous!;
                            _last.Next = null;
                        }
                    }
                    else
                    {
                        _last.Cursor--;
                        _last.Values[_last.Cursor] = default;
                    }

                    return;
                }

                node = node.Previous;
            }
        }

        public void RemoveAll(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            ChunkLinkedNode<TKey, TValue>? node = _first;
            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                for (int i = 0; i < valueSpan.Length; i++)
                {
                    if (!matcher.Match(valueSpan[i]!)) { continue; }

                    valueSpan[i] = _last!.Values[_last.Cursor - 1];

                    Count--;
                    if (_last.Cursor == 0)
                    {
                        if (_last == _first) { _last = _first = null; }
                        else
                        {
                            _last = _last.Previous!;
                            _last.Next = null;
                        }
                    }
                    else
                    {
                        _last.Cursor--;
                        _last.Values[_last.Cursor] = default;
                    }
                }

                node = node.Next;
            }
        }


        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (_first is null) { return; }

            ChunkLinkedNode<TKey, TValue>? node = _first;
            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                for (int i = 0; i < valueSpan.Length; i++)
                {
                    if (!matcher.Match(valueSpan[i]!)) { continue; }

                    valueSpan[i] = _last!.Values[_last.Cursor - 1];

                    if (_last.Cursor == 1)
                    {
                        if (_last == _first) { _last = _first = null; }
                        else
                        {
                            _last = _last.Previous!;
                            _last.Next = null;
                        }
                    }
                    else
                    {
                        _last.Cursor--;
                        _last.Values[_last.Cursor] = default;
                    }

                    Count--;
                    return;
                }

                node = node.Next;
            }
        }
        #endregion

        #region Find
        public bool FindFirst(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            ChunkLinkedNode<TKey, TValue>? node = _first;

            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                for (int i = 0; i < valueSpan.Length; i++)
                {
                    if (!matcher.Match(valueSpan[i]!)) { continue; }

                    if (node != _first)
                    {
                        TValue? temp = _first!.Values[_cacheIndex];
                        _first!.Values[_cacheIndex] = valueSpan[i];
                        valueSpan[i] = temp;

                        _cacheIndex = (_cacheIndex + 1) % _first.Values.Length;
                    }

                    value = valueSpan[i]!;
                    return true;
                }

                node = node.Next;
            }

            value = default;
            return false;
        }

        public bool FindLast(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            ChunkLinkedNode<TKey, TValue>? node = _last;

            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                for (int i = valueSpan.Length - 1; i >= 0; i--)
                {
                    if (!matcher.Match(valueSpan[i]!)) { continue; }

                    if (node != _first)
                    {
                        TValue? temp = _first!.Values[_cacheIndex];
                        _first!.Values[_cacheIndex] = valueSpan[i];
                        valueSpan[i] = temp;

                        _cacheIndex = (_cacheIndex + 1) % _first.Values.Length;
                    }

                    value = valueSpan[i]!;
                    return true;
                }

                node = node.Previous;
            }

            value = default;
            return false;
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            ChunkLinkedNode<TKey, TValue>? node = _first;

            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                for (int i = 0; i < valueSpan.Length; i++)
                {
                    if (!matcher.Match(valueSpan[i]!)) { continue; }

                    if (node != _first)
                    {
                        TValue? temp = _first!.Values[_cacheIndex];
                        _first!.Values[_cacheIndex] = valueSpan[i];
                        valueSpan[i] = temp;

                        _cacheIndex = (_cacheIndex + 1) % _first.Values.Length;
                    }

                    value = valueSpan[i]!;
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
            ChunkLinkedNode<TKey, TValue>? node = _first;
            int count = 0;

            while (node is not null)
            {
                Span<TValue?> valueSpan = node.Values.AsSpan(0, node.Cursor);
                foreach (TValue? value in valueSpan)
                {
                    if (matcher.Match(value!)) { count++; }
                }
                node = node.Next;
            }

            return count;
        }

        public IEnumerable<TValue> Traverse()
        {
            ChunkLinkedNode<TKey, TValue>? node = _first;

            while (node is not null)
            {
                for (int i = 0; i < node.Cursor; i++) { yield return node.Values[i]!; }
                node = node.Next;
            }
        }

        public IEnumerable<TValue> TraverseInverse()
        {
            ChunkLinkedNode<TKey, TValue>? node = _last;

            while (node is not null)
            {
                for (int i = node.Cursor - 1; i >= 0; i--) { yield return node.Values[i]!; }
                node = node.Previous;
            }
        }

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher)
        {
            ChunkLinkedNode<TKey, TValue>? node = _first;

            while (node is not null)
            {
                for (int i = 0; i < node.Cursor; i++)
                {
                    if (!matcher.Match(node.Values[i]!)) { continue; }

                    if (node != _first)
                    {
                        TValue? temp = _first!.Values[_cacheIndex];
                        _first!.Values[_cacheIndex] = node.Values[i];
                        node.Values[i] = temp;

                        _cacheIndex = (_cacheIndex + 1) % _first.Values.Length;
                    }

                    yield return node.Values[i]!;
                }
                node = node.Next;
            }
        }
    }

    /*
     public class ChunkList<TKey, TValue>(int size = 8) : IList<TKey, TValue>
           where TKey : notnull
           where TValue : notnull
       {
           private ChunkLinkedNode<TKey, TValue>? _first = null;
       
           // TODO:
           // remove null checks with circular nodes
           // move last searched (also with Filter method) item into buffer node (first node) & remove InsertFirst implementation
           // buffer circular index
           // buffer size x2 | x4
       
           public int Count { get; private set; }
       
           #region Insert
           public void InsertFirst(TValue value)
           {
               if (_first is null) { _first = new(size, value); }
               else
               {
                   ChunkLinkedNode<TKey, TValue> last = _first.Previous;
                   if (last.Cursor == last.Values.Length)
                   {
                       ChunkLinkedNode<TKey, TValue> newNode = new(size, _first.Values[0]!, previous: last, next: _first);
                       last.Next = newNode;
                       _first.Previous = newNode;
                   }
                   else { last.Values[last.Cursor++] = _first.Values[0]; }
               }
       
               Count++;
           }
       
           public void InsertLast(TValue value)
           {
               if (_first is null) { _first = new(size, value); }
               else
               {
                   ChunkLinkedNode<TKey, TValue> last = _first.Previous;
                   if (last.Cursor == last.Values.Length)
                   {
                       ChunkLinkedNode<TKey, TValue> newNode = new(size, _first.Values[0]!, previous: last, next: _first);
                       last.Next = newNode;
                       _first.Previous = newNode;
                   }
                   else { last.Values[last.Cursor++] = value; }
               }
       
               Count++;
           }
       
           public void Insert(TValue value)
           {
               if (_first is null) { _first = new(size, value); }
               else
               {
                   ChunkLinkedNode<TKey, TValue> last = _first.Previous;
                   if (last.Cursor == last.Values.Length)
                   {
                       ChunkLinkedNode<TKey, TValue> newNode = new(size, _first.Values[0]!, previous: last, next: _first);
                       last.Next = newNode;
                       _first.Previous = newNode;
                   }
                   else { last.Values[last.Cursor++] = value; }
               }
       
               Count++;
           }
           #endregion
       
           #region Remove
           public void RemoveFirst(IMatcher<TKey, TValue> matcher)
           {
               if (_first is null) { return; }
       
               ChunkLinkedNode<TKey, TValue> current = _first;
               do
               {
                   for (int i = 0; i < current.Cursor; i++)
                   {
                       if (!matcher.Match(current.Values[i])) { continue; }
       
                       Count--;
                       if (Count == 0)
                       {
                           _first = _first.Previous = _first.Next = null!;
                           return;
                       }
       
                       ChunkLinkedNode<TKey, TValue> last = _first.Previous;
                       current.Values[i] = last.Values[last.Cursor - 1];
                       last.Values[last.Cursor - 1] = default!;
       
                       last.Cursor--;
                       if (last.Cursor != 0) { return; }
                       last.Previous.Next = _first;
                       _first.Previous = last.Previous;
                       last.Previous = last.Next = null!;
                       return;
                   }
       
                   current = current.Next;
               } while (current != _first);
           }
       
           public void RemoveLast(IMatcher<TKey, TValue> matcher)
           {
               if (_first is null) { return; }
       
               ChunkLinkedNode<TKey, TValue> current = _first.Previous;
               do
               {
                   for (int i = current.Cursor - 1; i >= 0; i--)
                   {
                       if (!matcher.Match(current.Values[i])) { continue; }
       
                       Count--;
                       if (Count == 0)
                       {
                           _first = _first.Previous = _first.Next = null!;
                           return;
                       }
       
                       ChunkLinkedNode<TKey, TValue> last = _first.Previous;
                       current.Values[i] = last.Values[last.Cursor - 1];
                       last.Values[last.Cursor - 1] = default!;
       
                       last.Cursor--;
                       if (last.Cursor != 0) { return; }
                       last.Previous.Next = _first;
                       _first.Previous = last.Previous;
                       last.Previous = last.Next = null!;
                       return;
                   }
       
                   current = current.Previous;
               } while (current != _first.Previous);
           }
       
           public void RemoveAll(IMatcher<TKey, TValue> matcher)
           {
               if (_first is null) { return; }
       
               ChunkLinkedNode<TKey, TValue> current = _first;
               do
               {
                   for (int i = 0; i < current.Cursor; i++)
                   {
                       if (!matcher.Match(current.Values[i])) { continue; }
       
                       Count--;
                       if (Count == 0)
                       {
                           _first = _first.Previous = _first.Next = null!;
                           return;
                       }
       
                       ChunkLinkedNode<TKey, TValue> last = _first.Previous;
                       current.Values[i] = last.Values[last.Cursor - 1];
                       last.Values[last.Cursor - 1] = default!;
       
                       last.Cursor--;
                       if (last.Cursor != 0) { continue; }
                       last.Previous.Next = _first;
                       _first.Previous = last.Previous;
                       last.Previous = last.Next = null!;
                   }
       
                   current = current.Next;
               } while (current != _first);
           }
       
       
           public void Remove(IMatcher<TKey, TValue> matcher)
           {
               if (_first is null) { return; }
       
               ChunkLinkedNode<TKey, TValue> current = _first;
               do
               {
                   for (int i = 0; i < current.Cursor; i++)
                   {
                       if (!matcher.Match(current.Values[i])) { continue; }
       
                       Count--;
                       if (Count == 0) { _first = _first.Previous = _first.Next = null!; }
                       else
                       {
                           ChunkLinkedNode<TKey, TValue> last = _first.Previous;
                           current.Values[i] = last.Values[last.Cursor - 1];
                           last.Values[last.Cursor - 1] = default!;
                           last.Cursor--;
       
                           if (last.Cursor != 0) { return; }
                           last.Previous.Next = _first;
                           _first.Previous = last.Previous;
                           last.Previous = last.Next = null!;
                       }
       
                       return;
                   }
       
                   current = current.Next;
               } while (current != _first);
           }
           #endregion
       
           #region Find
           public bool FindFirst(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
           {
               if (_first is not null)
               {
                   ChunkLinkedNode<TKey, TValue> current = _first;
                   do
                   {
                       for (int i = 0; i < current.Cursor; i++)
                       {
                           if (!matcher.Match(current.Values[i])) { continue; }
                           value = current.Values[i];
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
                   ChunkLinkedNode<TKey, TValue> current = _first.Previous;
                   do
                   {
                       for (int i = current.Cursor - 1; i >= 0; i--)
                       {
                           if (!matcher.Match(current.Values[i])) { continue; }
                           value = current.Values[i];
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
                   ChunkLinkedNode<TKey, TValue> current = _first;
                   do
                   {
                       for (int i = 0; i < current.Cursor; i++)
                       {
                           if (!matcher.Match(current.Values[i])) { continue; }
                           value = current.Values[i];
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
               ChunkLinkedNode<TKey, TValue> current = _first;
               do
               {
                   for (int i = 0; i < current.Cursor; i++)
                   {
                       if (matcher.Match(current.Values[i])) { count++; }
                   }
       
                   current = current.Next;
               } while (current != _first);
       
               return count;
           }
       
           public IEnumerable<TValue> Traverse()
           {
               if (_first is null) { yield break; }
       
               ChunkLinkedNode<TKey, TValue> current = _first;
               do
               {
                   for (int i = 0; i < current.Cursor; i++) { yield return current.Values[i]; }
                   current = current.Next;
               } while (current != _first);
           }
       
           public IEnumerable<TValue> TraverseInverse()
           {
               if (_first is null) { yield break; }
       
               ChunkLinkedNode<TKey, TValue> current = _first.Previous;
               do
               {
                   for (int i = current.Cursor - 1; i >= 0; i--) { yield return current.Values[i]!; }
                   current = current.Next;
               } while (current != _first.Previous);
           }
       
           public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher)
           {
               if (_first is null) { yield break; }
       
               ChunkLinkedNode<TKey, TValue> current = _first;
               do
               {
                   for (int i = 0; i < current.Cursor; i++)
                   {
                       if (matcher.Match(current.Values[i])) { yield return current.Values[i]; }
                   }
       
                   current = current.Next;
               } while (current != _first);
           }
       }
     */
}
