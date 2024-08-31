
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class CachedList<TKey, TValue>(int size, IMatcher<TKey, TValue> defaultMatcher) : List<TKey, TValue>(defaultMatcher)
    {
        protected TValue[] Cache { get; set; } = new TValue[size];
        protected int NextCachedIndex { get; set; } = 0;
        //protected int InverseCachedIndex { get; set; } = 0;
        protected bool Looped { get; set; } = false;

        public new bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            return Find(DefaultMatcher, out value);
        }

        private bool FindInCache(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = Cache.AsSpan(0, !Looped ? NextCachedIndex : Cache.Length);

            foreach (TValue val in span)
            {
                if (!matcher.Match(val)) { continue; }

                //Cache[NextCachedIndex] = val;
                //InverseCachedIndex = NextCachedIndex;
                //NextCachedIndex = (NextCachedIndex + 1) % Cache.Length;
                //Looped |= InverseCachedIndex > NextCachedIndex;

                value = val!;
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }

        private bool FindInList(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = Array.AsSpan(0, Count);
            foreach (TValue val in span)
            {
                if (!matcher.Match(val)) { continue; }

                Cache[NextCachedIndex] = val;
                //InverseCachedIndex = NextCachedIndex;
                NextCachedIndex = (NextCachedIndex + 1) % Cache.Length;
                Looped |= NextCachedIndex == 0;

                value = val!;
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }

        public new bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            return FindInCache(matcher, out value) || FindInList(matcher, out value);
        }

    }
}
