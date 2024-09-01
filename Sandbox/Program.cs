
using Core.Collections;
using Core.Maths.Matrices;
using Core.Maths.Vectors;

using MyCachedList = Core.Collections.Lists.CachedList<int, int>;

const int Length = 5000;

void Operate<TList>(
    Func < TList > createList,
    Action<TList, int> addElement,
    Action<TList, int> removeElement,
    Func<TList, int, int> findElement,
    Func<TList, int> getCount)
{
    const int baseline = 50;
    const int staticElements = 30;
    const int addAmount = 15;
    const int removeAmount = addAmount - 1;
    const double constItemRatio = 95 / 100d;

    Random random = new(69);
    TList list = createList();

    // simulate scene loading
    for (int indexA = 0; indexA < baseline; indexA++) { addElement(list, indexA); }

    // simulate game loop
    for (int indexA = 0; indexA < Length; indexA++)
    {
        for (int indexB = 0; indexB < addAmount; indexB++)
        {
            addElement(list, baseline + addAmount * indexA + indexB);
        }

        int count = getCount(list);

        for (int indexB = 0; indexB < count / 4; indexB++)
        {
            double percent = random.NextDouble();
            int key = percent < constItemRatio
                ? random.Next(0, staticElements)
                : random.Next(staticElements + 1, count);

            int value = findElement(list, key);
            DoNothing(value);
        }

        for (int indexB = 0; indexB < removeAmount; indexB++)
        {
            removeElement(list, baseline + addAmount * indexA + indexB);
        }
    }
}

static void DoNothing(int value) { }

void MyCachedListX512()
{
    IntMatcher matcher = new();
    Operate(
        createList: () => new MyCachedList(size: 512, matcher),
        addElement: (list, key) => { list.InsertLast(key); },
        removeElement: (list, key) => { list.Remove(key); },
        findElement: (list, key) => list.Find(key, out int value) ? value : default,
        getCount: list => list.Count);
}

MyCachedListX512();

class IntMatcher : IMatcher<int, int>
{
    public int Key { get; set; }

    public bool Match(int value)
    {
        Matrix4x4<float> asd = new Matrix4x4<float>();
        Matrix4x4<float> asd2 = new Matrix4x4<float>();

        Matrix4x4<float> res = asd + asd2;

        return Key == value;
    }


}
/*
void SLLBase()
{
    Operate(
        createList: () => new SLLBase<int>(),
        addElement: (list, value) => { list.Insert(value); },
        removeElement: (list, value) => { list.Remove(value); },
        findElement: (list, value) => list.Find(value) ? value : default,
        getCount: list => list.Count);
}

void SLLCircular()
{
    Operate(
        createList: () => new SLLCircular<int>(),
        addElement: (list, value) => { list.Insert(value); },
        removeElement: (list, value) => { list.Remove(value); },
        findElement: (list, value) => list.Find(value) ? value : default,
        getCount: list => list.Count);
}

//SLLBase();
SLLCircular();
*/