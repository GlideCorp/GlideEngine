
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections
{
    public interface ICollection<TKey, TValue>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; }

        //public void Insert(TValue value);

        public void Remove(TKey key);
        public void Remove(IMatcher<TKey, TValue> matcher);
        //public void RemoveAll(TKey key);
        //public void RemoveAll(IMatcher<TKey, TValue> matcher);

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value);
        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value);
        //public IEnumerable<TValue> Filter(TKey key);
        //public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher);

        //public int CountMatches();
        //public IEnumerable<TValue> Traverse();
    }
}