using BenchmarkDotNet.Attributes;
using WordFinderApp.Core;
using WordFinderApp.Utility;

namespace WordFinderBenchmarks
{
    [MemoryDiagnoser]               // track allocations
    public class WordFinderBenchmarks
    {
        private List<string> _matrix;
        private IEnumerable<string> _matrixWords;
        private List<string> _smallStream;
        private List<string> _largeStream;

        [Params(64)]         // matrix sizes to test
        public int Size { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            // Generate a random square matrix of letters
            var rand = new Random(123);
            _matrix = new List<string>();
            for (int i = 0; i < Size; i++)
            {
                var row = new char[Size];
                for (int j = 0; j < Size; j++)
                    row[j] = (char)('A' + rand.Next(26));
                _matrix.Add(new string(row));
            }

            // Extract sequences once
            _matrixWords = MatrixWordExtractor.ExtractWords(_matrix);

            // Small vs. large word streams
            _smallStream = GenerateRandomWords(10, 5, rand);
            _largeStream = GenerateRandomWords(100000, 5, rand);
        }

        private List<string> GenerateRandomWords(int count, int length, Random rand)
        {
            var list = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var chars = new char[length];
                for (int j = 0; j < length; j++)
                    chars[j] = (char)('A' + rand.Next(26));
                list.Add(new string(chars));
            }
            return list;
        }

        [Benchmark(Baseline = true)]
        public void FindOriginal_SmallStream()
        {
            var finder = new WordFinder(_matrixWords);
            finder.FindOriginal(_smallStream);
        }

        [Benchmark]
        public void Find_SmallStream()
        {
            var finder = new WordFinder(_matrixWords);
            finder.Find(_smallStream);
        }
        [Benchmark]
        public void FindOriginal_LargeStream()
        {
            var finder = new WordFinder(_matrixWords);
            finder.FindOriginal(_largeStream);
        }

        [Benchmark]
        public void Find_LargeStream()
        {
            var finder = new WordFinder(_matrixWords);
            finder.Find(_largeStream);
        }

        [Benchmark]
        public void Find_LargeStreamParallel()
        {
            var finder = new WordFinder(_matrixWords);
            finder.FindParallel(_largeStream);
        }
    }
}