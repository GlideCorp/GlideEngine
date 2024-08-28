
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class CachedList<TKey, TValue>(int cacheSize = 8) : IList<TKey, TValue>
        where TKey : notnull
    {
        private TValue[] _cache = new TValue[cacheSize];
        private List<TValue> _list = [];

        private int _cacheIndex = 0;

        public void InsertFirst(TValue value) { throw new NotImplementedException(); }
        public void InsertLast(TValue value) { throw new NotImplementedException(); }

        public void Insert(TValue value)
        {
           // int indefx = _list.BinarySearch(value,);

        }

        public void RemoveFirst(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
        public void RemoveLast(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
        public void RemoveAll(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
        public void Remove(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }

        public bool FindFirst(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }
        public bool FindLast(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }
        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }

        public int CountMatches(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }

        public IEnumerable<TValue> Traverse() { throw new NotImplementedException(); }
        public IEnumerable<TValue> TraverseInverse() { throw new NotImplementedException(); }

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
    }
}
