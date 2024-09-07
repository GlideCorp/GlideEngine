
using System;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class List<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : ICollection<TKey, TValue>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        protected TValue[] Array { get; set; } = [];
        public int Count { get; protected set; } = 0;

        protected int IncreaseSize() { return Math.Max(Array.Length * 2, 2); }
        protected int DecreaseSize() { return Array.Length / 2; }

        public void InsertFirst(TValue value)
        {
            if (Count == Array.Length)
            {
                int newSize = IncreaseSize();
                TValue[] newArray = new TValue[newSize];

                System.Array.Copy(Array, sourceIndex: 0, newArray, destinationIndex: 1, length: Count);
                Array = newArray;
            }
            else { System.Array.Copy(Array, sourceIndex: 0, Array, destinationIndex: 1, length: Count); }

            Array[0] = value;
            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (Count == Array.Length)
            {
                int newSize = IncreaseSize();
                TValue[] newArray = new TValue[newSize];

                System.Array.Copy(Array, newArray, Count);
                Array = newArray;
            }

            Array[Count++] = value;
        }

        public void Insert(TValue value) { InsertLast(value); }

        public void Remove(TKey key)
        {
            DefaultMatcher.Key = key;
            Remove(DefaultMatcher);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (Count == 0) { return; }

            Span<TValue> span = Array.AsSpan(0, Count);
            for (int i = 0; i < span.Length; i++)
            {
                if (!matcher.Match(span[i])) { continue; }

                Count--;
                if (Count == 0) { Array = []; return; }

                if (Count <= Array.Length / 4)
                {
                    int newSize = DecreaseSize();

                    TValue[] newArray = new TValue[newSize];
                    System.Array.Copy(Array, sourceIndex: 0, newArray, destinationIndex: 0, length: i);
                    System.Array.Copy(Array, sourceIndex: i + 1, newArray, destinationIndex: i, length: Count - i);
                    Array = newArray;
                }
                else
                {
                    System.Array.Copy(Array, sourceIndex: i + 1, Array, destinationIndex: i, length: Count - i);
                    span[Count] = default!;
                }

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