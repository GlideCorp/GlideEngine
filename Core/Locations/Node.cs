using System.Diagnostics.CodeAnalysis;

namespace Core.Locations
{
    internal class Node
    {
        public ReadOnlyMemory<char> SubPath { get; init; }

        private Node? Parent { get; init; }
        private List<Node> Children { get; init; }
        public Trackable? Value { get; set; }

        public Node()
        {
            SubPath = "/".AsMemory();

            Parent = null;
            Children = [];
            Value = null;
        }

        public Node(ReadOnlyMemory<char> subPath, Node parent)
        {
            SubPath = subPath;

            Parent = parent;
            Children = [];
            Value = null;
        }

        public Node(ReadOnlyMemory<char> subPath, Node parent, Trackable value)
        {
            SubPath = subPath;

            Parent = parent;
            Children = [];
            Value = value;
        }

        private int Search(ReadOnlyMemory<char> subPath)
        {
            // TODO: make singletone comparer class
            Comparer<Node> comparer = Comparer<Node>.Create((x, _) =>
            {
                return x.SubPath.Span.CompareTo(subPath.Span, StringComparison.OrdinalIgnoreCase);
            });

            if (Children.Count > 32) { return Children.BinarySearch(null!, comparer); }
            else
            {
                for (int i = 0; i < Children.Count; i++)
                {
                    int comp = comparer.Compare(Children[i], null!);
                    if (comp > 0) { return ~i; }
                    else if (comp == 0) { return i; }
                }

                return ~Children.Count;
            }
        }

        public bool Append(ReadOnlyMemory<char> subPath, out Node child)
        {
            int index = Search(subPath);

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

        public bool Append(ReadOnlyMemory<char> subPath, int index, out Node child)
        {
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
            if (Children.Count == 0 && Parent is not null)
            {
                Parent.Detatch(SubPath);
            }
        }

        public bool Detatch(ReadOnlyMemory<char> subPath)
        {
            int index = Search(subPath);

            if (index >= 0)
            {
                Children.RemoveAt(index);
                DetatchIfEmpty();
                return true;
            }
            else { return false; }
        }

        public bool Find(ReadOnlyMemory<char> subPath, [NotNullWhen(true)] out Node? child)
        {
            int index = Search(subPath);

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

        public bool Find(ReadOnlyMemory<char> subPath, out int index, [NotNullWhen(true)] out Node? child)
        {
            index = Search(subPath);

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

        public IEnumerable<Trackable> RetriveValues()
        {
            if (Value is not null) { yield return Value; }

            foreach (Node node in RetriveChildern(recursive: true))
            {
                if (node.Value is not null) { yield return node.Value; }
            }
        }

        public override string ToString()
        {
            return $$"""
                {{nameof(Node)}}
                {
                    {{nameof(SubPath)}}: "{{SubPath}}",
                    {{nameof(Children)}}: <{{nameof(List<Node>)}}, Count: {{Children.Count}}>,
                    {{nameof(Value)}}: {{Value}}
                }
                """;
        }
    }
}
