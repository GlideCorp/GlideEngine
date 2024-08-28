using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections.Trees
{
    public interface ITree<TKey, TValue> : IBasicCollection<TKey, TValue>
        where TKey : notnull
    {
        public void Insert(IEnumerable<TKey> keyParts, TValue value);
        public void Remove(IEnumerable<TKey> keyParts);
    }
}
