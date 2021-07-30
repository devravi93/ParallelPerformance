using BenchmarkDotNet.Running;
using System;

namespace ParallelPerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var summary = BenchmarkRunner.Run<ParallelOperations>();
            var summary = BenchmarkRunner.Run<ApiParallelOperations>();
        }
    }
}
