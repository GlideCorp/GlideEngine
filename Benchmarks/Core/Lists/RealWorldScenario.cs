
using SystemLinkedList = System.Collections.Generic.LinkedList<int>;
using SystemList = System.Collections.Generic.List<int>;
using SystemSortedList = System.Collections.Generic.SortedList<int, int>;
using SystemDictionary = System.Collections.Generic.Dictionary<int, int>;
using MySinglyLinkedList = Core.Collections.Lists.SinglyLinkedList<int, int>;
using MyDoublyLinkedList = Core.Collections.Lists.DoublyLinkedList<int, int>;
using MyChunkLinkedList = Core.Collections.Lists.ChunkLinkedList<int, int>;
using MyFrequentLinkedList = Core.Collections.Lists.FrequentLinkedList<int, int>;
using MyList = Core.Collections.Lists.List<int, int>;

using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using BenchmarkDotNet.Order;
using Core.Collections;
using Core.Collections.Lists;

/*
   | Method                 | Length | Mean       | Error     | StdDev    | Rank | Gen0    | Allocated |
   |----------------------- |------- |-----------:|----------:|----------:|-----:|--------:|----------:|
   | SystemDictionary       | 1000   |   6.681 ms | 0.1296 ms | 0.1386 ms |    1 | 15.6250 | 150.89 KB |
   | SystemListBinarySearch | 1000   |  12.398 ms | 0.0760 ms | 0.0635 ms |    2 |       - |  32.58 KB |
   | MyChunkLinkedListX256  | 1000   |  23.845 ms | 0.1969 ms | 0.1644 ms |    3 |       - |   70.1 KB |
   | MyChunkLinkedListX128  | 1000   |  27.012 ms | 0.2577 ms | 0.2152 ms |    4 |       - |   74.1 KB |
   | MyChunkLinkedListX64   | 1000   |  30.096 ms | 0.4672 ms | 0.4370 ms |    5 |       - |  82.85 KB |
   | MyChunkLinkedListX32   | 1000   |  36.310 ms | 0.1706 ms | 0.1512 ms |    6 |       - |  99.37 KB |
   | MyFrequentLinkedList   | 1000   |  70.243 ms | 1.3910 ms | 2.4362 ms |    7 |       - | 470.87 KB |
   | MyDoublyLinkedList     | 1000   | 116.310 ms | 1.3901 ms | 1.1608 ms |    8 |       - | 588.47 KB |
   | MyChunkLinkedListX16   | 1000   | 120.611 ms | 2.2326 ms | 4.4070 ms |    8 |       - | 133.94 KB |
 */

namespace Benchmarks.Core.Lists
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest, methodOrderPolicy: MethodOrderPolicy.Alphabetical)]
    public class RealWorldScenario
    {
        [Params(5_000)] public int Length { get; set; }

        private void Operate<TList>(
            Func<TList> createList,
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

        private static void DoNothing(int value) { }

        private class IntMatcher : IMatcher<int, int>
        {
            public int Key { get; set; }

            public bool Match(int value)
            {
                return Key == value;
            }
        }

        /*
        [Benchmark]
        public void SystemLinkedList()
        {
            Operate(
                createList: () => new LinkedList<int>(),
                addElement: (list, value) => { list.AddLast(value); },
                removeElement: (list, value) => { list.Remove(value); },
                findElement: (list, value) => list.Find(value) == null ? value : default,
                getCount: list => list.Count);
        }
        */

        [Benchmark]
        public void SystemList()
        {
            Operate(
                createList: () => new SystemList(),
                addElement: (list, value) => { list.Add(value); },
                removeElement: (list, value) =>
                {
                    int index = list.IndexOf(value);
                    list[index] = list[^1];
                    list.RemoveAt(list.Count - 1);
                },
                findElement: (list, value) => { return list.Find(x => x == value); },
                getCount: (list) => list.Count);
        }

        /*
        [Benchmark]
        public void SystemListBinarySearch()
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

        [Benchmark]
        public void SystemSortedList()
        {
            Operate(
                createList: () => new SystemSortedList(),
                addElement: (list, value) => { list.TryAdd(value, value); },
                removeElement: (list, value) => { list.Remove(value); },
                findElement: (list, value) => list.GetValueOrDefault(value),
                getCount: (list) => list.Count);
        }

        [Benchmark]
        public void SystemDictionary()
        {
            Operate(
                createList: () => new SystemDictionary(),
                addElement: (list, value) => { list.TryAdd(value, value); },
                removeElement: (list, value) => { list.Remove(value); },
                findElement: (list, value) => list.GetValueOrDefault(value),
                getCount: (list) => list.Count);
        }
     */
        [Benchmark]
        public void MySinglyLinkedList()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new MySinglyLinkedList(matcher),
                addElement: (list, key) => { list.InsertLast(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }
        /*
        [Benchmark]
        public void MyFrequentLinkedList()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new FrequentLinkedList<int, int>(matcher),
                addElement: (list, key) => { list.InsertLast(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }

        [Benchmark]
        public void MyDoublyLinkedList()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new DoublyLinkedList<int, int>(matcher),
                addElement: (list, key) => { list.InsertLast(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }

        [Benchmark]
        public void MyChunkLinkedListX32()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new ChunkLinkedList<int, int>(size: 32, matcher),
                addElement: (list, key) => { list.Insert(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }

        [Benchmark]
        public void MyChunkLinkedListX128()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new ChunkLinkedList<int, int>(size: 128, matcher),
                addElement: (list, key) => { list.Insert(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }
        
        [Benchmark]
        public void MyChunkLinkedListX512()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new ChunkLinkedList<int, int>(size: 512, matcher),
                addElement: (list, key) => { list.Insert(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }
        */

        [Benchmark]
        public void MyList()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new MyList(matcher),
                addElement: (list, key) => { list.InsertLast(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }

    }
}

