
using System;

namespace Core.Collections.Lists
{
    public class BinaryCachedList<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher)
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        protected TValue[] Array { get; set; } = [];
        public int Count { get; protected set; } = 0;

        protected int IncreaseSize() { return Math.Max(Array.Length * 2, 2); }
        protected int DecreaseSize() { return Array.Length / 2; }

        private int IndexOf(TKey key)
        {
            if (Count == 0) { return -1; }

            DefaultMatcher.Key = key;
            Span<TValue> span = Array.AsSpan(0, Count);
            int left = 0;
            int right = span.Length - 1;
            int middle = right / 2;

            while (left <= right)
            {
                middle = (left + right) / 2;
                int comparison = DefaultMatcher.Compare(span[middle]);
                switch (comparison)
                {
                    case 0: return middle;
                    case < 0: left = middle + 1; break;
                    default: right = middle - 1; break;
                }
            }

            return ~middle;
        }

        public void OrderedInsert(TKey key, TValue value)
        {
            int index = IndexOf(key);
            if (index >= 0) { return; }

            index = ~index;
            if (Count == Array.Length)
            {
                int newSize = IncreaseSize();
                TValue[] newArray = new TValue[newSize];

                System.Array.Copy(Array, sourceIndex: 0, newArray, destinationIndex: 0, length: index);
                System.Array.Copy(Array, sourceIndex: index, newArray, destinationIndex: index + 1, length: Array.Length - index);
                Array = newArray;
            }
            else { System.Array.Copy(Array, sourceIndex: index, Array, destinationIndex: index + 1, length: Array.Length - index); }

            Array[index] = value;
            Count++;
        }

        public void OrderedRemove(TKey key)
        {

        }
    }
}
