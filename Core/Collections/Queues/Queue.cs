
using Core.Collections.Interfaces;
using Core.Collections.LinkedLists;
using System.Runtime.CompilerServices;

namespace Core.Collections.Queues
{
    public class Queue<TValue> : IQueue<TValue>
    {
        public int Count => BackingCollection.Count;
        private SinglyLinkedList<TValue> BackingCollection { get; set; } = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(TValue value) { Enqueue(value); }

        public void Enqueue(TValue value) { BackingCollection.InsertLast(value); }

        public TValue Dequeue()
        {
            TValue value = BackingCollection.ValueAt(0);
            BackingCollection.RemoveFirst();
            return value;
        }

        public TValue Peek() { return BackingCollection.ValueAt(0); }

        public void Clear() { BackingCollection.Clear(); }
    }
}
