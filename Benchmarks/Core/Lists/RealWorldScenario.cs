
using SystemLinkedList = System.Collections.Generic.LinkedList<int>;
using SystemList = System.Collections.Generic.List<int>;
using SystemSortedList = System.Collections.Generic.SortedList<int, int>;
using SystemDictionary = System.Collections.Generic.Dictionary<int, int>;
using MySinglyLinkedList = Core.Collections.Lists.SinglyLinkedList<int, int>;
using MyDoublyLinkedList = Core.Collections.Lists.DoublyLinkedList<int, int>;
using MyChunkLinkedList = Core.Collections.Lists.ChunkList<int, int>;
using MyFrequentLinkedList = Core.Collections.Lists.FrequentList<int, int>;

using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using Core.Collections.Nodes;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Order;

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
        [Params(1000)]
        public int Length { get; set; }

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

        private static void DoNothing(int value) { }

        private class IntComparator : IMatcher<int, int>
        {
            public int Key { get; set; }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Match(int value) { return Key == value; }
        }

        //[Benchmark]
        public void SystemLinkedList()
        {
            Operate(
                createList: () => new SystemLinkedList(),
                addElement: (list, value) => { list.AddFirst(value); },
                removeElement: (list, value) => { list.Remove(value); },
                findElement: (list, value) =>
                {
                    LinkedListNode<int>? node = list.Find(value);
                    return node?.Value ?? default;
                },
                getCount: (list) => list.Count);
        }

        //[Benchmark]
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

        //[Benchmark]
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

        //[Benchmark(Baseline = true)]
        public void MySinglyLinkedList()
        {
            IntComparator intComparator = new();
            Operate(
                createList: () => new MySinglyLinkedList(),
                addElement: (list, value) => { list.Insert(value); },
                removeElement: (list, value) =>
                {
                    intComparator.Key = value;
                    list.Remove(intComparator);
                },
                findElement: (list, value) =>
                {
                    intComparator.Key = value;
                    return list.Find(intComparator, out int v) ? v : default;
                },
                getCount: (list) => list.Count);
        }

        [Benchmark]
        public void MyDoublyLinkedList()
        {
            IntComparator intComparator = new();
            Operate(
                createList: () => new MyDoublyLinkedList(),
                addElement: (list, value) => { list.Insert(value); },
                removeElement: (list, value) =>
                {
                    intComparator.Key = value;
                    list.Remove(intComparator);
                },
                findElement: (list, value) =>
                {
                    intComparator.Key = value;
                    return list.Find(intComparator, out int v) ? v : default;
                },
                getCount: (list) => list.Count);
        }

        [Benchmark]
        public void MyChunkLinkedListX16()
        {
            IntComparator intComparator = new();
            Operate(
                createList: () => new MyChunkLinkedList(16),
                addElement: (list, value) => { list.Insert(value); },
                removeElement: (list, value) =>
                {
                    intComparator.Key = value;
                    list.Remove(intComparator);
                },
                findElement: (list, value) =>
                {
                    intComparator.Key = value;
                    return list.Find(intComparator, out int v) ? v : default;
                },
                getCount: (list) => list.Count);
        }

        [Benchmark]
        public void MyChunkLinkedListX32()
        {
            IntComparator intComparator = new();
            Operate(
               createList: () => new MyChunkLinkedList(32),
               addElement: (list, value) => { list.Insert(value); },
               removeElement: (list, value) =>
               {
                   intComparator.Key = value;
                   list.Remove(intComparator);
               },
               findElement: (list, value) =>
               {
                   intComparator.Key = value;
                   return list.Find(intComparator, out int v) ? v : default;
               },
               getCount: (list) => list.Count);
        }

        [Benchmark]
        public void MyChunkLinkedListX64()
        {
            IntComparator intComparator = new();
            Operate(
               createList: () => new MyChunkLinkedList(64),
               addElement: (list, value) => { list.Insert(value); },
               removeElement: (list, value) =>
               {
                   intComparator.Key = value;
                   list.Remove(intComparator);
               },
               findElement: (list, value) =>
               {
                   intComparator.Key = value;
                   return list.Find(intComparator, out int v) ? v : default;
               },
               getCount: (list) => list.Count);
        }

        [Benchmark]
        public void MyChunkLinkedListX128()
        {
            IntComparator intComparator = new();
            Operate(
               createList: () => new MyChunkLinkedList(128),
               addElement: (list, value) => { list.Insert(value); },
               removeElement: (list, value) =>
               {
                   intComparator.Key = value;
                   list.Remove(intComparator);
               },
               findElement: (list, value) =>
               {
                   intComparator.Key = value;
                   return list.Find(intComparator, out int v) ? v : default;
               },
               getCount: (list) => list.Count);
        }

        [Benchmark]
        public void MyChunkLinkedListX256()
        {
            IntComparator intComparator = new();
            Operate(
               createList: () => new MyChunkLinkedList(256),
               addElement: (list, value) => { list.Insert(value); },
               removeElement: (list, value) =>
               {
                   intComparator.Key = value;
                   list.Remove(intComparator);
               },
               findElement: (list, value) =>
               {
                   intComparator.Key = value;
                   return list.Find(intComparator, out int v) ? v : default;
               },
               getCount: (list) => list.Count);
        }

        [Benchmark]
        public void MyFrequentLinkedList()
        {
            IntComparator intComparator = new();
            Operate(
               createList: () => new MyFrequentLinkedList(),
               addElement: (list, value) => { list.Insert(value); },
               removeElement: (list, value) =>
               {
                   intComparator.Key = value;
                   list.Remove(intComparator);
               },
               findElement: (list, value) =>
               {
                   intComparator.Key = value;
                   return list.Find(intComparator, out int v) ? v : default;
               },
               getCount: (list) => list.Count);
        }
    }
}

