
namespace Core.Collections.Interfaces
{
    public interface IStack<TValue> : ICollection<TValue>
    {
        public void Push(TValue value);

        public TValue Pop();

        public TValue Peek();
    }
}
