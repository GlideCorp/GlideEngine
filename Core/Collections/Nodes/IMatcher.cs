
namespace Core.Collections.Nodes
{
    public interface IMatcher<TKey, in TValue>
        where TKey : notnull
    {
        public TKey Key { get; set; }

        public bool Match(TValue value);
    }
}
