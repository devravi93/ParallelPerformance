using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParallelPerformance
{
    public class ParallelOperations
    {
        [Benchmark]
        public int[] NormalFor()
        {
            var arr = new int[1_000_000];

            for (int i = 0; i < 1_000_000; i++)
            {
                arr[i] = i;
            }

            return arr;
        }

        [Benchmark]
        public int[] ParallelForEach()
        {
            var arr = new int[1_000_000];

            Parallel.For(0, 1_000_000, i =>
            {
                arr[i] = i;
            });

            return arr;
        }

    }
}
