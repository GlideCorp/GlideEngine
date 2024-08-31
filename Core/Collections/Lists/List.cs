
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class List<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : ICollection<TKey, TValue>
    {
        public IMatcher<TKey, TValue> DefaultMatcher { get; init; } = defaultMatcher;

        private TValue[] _array = [];
        public int Count { get; private set; } = 0;

        private int IncreaseSize() { return Math.Max(_array.Length * 2, 2); }
        private int DecreaseSize() { return _array.Length / 2; }

        public void InsertFirst(TValue value)
        {
            if (Count == _array.Length)
            {
                int newSize = IncreaseSize();
                TValue[] newArray = new TValue[newSize];

                Array.Copy(_array, sourceIndex: 0, newArray, destinationIndex: 1, length: Count);
                _array = newArray;
            }
            else { Array.Copy(_array, sourceIndex: 0, _array, destinationIndex: 1, length: Count); }

            _array[0] = value;
            Count++;
        }

        public void InsertLast(TValue value)
        {
            if (Count == _array.Length)
            {
                int newSize = IncreaseSize();
                TValue[] newArray = new TValue[newSize];

                Array.Copy(_array, newArray, Count);
                _array = newArray;
            }

            _array[Count++] = value;
        }

        public void Insert(TValue value) { InsertLast(value); }

        public void Remove(TKey key)
        {
            if (Count == 0) { return; }

            DefaultMatcher.Key = key;
            Span<TValue> span = _array.AsSpan(0, Count);

            for (int i = 0; i < span.Length; i++)
            {
                if (!DefaultMatcher.Match(span[i])) { continue; }

                Count--;
                if (Count == 0) { _array = []; return; }

                if (Count == _array.Length / 4)
                {
                    int newSize = DecreaseSize();

                    TValue[] newArray = new TValue[newSize];
                    Array.Copy(_array, sourceIndex: 0, newArray, destinationIndex: 0, length: i);
                    Array.Copy(_array, sourceIndex: i + 1, newArray, destinationIndex: i, length: Count - i);
                    _array = newArray;
                }
                else
                {
                    Array.Copy(_array, sourceIndex: i + 1, _array, destinationIndex: i, length: Count - i);
                    _array[Count] = default!;
                }

                return;
            }
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            if (Count == 0) { return; }

            Span<TValue> span = _array.AsSpan(0, Count);
            for (int i = 0; i < span.Length; i++)
            {
                if (!matcher.Match(span[i])) { continue; }

                Count--;
                if (Count == 0) { _array = []; return; }

                if (Count == _array.Length / 4)
                {
                    int newSize = DecreaseSize();

                    TValue[] newArray = new TValue[newSize];
                    Array.Copy(_array, sourceIndex: 0, newArray, destinationIndex: 0, length: i);
                    Array.Copy(_array, sourceIndex: i + 1, newArray, destinationIndex: i, length: Count - i);
                    _array = newArray;
                }
                else
                {
                    Array.Copy(_array, sourceIndex: i + 1, _array, destinationIndex: i, length: Count - i);
                    _array[Count] = default!;
                }

                return;
            }
        }

        public bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            DefaultMatcher.Key = key;
            Span<TValue> span = _array.AsSpan(0, Count);

            foreach (TValue val in span)
            {
                if (!DefaultMatcher.Match(val)) { continue; }

                value = val!;
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (Count == 0) { goto ReturnDefault; }

            Span<TValue> span = _array.AsSpan(0, Count);

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