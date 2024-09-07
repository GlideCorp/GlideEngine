
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class CachedList2<TKey, TValue>(int cacheSize, IMatcher<TKey, TValue> defaultMatcher)
        : ICollection<TKey, TValue>
    {
        /*
        protected class CacheValue()
        {
            public DateTime LastHit { get; set; } = DateTime.MinValue;
            public TValue? Value { get; set; } = default;
        }
        */
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        protected TValue[] Array { get; set; } = [];
        protected TValue[] Cache { get; set; } = new TValue[cacheSize];

        public int Count => ArrayCount + CacheCount;
        protected int ArrayCount { get; set; } = 0;
        protected int CacheCount { get; set; } = 0;

        protected int NextSize() { return Math.Max(Array.Length * 2, 2); }
        protected int PreviousSize() { return Array.Length / 2; }

        private void ResizeArray(int newSize)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> span1 = Array.AsSpan(0, ArrayCount);
            Span<TValue> span2 = newArray.AsSpan(0, ArrayCount);
            span1.CopyTo(span2);

            Array = newArray;
        }

        private void ResizeArrayWithoutElementAt(int newSize, int skipIndex)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> span1 = Array.AsSpan(0, skipIndex);
            Span<TValue> span2 = newArray.AsSpan(0, skipIndex);
            span1.CopyTo(span2);

            span1 = Array.AsSpan(skipIndex + 1, ArrayCount);
            span2 = newArray.AsSpan(skipIndex + 1, ArrayCount);
            span1.CopyTo(span2);

            Array = newArray;
        }


        private static void PushBackAndAddLast(Span<TValue> source, Span<TValue> destination,
            int from, int to, TValue value)
        {
            Span<TValue> moveSource = source[from..to];
            Span<TValue> moveDestination = destination[(from - 1)..(to - 1)];
            moveSource.CopyTo(moveDestination);

            destination[^1] = value;
        }

        private static void PushForwardAndAddFirst(Span<TValue> source, Span<TValue> destination,
            int from, int to, TValue value)
        {
            Span<TValue> moveSource = source[from..to];
            Span<TValue> moveDestination = destination[(from + 1)..(to + 1)];
            moveSource.CopyTo(moveDestination);

            destination[0] = value;
        }


        public void Insert(TValue value)
        {
            if (ArrayCount == Array.Length) { ResizeArray(newSize: NextSize()); }
            Array[ArrayCount++] = value;
        }

        private bool RemoveFromCache(IMatcher<TKey, TValue> matcher)
        {
            Span<TValue> span = Cache.AsSpan(0, CacheCount);
            for (int i = 0; i < span.Length; i++)
            {
                if (matcher.Match(span[i])) { continue; }

                PushBackAndAddLast(source: span, destination: span, from: i + 1, to: CacheCount, value: default!);
                CacheCount--;
                return true;
            }

            return false;
        }

        private void RemoveFromArray(IMatcher<TKey, TValue> matcher)
        {
            Span<TValue> span = Array.AsSpan(0, ArrayCount);
            for (int i = 0; i < span.Length; i++)
            {
                if (matcher.Match(span[i])) { continue; }

                if (ArrayCount == 1) { Array = []; }
                else if (ArrayCount - 1 <= Array.Length / 4)
                    ResizeArrayWithoutElementAt(newSize: PreviousSize(), skipIndex: i);
                else
                    PushBackAndAddLast(source: span, destination: span, from: i + 1, to: ArrayCount, value: default!);

                ArrayCount--;
                return;
            }
        }

        public void Remove(TKey key)
        {
            DefaultMatcher.Key = key;
            Remove(DefaultMatcher);
        }

        public void Remove(IMatcher<TKey, TValue> matcher) { if (!RemoveFromCache(matcher)) { RemoveFromArray(matcher); } }

        private bool FindInCache(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = Cache.AsSpan(0, CacheCount);
            for (int i = 0; i < span.Length; i++)
            {
                if (!matcher.Match(span[i])) { continue; }

                value = span[i]!;
                PushForwardAndAddFirst(source: span, destination: span, from: 0, to: i - 1, value: value);
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }

        private void RemoveLastFromArray()
        {
            if (ArrayCount - 1 <= Array.Length / 4)
                ResizeArrayWithoutElementAt(newSize: PreviousSize(), skipIndex: ArrayCount - 1);
            else { Array[ArrayCount - 1] = default!; }
        }

        private bool FindInList(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> arraySpan = Array.AsSpan(0, ArrayCount);
            for (int i = 0; i < arraySpan.Length; i++)
            {
                if (!matcher.Match(arraySpan[i])) { continue; }

                value = arraySpan[i]!;
                Span<TValue> cacheSpan = Cache.AsSpan(0, CacheCount);

                if (ArrayCount == 1) { Array = []; }
                else if (CacheCount < Cache.Length)
                {
                    arraySpan[i] = arraySpan[ArrayCount - 1];
                    RemoveLastFromArray();
                    cacheSpan[CacheCount++] = value;
                }
                else if (CacheCount == Cache.Length)
                {
                    arraySpan[i] = cacheSpan[CacheCount - 1];
                    PushForwardAndAddFirst(source: cacheSpan, destination: cacheSpan, from: 0, to: Cache.Length - 1, value: value);
                }

                Cache[0] = value;
                ArrayCount--;
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            value = default;
            return false;
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            value = default;
            return false;
        }
    }
}