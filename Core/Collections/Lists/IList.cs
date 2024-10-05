
namespace Core.Collections.Lists
{
    public interface IList<TValue> : ICollection<TValue>
    {
        /// <summary>
        /// The array used behind the scenes
        /// </summary>
        public TValue[] BackingArray { get; }
    }
}
