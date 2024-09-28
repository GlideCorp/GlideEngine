
using Core.Collections.LinkedLists.Nodes;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.LinkedLists
{
    public class OptimizedChunkLinkedList<TValue>(int chunkSize) : OptimizedLinkedChunkList<TValue, TValue>(chunkSize, new DefaultFilter<TValue>())
        where TValue : IComparable<TValue>, IEquatable<TValue>;

    public class OptimizedLinkedChunkList<TKey, TValue>(int chunkSize, IFilter<TKey, TValue> defaultFilter) : ILinkedList<TKey, TValue, DoublyLinkedChunkNode<TValue>>
    {
        public IFilter<TKey, TValue> DefaultFilter { get; init; } = defaultFilter;

        public DoublyLinkedChunkNode<TValue>? FirstNode { get; set; } = null;
        public DoublyLinkedChunkNode<TValue>? LastNode { get; set; } = null;

        public int Count { get; private set; } = 0;
        public bool IsPacked { get; protected set; } = false;
        public int ChunkSize { get; init; } = chunkSize;

        protected DoublyLinkedChunkNode<TValue>? PoolHead { get; set; }

        public TValue this[int index]
        {
            get
            {
                Debug.Assert(index < Count);
                if (index < ChunkSize) { return FirstNode!.Values[index]; }

                if (index >= Count - LastNode!.NextValueIndex) { return LastNode!.Values[Count - 1 - index]; }

                index -= ChunkSize;
                DoublyLinkedChunkNode<TValue> current = FirstNode!.Next!;
                while (index >= ChunkSize)
                {
                    index -= ChunkSize;
                    current = current.Next!;
                }

                return current.Values[index];
            }
        }

        private DoublyLinkedChunkNode<TValue> CreateNode(TValue value, DoublyLinkedChunkNode<TValue>? previous, DoublyLinkedChunkNode<TValue>? next)
        {
            DoublyLinkedChunkNode<TValue> newNode;
            if (PoolHead is null) { newNode = new(ChunkSize, value, previous, next); }
            else
            {
                newNode = PoolHead;
                PoolHead = PoolHead.Next;
                newNode.Values[0] = value;
                newNode.NextValueIndex = 1;
                newNode.Previous = previous;
                newNode.Next = next;
            }

            return newNode;
        }

        private void InsertZero(TValue value)
        {
            FirstNode = LastNode = CreateNode(value, previous: null, next: null);
            Count = 1;
        }

        private void InsertFirstNoChecks(TValue value)
        {
            Count++;

            if (FirstNode!.NextValueIndex < ChunkSize) { FirstNode.Values[FirstNode.NextValueIndex++] = value; return; }

            if (LastNode!.NextValueIndex == ChunkSize)
            {
                DoublyLinkedChunkNode<TValue> newNode = CreateNode(FirstNode.Values[^1], previous: LastNode, next: null);
                LastNode.Next = newNode;
                LastNode = newNode;
            }

            ArrayHelper.CopyOffset(
                source: FirstNode.Values, destination: FirstNode.Values,
                sourceOffset: 0, destinationOffset: 1, length: ChunkSize - 1);
            FirstNode.Values[0] = value;
        }

        private void InsertLastNoChecks(TValue value)
        {
            if (LastNode!.NextValueIndex == ChunkSize)
            {
                DoublyLinkedChunkNode<TValue> newNode = CreateNode(value, previous: LastNode, next: null);
                LastNode.Next = newNode;
                LastNode = newNode;
            }
            else { LastNode.Values[LastNode.NextValueIndex++] = value; }

            Count++;
        }

        public void InsertFirst(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            InsertFirstNoChecks(value);
        }

        public void InsertLast(TValue value)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { InsertZero(value); return; }

            InsertLastNoChecks(value);
        }

        public void Insert(TValue value) { InsertLast(value); }

        public void InsertAt(TValue value, int index)
        {
            Debug.Assert(index <= Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (index < ChunkSize)
            {
                if (Count == 0) { InsertZero(value); }
                else { InsertFirstNoChecks(value); }
                return;
            }

            if (index > Count - LastNode!.NextValueIndex) { InsertLastNoChecks(value); return; }

            index -= ChunkSize;
            DoublyLinkedChunkNode<TValue> current = FirstNode!.Next!;
            while (index >= ChunkSize)
            {
                index -= ChunkSize;
                current = current.Next!;
            }

            if (LastNode!.NextValueIndex == ChunkSize)
            {
                DoublyLinkedChunkNode<TValue> newNode = CreateNode(current.Values[index], previous: LastNode, next: null);
                LastNode.Next = newNode;
                LastNode = newNode;
            }
            else { LastNode.Values[LastNode.NextValueIndex++] = current.Values[index]; }

            current.Values[index] = value;
            Count++;
        }

        private void RemoveZero()
        {
            FirstNode!.Values[0] = default!;

            FirstNode.Next = PoolHead;
            PoolHead = FirstNode;
            FirstNode = LastNode = null;

            Count = 0;
        }

        private void UnlinkLastNode()
        {
            LastNode!.Next = PoolHead;
            PoolHead = LastNode;
            LastNode = LastNode.Previous;
            PoolHead.Previous = LastNode!.Next = null;
        }

        private void RemoveFromCurrentNoChecks(DoublyLinkedChunkNode<TValue> current, int index)
        {
            ArrayHelper.CopyOffset(
                source: current.Values, destination: current.Values,
                sourceOffset: index + 1, destinationOffset: index, length: ChunkSize - 1 - index);

            LastNode!.NextValueIndex--;
            current.Values[^1] = LastNode.Values[LastNode.NextValueIndex];
            LastNode.Values[LastNode.NextValueIndex] = default!;

            Count--;

            if (LastNode.NextValueIndex == 0) { UnlinkLastNode(); }
        }

        private void RemoveFromLastNoChecks(int index)
        {
            LastNode!.NextValueIndex--;
            LastNode.Values[index] = LastNode.Values[LastNode!.NextValueIndex]!;
            LastNode.Values[LastNode!.NextValueIndex] = default!;
            Count--;

            if (LastNode.NextValueIndex == 0) { UnlinkLastNode(); }
        }

        public void RemoveFirst()
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (Count == 1) { RemoveZero(); return; }

            RemoveFromCurrentNoChecks(current: FirstNode!, index: 0);
        }

        public void RemoveLast()
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (Count == 1) { RemoveZero(); return; }

            LastNode!.NextValueIndex--;
            LastNode.Values[LastNode!.NextValueIndex] = default!;
            Count--;

            if (LastNode.NextValueIndex == 0) { UnlinkLastNode(); }
        }

        private bool TryRemove(DoublyLinkedChunkNode<TValue> current, IFilter<TKey, TValue> filter)
        {
            Span<TValue> span = current.Values.AsSpan(0, current.NextValueIndex);
            for (int i = 0; i < span.Length; i++)
            {
                if (!filter.Match(span[i])) { continue; }

                RemoveFromCurrentNoChecks(current, index: i);
                return true;
            }

            return false;
        }

        public void Remove(TKey key)
        {
            DefaultFilter.Key = key;
            Remove(DefaultFilter);
        }

        public void Remove(IFilter<TKey, TValue> filter)
        {
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (TryRemove(current: FirstNode!, filter)) { return; }

            if (Count > ChunkSize && TryRemove(current: LastNode!, filter)) { return; }

            DoublyLinkedChunkNode<TValue>? current = FirstNode!.Next!;
            while (current != LastNode)
            {
                if (TryRemove(current, filter)) { return; }
                current = current.Next!;
            }
        }

        public void RemoveAt(int index)
        {
            Debug.Assert(index < Count);
            if (IsPacked) { throw new InvalidOperationException(); }

            if (Count == 0) { return; }

            if (index < ChunkSize) { RemoveFromCurrentNoChecks(current: FirstNode!, index); return; }

            if (index >= Count - LastNode!.NextValueIndex) { RemoveFromLastNoChecks(index); return; }

            index -= ChunkSize;
            DoublyLinkedChunkNode<TValue> current = FirstNode!.Next!;
            while (index >= ChunkSize)
            {
                index -= ChunkSize;
                current = current.Next!;
            }

            RemoveFromCurrentNoChecks(current, index);
        }

        public bool Search(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultFilter.Key = key;
            return Search(DefaultFilter, out value);
        }

        public bool Search(IFilter<TKey, TValue> filter, [NotNullWhen(true)] out TValue? value)
        {
            DoublyLinkedChunkNode<TValue>? current = FirstNode;
            while (current is not null)
            {
                Span<TValue> span = current.Values.AsSpan(0, current.NextValueIndex);
                for (int i = 0; i < span.Length; i++)
                {
                    if (!filter.Match(span[i])) { continue; }
                    value = span[i]!;
                    return true;
                }
                current = current.Next;
            }

            value = default;
            return false;
        }

        public void Clear()
        {
            FirstNode = LastNode = null;
            Count = 0;
        }

        public void Pack()
        {
            IsPacked = true;
            PoolHead = null;
        }

        public IEnumerable<TValue> Filter(TKey key)
        {
            DefaultFilter.Key = key;
            return Filter(DefaultFilter);
        }

        public IEnumerable<TValue> Filter(IFilter<TKey, TValue> filter)
        {
            DoublyLinkedChunkNode<TValue>? current = FirstNode;

            while (current is not null)
            {
                for (int i = 0; i < current.NextValueIndex; i++) { if (filter.Match(current.Values[i])) { yield return current.Values[i]; } }
                current = current.Next;
            }
        }
    }
}