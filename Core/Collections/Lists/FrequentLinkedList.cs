
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

        private void NextToFront(SinglyLinkedNode<TValue> previous)
        {
            SinglyLinkedNode<TValue> toFront = previous.Next!;
            previous.Next = toFront.Next;
            toFront.Next = FirstNode;
            FirstNode = toFront;
        }

        private void LastToFront(SinglyLinkedNode<TValue> previous)
        {
            LastNode!.Next = FirstNode;
            FirstNode = LastNode;
            LastNode = previous;
            previous.Next = null;
        }

        public new bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            if (FirstNode is null) { goto ReturnDefault; }

            if (matcher.Match(FirstNode.Value))
            {
                value = FirstNode.Value!;
                return true;
            }

            if (FirstNode.Next is null) { goto ReturnDefault; }

            SinglyLinkedNode<TValue> previous = FirstNode;
            while (previous.Next != LastNode)
            {
                if (matcher.Match(previous.Next!.Value))
                {
                    value = previous.Next.Value!;
                    NextToFront(previous);
                    return true;
                }

                previous = previous.Next;
            }

            if (matcher.Match(LastNode!.Value))
            {
                value = LastNode.Value!;
                LastToFront(previous);
                return true;
            }

ReturnDefault:
            value = default;
            return false;
        }
    }
}