
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class CachedList<TKey, TValue>(int size, IMatcher<TKey, TValue> defaultMatcher) : List<TKey, TValue>(defaultMatcher)
    {
        protected TValue[] Cache { get; set; } = new TValue[size];
        protected int NextCacheIndex { get; set; } = 0;
        protected int InverseCacheIndex { get; set; } = 0;
        protected bool Looped { get; set; } = false;

        public new bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            return Find(DefaultMatcher, out value);
        }

        private bool FindInCache(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = Cache.AsSpan(0, !Looped ? NextCacheIndex : Cache.Length);

            for (int i = 0; i < span.Length; i++)
            {
                if (!matcher.Match(span[i])) { continue; }

                value = span[i]!;

                if (InverseCacheIndex == (NextCacheIndex - 1) % Cache.Length ||
                    ((NextCacheIndex > i || i >= InverseCacheIndex) &&
                     (InverseCacheIndex >= NextCacheIndex || NextCacheIndex > i) &&
                     (i >= InverseCacheIndex || i >= NextCacheIndex))) { return true; }

                span[i] = span[InverseCacheIndex];
                span[InverseCacheIndex] = value;
                InverseCacheIndex = (NextCacheIndex + 1) % Cache.Length;

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

                Cache[NextCacheIndex] = val;
                InverseCacheIndex = (NextCacheIndex - 1) % Cache.Length;
                NextCacheIndex = (NextCacheIndex + 1) % Cache.Length;
                Looped |= NextCacheIndex == 0;

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
