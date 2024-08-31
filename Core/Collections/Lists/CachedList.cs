/*
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class CachedList<TKey, TValue>(int cacheSize = 8) : IList<TKey, TValue>
        where TKey : notnull
        where TValue : notnull
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

        public void RemoveFirst(Match<TValue> match) { throw new NotImplementedException(); }
        public void RemoveLast(Match<TValue> match) { throw new NotImplementedException(); }
        public void RemoveAll(Match<TValue> match) { throw new NotImplementedException(); }
        public void Remove(Match<TValue> match) { throw new NotImplementedException(); }

        public bool FindFirst(Match<TValue> match, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }
        public bool FindLast(Match<TValue> match, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }
        public bool Find(Match<TValue> match, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }

        public int CountMatches(Match<TValue> match) { throw new NotImplementedException(); }

        public IEnumerable<TValue> Traverse() { throw new NotImplementedException(); }
        public IEnumerable<TValue> TraverseInverse() { throw new NotImplementedException(); }

        public IEnumerable<TValue> Filter(Match<TValue> match) { throw new NotImplementedException(); }
    }
}
*/