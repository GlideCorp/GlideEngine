

namespace Core.Collections.Interfaces
{
    public interface IQueue<TValue> : ICollection<TValue>
    {
        public void Enqueue(TValue value);

        public TValue Dequeue();

        public TValue Peek();
    }
}

