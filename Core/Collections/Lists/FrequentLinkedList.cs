
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Lists
{
    public class FrequentLinkedList<TKey, TValue>(IMatcher<TKey, TValue> defaultMatcher) : SinglyLinkedList<TKey, TValue>(defaultMatcher)
    {
        public new bool Find(TKey key, [NotNullWhen(true)] out TValue? value)
        {
            DefaultMatcher.Key = key;
            return Find(DefaultMatcher, out value);
        }

        public new bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (First is null) { goto ReturnDefault; }

            if (matcher.Match(First.Value))
            {
                value = First.Value!;
                return true;
            }

            if (First.Next is null) { goto ReturnDefault; }

            SinglyLinkedNode<TValue> previous = First;
            while (previous.Next != Last)
            {
                if (matcher.Match(previous.Next!.Value))
                {
                    value = previous.Next.Value!;

                    SinglyLinkedNode<TValue> toFront = previous.Next;
                    previous.Next = toFront.Next;
                    toFront.Next = First;
                    First = toFront;

                    return true;
                }

                previous = previous.Next;
            }

            if (matcher.Match(Last!.Value))
            {
                value = Last.Value!;

                Last.Next = First;
                First = Last;
                Last = previous;
                previous.Next = null;

                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }
    }
}