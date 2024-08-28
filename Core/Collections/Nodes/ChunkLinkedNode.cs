
namespace Core.Collections.Nodes
{
    public class ChunkLinkedNode<TKey, TValue>
    {
        public TValue?[] Values;
        public int Cursor;

        public ChunkLinkedNode<TKey, TValue>? Previous;
        public ChunkLinkedNode<TKey, TValue>? Next;

        public ChunkLinkedNode(int size)
        {
            Values = new TValue?[size];
            Cursor = 0;

            Previous = null;
            Next = null;
        }

        public ChunkLinkedNode(int size, ChunkLinkedNode<TKey, TValue>? previous, ChunkLinkedNode<TKey, TValue>? next)
        {
            Values = new TValue?[size];
            Cursor = 0;

            Previous = previous;
            Next = next;
        }
    }
    /*
     public class ChunkLinkedNode<TKey, TValue>
           where TValue : notnull
       {
           public TValue[] Values;
           public int Cursor;
       
           public ChunkLinkedNode<TKey, TValue> Previous;
           public ChunkLinkedNode<TKey, TValue> Next;
       
           public ChunkLinkedNode(int size, TValue value)
           {
               Values = new TValue[size];
               Values[0] = value;
               Cursor = 1;
       
               Previous = this;
               Next = this;
           }
       
           public ChunkLinkedNode(int size, TValue value, ChunkLinkedNode<TKey, TValue> previous, ChunkLinkedNode<TKey, TValue> next)
           {
               Values = new TValue[size];
               Values[0] = value;
               Cursor = 1;
       
               Previous = previous;
               Next = next;
           }
       }
     */
}
