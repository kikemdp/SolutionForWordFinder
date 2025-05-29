using BenchmarkDotNet.Running;

namespace WordFinderBenchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<WordFinderBenchmarks>();
        }
    }
}
