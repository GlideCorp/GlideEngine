
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class List<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : ICollection<TKey, TValue>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        protected TValue[] Array { get; set; } = [];
        public int Count { get; protected set; } = 0;

        protected int GrowthFactor { get; set; } = 2;
        protected int ShrinkFactor { get; set; } = 2;

        protected int Growth() { return Math.Max(Array.Length * GrowthFactor, 2); }
        protected int Shrink() { return Array.Length / ShrinkFactor; }

        private void ResizeArray(int newSize)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> arraySpan = Array.AsSpan(0, Count);
            Span<TValue> newArraySpan = newArray.AsSpan(0, Count);
            arraySpan.CopyTo(newArraySpan);

            Array = newArray;
        }

        private void ResizeArrayAndAddLast(int newSize, TValue value)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> arraySpan = Array.AsSpan(0, Count);
            Span<TValue> newArraySpan = newArray.AsSpan(0, Count);
            arraySpan.CopyTo(newArraySpan);

            newArray[Count] = value;
            Array = newArray;
        }

        private void ResizeArrayAndAddFirst(int newSize, TValue value)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> arraySpan = Array.AsSpan(0, Count);
            Span<TValue> newArraySpan = newArray.AsSpan(1, Count);
            arraySpan.CopyTo(newArraySpan);

            newArraySpan[0] = value;
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

        private void ResizeArrayWithoutElementAt(int newSize, int skipIndex)
        {
            TValue[] newArray = new TValue[newSize];

            Span<TValue> arraySpan = Array.AsSpan(0, skipIndex);
            Span<TValue> newArraySpan = newArray.AsSpan(0, skipIndex);
            arraySpan.CopyTo(newArraySpan);

            arraySpan = Array.AsSpan(skipIndex + 1, Count);
            newArraySpan = newArray.AsSpan(skipIndex, Count - 1);
            arraySpan.CopyTo(newArraySpan);

            Array = newArray;
        }

        public void InsertFirst(TValue value)
        {
            if (Count == Array.Length) { ResizeArrayAndAddFirst(newSize: Growth(), value); }
            else
            {
                Span<TValue> span = Array;
                PushForwardAndAddFirst(span, span, 0, Count, value);
            }

            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (Count == Array.Length) { ResizeArrayAndAddLast(newSize: Growth(), value); }
            else { Array[Count] = value; }

            Count++;
        }

        public void Insert(TValue value) { InsertLast(value); }

        public void Remove(TKey key)
        {
            DefaultMatcher.Key = key;
            Remove(DefaultMatcher);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            Span<TValue> span = Array.AsSpan(0, Count);
            for (int i = 0; i < span.Length; i++)
            {
                if (matcher.Match(span[i])) { continue; }

                if (Count == 1) { Array = []; }
                else if (Count - 1 <= Array.Length / 4)
                    ResizeArrayWithoutElementAt(newSize: Shrink(), skipIndex: i);
                else
                    PushBackAndAddLast(source: span, destination: span, from: i + 1, to: Count, value: default!);

                Count--;
                return;
            }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            return Find(DefaultMatcher, out value);
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = Array.AsSpan(0, Count);
            foreach (TValue val in span)
            {
                if (!matcher.Match(val)) { continue; }

                value = val!;
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }
    }
}