
namespace Core.Collections
{
    public interface IMatcher<TKey, in TValue>
    {
        public TKey Key { get; set; }

        public bool Match(TValue value);
    }
}