
using Core.Collections.Interfaces;
using Core.Collections.LinkedLists;
using System.Runtime.CompilerServices;

namespace Core.Collections.Stacks
{
    public class Stack<TValue> : IStack<TValue>
    {
        public int Count => BackingCollection.Count;
        private SinglyLinkedList<TValue> BackingCollection { get; set; } = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(TValue value) { Push(value); }

        public void Push(TValue value) { BackingCollection.InsertFirst(value); }

        public TValue Pop()
        {
            TValue value = BackingCollection.ValueAt(0);
            BackingCollection.RemoveFirst();
            return value;
        }

        public TValue Peek() { return BackingCollection.ValueAt(0); }

        public void Clear() { BackingCollection.Clear(); }
    }
}
