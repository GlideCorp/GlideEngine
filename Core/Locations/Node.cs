using System.Diagnostics.CodeAnalysis;

namespace Core.Locations
{
    internal class Node
    {
        public string SubPath { get; init; }

        private Node? Parent { get; init; }
        private List<Trackable> Values { get; init; }
        private List<Node> Children { get; init; }

        public Node()
        {
            SubPath = "/";

            Parent = null;
            Values = [];
            Children = [];
        }

        public Node(string subPath, Node parent)
        {
            SubPath = subPath;

            Parent = parent;
            Values = [];
            Children = [];
        }

        public bool Append(string subPath, out Node child)
        {
            // TODO: make singletone comparer class
            Comparer<Node> comparer = Comparer<Node>.Create((x, _) =>
            {
                return x.SubPath.CompareTo(subPath);
            });
            int index = Children.BinarySearch(null!, comparer);

            if (index >= 0)
            {
                child = Children[index];
                return false;
            }
            else
            {
                child = new(subPath, this);
                Children.Insert(~index, child);
                return true;
            }
        }

        private void DetatchIfEmpty()
        {
            if (Children.Count == 0 && Values.Count == 0 && Parent is not null)
            {
                Parent.Detatch(SubPath);
            }
        }

        public bool Detatch(string subPath)
        {
            // TODO: make singletone comparer class
            Comparer<Node> comparer = Comparer<Node>.Create((x, _) =>
            {
                return x.SubPath.CompareTo(subPath);
            });
            int index = Children.BinarySearch(null!, comparer);

            if (index >= 0)
            {
                Children.RemoveAt(index);
                DetatchIfEmpty();

                return true;
            }
            else { return false; }
        }

        public bool Insert(Trackable trackable)
        {
            int index = Values.BinarySearch(trackable);
            if (index >= 0) { return false; }
            else
            {
                Values.Insert(~index, trackable);
                return true;
            }
        }

        public bool Remove(Trackable trackable)
        {
            int index = Values.BinarySearch(trackable);
            if (index >= 0)
            {
                Values.RemoveAt(index);
                DetatchIfEmpty();

                return true;
            }
            else { return false; }
        }

        public bool Find(string subPath, [NotNullWhen(true)] out Node? child)
        {
            // TODO: make singletone comparer class
            Comparer<Node> comparer = Comparer<Node>.Create((x, _) =>
            {
                return x.SubPath.CompareTo(subPath);
            });
            int index = Children.BinarySearch(null!, comparer);

            if (index >= 0)
            {
                child = Children[index];
                return true;
            }
            else
            {
                child = null;
                return false;
            }
        }

        public IEnumerable<Node> RetriveChildern(bool recursive = false)
        {
            if (recursive)
            {
                Queue<Node> queue = new();

                for (int i = 0; i < Children.Count; i++)
                {
                    yield return Children[i];
                    queue.Enqueue(Children[i]);
                }

                while (queue.Count > 0)
                {
                    Node node = queue.Dequeue();
                    for (int i = 0; i < node.Children.Count; i++)
                    {
                        yield return node.Children[i];
                        queue.Enqueue(node.Children[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Children.Count; i++) { yield return Children[i]; }
            }
        }

        public IEnumerable<Trackable> RetriveValues(bool recursive = false)
        {
            if (recursive)
            {
                Queue<Node> queue = new();

                for (int i = 0; i < Values.Count; i++) { yield return Values[i]; }
                for (int i = 0; i < Children.Count; i++) { queue.Enqueue(Children[i]); }

                while (queue.Count > 0)
                {
                    Node node = queue.Dequeue();

                    for (int i = 0; i < node.Values.Count; i++) { yield return node.Values[i]; }
                    for (int i = 0; i < node.Children.Count; i++) { queue.Enqueue(node.Children[i]); }
                }
            }
            else
            {
                for (int i = 0; i < Values.Count; i++) { yield return Values[i]; }
            }
        }

        public override string ToString()
        {
            return $$"""
                {{nameof(Node)}}
                {
                    {{nameof(SubPath)}}: "{{SubPath}}",
                    {{nameof(Children)}}: <{{nameof(List<Node>)}}, Count: {{Children.Count}}>,
                    {{nameof(Values)}}: <{{nameof(List<Trackable>)}}, Count: {{Values.Count}}>
                }
                """;
        }
    }
}
