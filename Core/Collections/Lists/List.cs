
using Core.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class List<TValue> : List<TValue, TValue>
        where TValue : IComparable<TValue>, IEquatable<TValue>
    {
        public List() : base(new DefaultFilter<TValue>()) { }
        public List(int initialSize) : base(initialSize, new DefaultFilter<TValue>()) { }
    }

    public class List<TKey, TValue> //: IList<TKey, TValue>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; }

        protected TValue[] Array { get; set; }
        public int Count { get; protected set; }

        protected float GrowthFactor { get; set; } = 2;
        protected float ShrinkFactor { get; set; } = 0.5f;

        public List(IFilter<TKey, TValue> defaultFilter)
        {
            DefaultFilter = defaultFilter;

            Array = [];
            Count = 0;
        }

        public List(int initialSize, IFilter<TKey, TValue> defaultFilter)
        {
            DefaultFilter = defaultFilter;

            Array = initialSize == 0 ? [] : new TValue[initialSize];
            Count = 0;
        }

        private int Growth() { return Math.Max((int)(Array.Length * GrowthFactor), 2); }
        private int Shrink() { return (int)(Array.Length * ShrinkFactor); }

        public void InsertFirst(TValue value)
        {
            if (Count == Array.Length)
            {
                int newLength = Growth();
                TValue[] newArray = new TValue[newLength];
                ArrayHelper.CopyOffset(source: Array, destination: newArray, sourceOffset: 0, destinationOffset: 1, Count);
            }
            else
            {
                ArrayHelper.CopyOffset(source: Array, destination: Array, sourceOffset: 0, destinationOffset: 1, Count);
            }

            Array[0] = value;
            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (Count == Array.Length)
            {
                int newLength = Growth();
                TValue[] newArray = new TValue[newLength];
                ArrayHelper.CopyUntil(source: Array, destination: newArray, untilExclusive: Count);

                Array = newArray;
            }

            Array[Count++] = value;
        }

        public void Insert(TValue value) { InsertLast(value); }

        public void Remove(TKey key)
        {
            DefaultFilter.Key = key;
            Remove(DefaultFilter);
        }

        public void Remove(IFilter<TKey, TValue> filter)
        {
            Span<TValue> span = Array.AsSpan(0, Count);
            for (int i = 0; i < span.Length; i++)
            {
                if (filter.Match(span[i])) { continue; }

                if (Count == 1) { Array = []; }
                else if (Count - 1 <= Array.Length / 4)
                {
                    int newLength = Shrink();
                    TValue[] newArray = new TValue[newLength];
                    ArrayHelper.CopyUntil(source: Array, destination: newArray, untilExclusive: i);
                    ArrayHelper.CopyOffset(
                        source: Array, destination: newArray, 
                        sourceOffset: i + 1, destinationOffset: i,
                        length: Count - i - 1);

                    Array = newArray;
                }
                else
                {
                    ArrayHelper.CopyOffset(
                        source: Array, destination: Array,
                        sourceOffset: i + 1, destinationOffset: i,
                        length: Count - i - 1);

                    Array[Count] = default!;
                }

                Count--;
                return;
            }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultFilter.Key = key;
            return Find(DefaultFilter, out value);
        }

        public bool Find(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = Array.AsSpan(0, Count);
            foreach (TValue val in span)
            {
                if (!filter.Match(val)) { continue; }

                value = val!;
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }

        public void Pack()
        {
            if (Array.Length == Count) { return; }

            TValue[] newArray = new TValue[Count];
            ArrayHelper.Copy(source: Array, destination: newArray);
            Array = newArray;
        }

        public TValue[] ToArray()
        {
            TValue[] newArray = new TValue[Count];
            ArrayHelper.Copy(source: Array, destination: newArray);
            return newArray;
        }
    }
}