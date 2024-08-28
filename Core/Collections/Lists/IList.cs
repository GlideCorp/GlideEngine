
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public interface IList<TKey, TValue> : IBasicCollection<TKey, TValue>
        where TKey : notnull
    {
        public void Insert(TValue value);
        public void InsertFirst(TValue value);
        public void InsertLast(TValue value);

        public void RemoveFirst(IMatcher<TKey, TValue> matcher);
        public void RemoveLast(IMatcher<TKey, TValue> matcher);
        public void RemoveAll(IMatcher<TKey, TValue> matcher);

        public bool FindFirst(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value);
        public bool FindLast(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value);

        public IEnumerable<TValue> TraverseInverse();
    }
}