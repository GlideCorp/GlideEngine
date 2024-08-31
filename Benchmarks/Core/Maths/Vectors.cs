
using MyVector = Core.Maths.Vectors.Vector<int>;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Benchmarks.Core.Maths
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest, methodOrderPolicy: MethodOrderPolicy.Alphabetical)]
    public class Vectors
    {
        [Params(10, 100, 1000, 10000, 100_000)]
        public int Length { get; set; }

        public int[] LeftValues { get; set; }
        public int[] RightValues { get; set; }

        private void DoNothing<T>(T value) { }

        [GlobalSetup]
        public void Setup()
        {
            LeftValues = new int[Length];
            for (int i = 0; i < Length; i++) { LeftValues[i] = i + 1; }

            RightValues = new int[Length];
            for (int i = 0; i < Length; i++) { RightValues[i] = Length - i; }
        }

        [Benchmark]
        public void MyVector()
        {
            MyVector left = new(LeftValues);
            MyVector right = new(RightValues);
            MyVector result = left + right;

            DoNothing(result);
        }
    }
}
