

using SystemSortedList = System.Collections.Generic.SortedList<int, int>;
using SystemDictionary = System.Collections.Generic.Dictionary<int, int>;

using MyCachedList = Core.Collections.Lists.CachedList<int, int>;

using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using BenchmarkDotNet.Order;
using Core.Collections;
using Core.Collections.Lists;

/*
| Method                 | Cycles | Mean        | Error     | StdDev    | Rank | Gen0        | Allocated     |
   |----------------------- |------- |------------:|----------:|----------:|-----:|------------:|--------------:|
   | SystemDictionary       | 10000  |    277.7 ms |   5.48 ms |  11.07 ms |    1 |           - |     657.78 KB |
   | SystemListBinarySearch | 10000  |    547.0 ms |   3.85 ms |   3.60 ms |    2 |           - |     129.01 KB |
   | SystemSortedList       | 10000  |    574.5 ms |   3.64 ms |   3.41 ms |    3 |           - |     257.33 KB |
   | MyCachedListX32        | 10000  |  2,518.3 ms |  15.00 ms |  11.71 ms |    4 |           - |     129.32 KB |
   | MyCachedListX8         | 10000  |  2,557.9 ms |  17.07 ms |  13.32 ms |    4 |           - |     129.23 KB |
   | MyCachedListX128       | 10000  |  2,841.1 ms |  12.66 ms |  10.57 ms |    5 |           - |      129.7 KB |
   | MyList                 | 10000  |  5,920.3 ms |  23.21 ms |  21.71 ms |    6 |           - |     129.16 KB |
   | MyLinkedChunkListX512  | 10000  |  6,052.9 ms |  37.88 ms |  35.44 ms |    6 |           - |     592.95 KB |
   | MyLinkedChunkListX128  | 10000  |  6,149.0 ms |  45.12 ms |  42.21 ms |    6 |           - |     668.68 KB |
   | MyLinkedChunkListX32   | 10000  |  6,929.1 ms |  23.10 ms |  21.60 ms |    7 |           - |     918.23 KB |
   | SystemList             | 10000  |  6,951.8 ms |  63.77 ms |  59.65 ms |    7 | 133000.0000 | 1087882.91 KB |
   | MySinglyLinkedList     | 10000  | 34,262.4 ms | 321.35 ms | 300.59 ms |    8 |           - |    4689.91 KB |
   | MyFrequentLinkedList   | 10000  | 35,574.1 ms | 255.57 ms | 239.06 ms |    9 |           - |    4689.91 KB |
   | MyDoublyLinkedList     | 10000  | 35,876.7 ms | 224.24 ms | 209.76 ms |    9 |           - |    5862.17 KB |
   | SystemLinkedList       | 10000  | 38,725.1 ms | 304.67 ms | 284.99 ms |   10 |           - |    7034.32 KB |
 */

namespace Benchmarks.Core.Lists
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest, methodOrderPolicy: MethodOrderPolicy.Alphabetical)]
    public class RealWorldScenario
    {
        [Params(5_000)] public int Cycles { get; set; }
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

        [Benchmark]
        public void MyCachedListX8()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new MyCachedList(size: 8, matcher),
                addElement: (list, key) => { list.InsertLast(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }

        [Benchmark]
        public void MyCachedListX32()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new MyCachedList(size: 32, matcher),
                addElement: (list, key) => { list.InsertLast(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }

        [Benchmark]
        public void MyCachedListX128()
        {
            IntMatcher matcher = new();
            Operate(
                createList: () => new MyCachedList(size: 128, matcher),
                addElement: (list, key) => { list.InsertLast(key); },
                removeElement: (list, key) => { list.Remove(key); },
                findElement: (list, key) => list.Find(key, out int value) ? value : default,
                getCount: list => list.Count);
        }
        */
    }
}

