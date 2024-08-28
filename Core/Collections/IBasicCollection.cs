
using System.Diagnostics.CodeAnalysis;
using Core.Collections.Nodes;

namespace Core.Collections
{
    public interface IBasicCollection<TKey, TValue>
        where TKey : notnull
    {
        public void Remove(IMatcher<TKey, TValue> matcher);
        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value);

        public int CountMatches(IMatcher<TKey, TValue> matcher);

        public IEnumerable<TValue> Traverse();

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher);
    }
}