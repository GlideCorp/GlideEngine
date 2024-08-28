
using Core.Collections.Lists;
using SystemList = System.Collections.Generic.List<int>;
using MySinglyLinkedList = Core.Collections.Lists.SinglyLinkedList<int, int>;
using MyDoublyLinkedList = Core.Collections.Lists.DoublyLinkedList<int, int>;
using MyChunkLinkedList = Core.Collections.Lists.ChunkList<int, int>;
using Core.Collections.Nodes;

const int Length = 1000;

void Operate<TList>(
    Func<TList> createList,
    Action<TList, int> addElement,
    Action<TList, int> removeElement,
    Func<TList, int, int> findElement,
    Func<TList, int> getCount)
{
    const int baseline = 50;
    const int staticElements = 30;
    const int addAmount = 15;
    const int removeAmount = addAmount - 2;
    const double constItemRatio = 95 / 100d;

    Random random = new(69);
    TList list = createList();

    int max = 0;
    int indexA;

    // simulate scene loading
    for (indexA = 0; indexA < baseline; indexA++) { addElement(list, max++); }

    // simulate game loop
    for (indexA = 0; indexA < Length; indexA++)
    {
        for (int j = 0; j < addAmount; j++) { addElement(list, max++); }

        int count = getCount(list) / 4;

        int indexB;
        for (indexB = 0; indexB < count; indexB++)
        {
            double percent = random.NextDouble();
            int key = percent < constItemRatio ? random.Next(0, staticElements) : random.Next(staticElements + 1, max);

            int value = findElement(list, key);
            DoNothing(value);
        }

        int min = max - 1;
        for (indexB = 0; indexB < removeAmount; indexB++) { removeElement(list, min--); }
    }
}

static void DoNothing(int value) { }


void SystemListBinarySearch()
{
    Operate(
        createList: () => new SystemList(),
        addElement: (list, value) =>
        {
            int index = list.BinarySearch(value);
            if (index < 0) { list.Insert(~index, value); }
        },
        removeElement: (list, value) =>
        {
            int index = list.BinarySearch(value);
            if (index >= 0) { list.RemoveAt(index); }
        },
        findElement: (list, value) =>
        {
            int index = list.BinarySearch(value);
            return index >= 0 ? list[index] : default;
        },
        getCount: (list) => list.Count);
}

SystemListBinarySearch();

internal class IntComparator() : IMatcher<int, int>
{
    public int Key { get; set; }

    public bool Match(int value) { return Key == value; }
}