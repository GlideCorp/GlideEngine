
namespace Core.Collections.Lists
{
    public interface IList<TKey, TValue> : ICollection<TKey, TValue>
    {
        /// <summary>
        /// The array used in the background
        /// </summary>
        public TValue[] BackedArray { get; }
    }
}
