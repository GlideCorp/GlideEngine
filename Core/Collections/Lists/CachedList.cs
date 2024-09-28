
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class CachedList<TKey, TValue>(int cacheSize, IFilter<TKey, TValue> defaultFilter) //: ICollection<TKey, TValue>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; } = defaultFilter;

        protected TValue[] Array { get; set; } = [];
        protected TValue[] Cache { get; set; } = new TValue[cacheSize];

        public int Count => ArrayCount + CacheCount;
        protected int ArrayCount { get; set; } = 0;
        protected int CacheCount { get; set; } = 0;

        protected int GrowthFactor { get; set; } = 2;
        protected int ShrinkFactor { get; set; } = 2;

        protected int Growth() { return Math.Max(Array.Length * GrowthFactor, 2); }
        protected int Shrink() { return Array.Length / ShrinkFactor; }

        private void ResizeArray(int newSize)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> arraySpan = Array.AsSpan(0, ArrayCount);
            Span<TValue> newArraySpan = newArray.AsSpan(0, ArrayCount);
            arraySpan.CopyTo(newArraySpan);

            Array = newArray;
        }

        private void ResizeArrayWithoutElementAt(int newSize, int skipIndex)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> arraySpan = Array.AsSpan(0, skipIndex);
            Span<TValue> newArraySpan = newArray.AsSpan(0, skipIndex);
            arraySpan.CopyTo(newArraySpan);

            arraySpan = Array.AsSpan(skipIndex + 1, ArrayCount);
            newArraySpan = newArray.AsSpan(skipIndex, ArrayCount - 1);
            arraySpan.CopyTo(newArraySpan);

            Array = newArray;
        }

        private static void PushBackAndAddLast(Span<TValue> source, Span<TValue> destination, int from, int to, TValue value)
        {
            Span<TValue> moveSource = source[from..to];
            Span<TValue> moveDestination = destination[(from - 1)..(to - 1)];
            moveSource.CopyTo(moveDestination);

            destination[to - 1] = value;
        }

        private static void PushForwardAndAddFirst(Span<TValue> source, Span<TValue> destination, int from, int to, TValue value)
        {
            Span<TValue> moveSource = source[from..to];
            Span<TValue> moveDestination = destination[(from + 1)..(to + 1)];
            moveSource.CopyTo(moveDestination);

            destination[from] = value;
        }

        public void Insert(TValue value)
        {
            if (ArrayCount == Array.Length) { ResizeArray(newSize: Growth()); }
            Array[ArrayCount++] = value;
        }

        private bool RemoveFromCache(IFilter<TKey, TValue> filter)
        {
            Span<TValue> span = Cache.AsSpan(0, CacheCount);
            for (int i = 0; i < span.Length; i++)
            {
                if (filter.Match(span[i])) { continue; }

                PushBackAndAddLast(source: span, destination: span, from: i + 1, to: CacheCount, value: default!);
                CacheCount--;
                return true;
            }

            return false;
        }

        private void RemoveFromArray(IFilter<TKey, TValue> filter)
        {
            Span<TValue> span = Array.AsSpan(0, ArrayCount);
            for (int i = 0; i < span.Length; i++)
            {
                if (filter.Match(span[i])) { continue; }

                if (ArrayCount == 1) { Array = []; }
                else if (ArrayCount - 1 <= Array.Length / 4)
                    ResizeArrayWithoutElementAt(newSize: Shrink(), skipIndex: i);
                else
                    PushBackAndAddLast(source: span, destination: span, from: i + 1, to: ArrayCount, value: default!);

                ArrayCount--;
                return;
            }
        }

        public void Remove(TKey key)
        {
            DefaultFilter.Key = key;
            Remove(DefaultFilter);
        }

        public void Remove(IFilter<TKey, TValue> filter) { if (!RemoveFromCache(filter)) { RemoveFromArray(filter); } }

        private bool FindInCache(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = Cache.AsSpan(0, CacheCount);
            for (int i = 0; i < span.Length; i++)
            {
                if (!filter.Match(span[i])) { continue; }

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
                ResizeArrayWithoutElementAt(newSize: Shrink(), skipIndex: ArrayCount - 1);
            else { Array[ArrayCount - 1] = default!; }
        }

        private bool FindInList(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> arraySpan = Array.AsSpan(0, ArrayCount);
            for (int i = 0; i < arraySpan.Length; i++)
            {
                if (!filter.Match(arraySpan[i])) { continue; }

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

        public bool Find(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            value = default;
            return false;
        }
    }
}