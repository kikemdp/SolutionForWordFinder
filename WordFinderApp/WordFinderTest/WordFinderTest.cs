using WordFinderApp.Core;
using WordFinderApp.Utility;

namespace WordFinderTest
{
    public class WordFinderTest
    {
        [Fact]
        public void FindOptimized_ShouldHandleLargeWordStream_EfficientlyAndCorrectly()
        {
            // Arrange
            var matrix = new List<string> { "abcd", "efgh", "ijkl", "mnop", "abcd" };
            var matrixWords = MatrixWordExtractor.ExtractWords(matrix);
            var wordFinder = new WordFinder(matrixWords);

            var random = new Random();
            var wordstream = Enumerable.Range(0, 10000000)
                .Select(_ => GenerateRandomString(4, random))
                .Concat(new[] { "abcd", "mnop" }); // These words appear in the matrix

            // Act
            var result = wordFinder.Find(wordstream).ToList();

            // Assert
            Assert.Contains("abcd", result); // Should be first as it appears twice in matrix
            Assert.Contains("mnop", result); // Should be second as it appears once
            Assert.True(result.IndexOf("abcd") < result.IndexOf("mnop")); // Verify order
        }
        [Fact]
        public void Find_ShouldReturnEmpty_WhenNoMatchingWords()
        {
            // Arrange
            var matrix = new List<string> { "abcd", "efgh", "ijkl", "mnop" };
            var matrixWords = MatrixWordExtractor.ExtractWords(matrix);
            var wordFinder = new WordFinder(matrixWords);
            var wordstream = new List<string> { "qrst", "uvwx", "yz" };

            // Act
            var result = wordFinder.Find(wordstream);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Find_ShouldHandleLargeWordStream_Efficiently()
        {
            // Arrange
            var matrix = new List<string> { "abcd", "efgh", "ijkl", "mnop" };
            var matrixWords = MatrixWordExtractor.ExtractWords(matrix);
            var wordFinder = new WordFinder(matrixWords);

            var random = new Random();
            var wordstream = Enumerable.Range(0, 10000000)
                .Select(_ => GenerateRandomString(4, random)) // Generates a random string each time
                .Concat(new[] { "mnop" });


            // Act
            var result = wordFinder.Find(wordstream);

            // Assert
            Assert.Contains("mnop", result);
        }

        [Fact]
        public void Findparallel_ShouldHandleLargeWordStream_Efficiently()
        {
            // Arrange
            var matrix = new List<string> { "abcd", "efgh", "ijkl", "mnop" };
            var matrixWords = MatrixWordExtractor.ExtractWords(matrix);
            var wordFinder = new WordFinder(matrixWords);

            var random = new Random();
            var wordstream = Enumerable.Range(0, 10000000)
                .Select(_ => GenerateRandomString(4, random)) // Generates a random string each time
                .Concat(new[] { "mnop" });

            // Act
            var result = wordFinder.FindParallel(wordstream);

            // Assert
            Assert.Contains("mnop", result);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenMatrixIsNull()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new WordFinder(null!));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenMatrixContainsNullRow()
        {
            // Arrange
            var matrix = new List<string> { "abc", null! };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => new WordFinder(matrix));
            Assert.Contains("cannot contain null values", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Constructor_ShouldNotThrow_WhenMatrixIsValid()
        {
            // Arrange
            var matrix = new List<string> { "abc", "def" };

            // Act & Assert
            var finder = new WordFinder(matrix);
            Assert.NotNull(finder);
        }

        private string GenerateRandomString(int length, Random random)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }

        [Fact]
        public void Find_ShouldRespectWordCountInTop10()
        {
            // Arrange
            var matrix = new List<string> { "mnops", "qrsts", "mnops", "mnops", "qrsts" };
            var matrixWords = MatrixWordExtractor.ExtractWords(matrix);
            var wordFinder = new WordFinder(matrixWords);

            var wordstream = new List<string>
            {
                "mnop", "efgh", "mnop", "qrst", "uvwx", "yz12", "abcd"
            };

            var expected = new List<string> { "mnop", "qrst" };

            // Act
            var result = wordFinder.Find(wordstream).ToList();

            // Assert
            Assert.Equal(expected, result);
        }

    }
}