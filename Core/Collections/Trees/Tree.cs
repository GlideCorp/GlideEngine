
using Core.Collections.Nodes;
using System.Diagnostics.CodeAnalysis;

namespace Core.Collections.Trees
{
    /*
    public class Tree<TKey, TValue>(TKey rootKey, Match<TreeNode<TKey, TValue>> basicMatch) : ITree<TKey, TValue>
        where TKey : notnull
    {
        private readonly TreeNode<TKey, TValue> _root = new(rootKey, parent: null);

        private IEnumerable<TreeNode<TKey, TValue>> TraverseNodes()
        {
            Queue<TreeNode<TKey, TValue>> queue = new();

            foreach (TreeNode<TKey, TValue> child in _root.Children.Traverse())
            {
                yield return child;
                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                TreeNode<TKey, TValue> node = queue.Dequeue();

                foreach (TreeNode<TKey, TValue> child in node.Children.Traverse())
                {
                    yield return child;
                    queue.Enqueue(child);
                }
            }
        }

        public void Insert(IEnumerable<TKey> keyParts, TValue value)
        {
            TreeNode<TKey, TValue> currentNode = _root;

            foreach (TKey key in keyParts)
            {
                if (!currentNode.Children.Find(basicMatch, out TreeNode<TKey, TValue>? newNode))
                {
                    newNode = new(key, parent: currentNode);
                    currentNode.Children.Insert(newNode);
                }

                currentNode = newNode;
            }

            currentNode.Value = value;
            currentNode.IsValueSet = true;
        }

        private void TryRemoveBranch(TreeNode<TKey, TValue> currentNode)
        {
            while (currentNode.Children.Count == 0 &&
                   currentNode is { IsValueSet: false, Parent: not null })
            {
                currentNode = currentNode.Parent;
                currentNode.Children.Remove(basicMatch);
            }
        }

        public void Remove(IEnumerable<TKey> keyParts)
        {
            TreeNode<TKey, TValue> currentNode = _root;

            foreach (TKey key in keyParts)
            {
                if (!currentNode.Children.Find(basicMatch, out TreeNode<TKey, TValue>? newNode)) { return; }
                currentNode = newNode;
            }

            currentNode.Value = default;
            currentNode.IsValueSet = false;
            TryRemoveBranch(currentNode);
        }

        public void Remove(IMatcher<TKey, TValue> matcher)
        {
            foreach (TreeNode<TKey, TValue> node in TraverseNodes())
            {
                if (node.IsValueSet && matcher.Match(node.Value!))
                {
                    node.Value = default;
                    node.IsValueSet = false;
                    TryRemoveBranch(node);
                    return;
                }
            }
        }

        public bool Find(IMatcher<TKey, TValue> matcher, [NotNullWhen(true)] out TValue? value)
        {
            foreach (TreeNode<TKey, TValue> node in TraverseNodes())
            {
                if (!node.IsValueSet || !matcher.Match(node.Value!)) { continue; }
                value = node.Value!;
                return true;
            }

            value = default;
            return false;
        }

        public int CountMatches(IMatcher<TKey, TValue> matcher)
        {
            int count = 0;
            foreach (TreeNode<TKey, TValue> node in TraverseNodes())
            {
                if (node.IsValueSet && matcher.Match(node.Value!)) { count++; }
            }
            return count;
        }

        public IEnumerable<TValue> Traverse()
        {
            foreach (TreeNode<TKey, TValue> node in TraverseNodes())
            {
                if (node.IsValueSet) { yield return node.Value!; }
            }
        }

        public IEnumerable<TValue> Filter(IMatcher<TKey, TValue> matcher)
        {
            foreach (TreeNode<TKey, TValue> node in TraverseNodes())
            {
                if (node.IsValueSet && matcher.Match(node.Value!)) { yield return node.Value!; }
            }
        }
    }
    */
}
