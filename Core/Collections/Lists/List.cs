
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class List<TKey, TValue>(int initialSize = 2) : IList<TKey, TValue>
        where TKey : notnull
    {
        private TValue[] _array = new TValue[initialSize];
        private int _count = 0;

        #region Insert
        private void DoubleSize()
        {
            int size = _array.Length;
            TValue[] newArray = new TValue[size * 2];
            Array.Copy(_array, newArray, size);
            _array = newArray;
        }

        public void InsertFirst(TValue value) { throw new NotImplementedException(); }
        public void InsertLast(TValue value) { throw new NotImplementedException(); }

        public void Insert(TValue value)
        {
            if (_count == _array.Length) { DoubleSize(); }
        }
        #endregion

        #region Remove
        private void HalveSize()
        {
            int size = _array.Length / 2;
            TValue[] newArray = new TValue[size];
            Array.Copy(_array, newArray, size);
            _array = newArray;
        }
        public void RemoveFirst(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
        public void RemoveLast(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
        public void RemoveAll(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
        public void Remove(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
        #endregion

        #region Find
        public bool FindFirst(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }
        public bool FindLast(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }
        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value) { throw new NotImplementedException(); }
        #endregion

        public int CountMatches(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }

        public IEnumerable<TValue> Traverse() { throw new NotImplementedException(); }
        public IEnumerable<TValue> TraverseInverse() { throw new NotImplementedException(); }

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher) { throw new NotImplementedException(); }
    }
}
