
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

using SystemVector = System.Numerics.Vector3;
using MyVector = Core.Maths.Vectors.Vector<int>;

namespace Benchmarks.Core.Maths
{
    [MemoryDiagnoser]
    [RankColumn]
    [Orderer(summaryOrderPolicy: SummaryOrderPolicy.FastestToSlowest, methodOrderPolicy: MethodOrderPolicy.Alphabetical)]
    public class Vectors3
    {
        [Params(3)]
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
        public void SystemVector()
        {
            SystemVector left = new(LeftValues[0], LeftValues[1], LeftValues[2]);
            SystemVector right = new(RightValues[0], RightValues[1], RightValues[2]);
            SystemVector result = left + right;

            DoNothing(result);
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
