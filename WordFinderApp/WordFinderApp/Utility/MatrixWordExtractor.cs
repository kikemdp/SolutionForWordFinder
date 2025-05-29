namespace WordFinderApp.Utility
{
    // Utility class for extracting words from the matrix
    public static class MatrixWordExtractor
    {
        /// <summary>
        /// Extracts words from the matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static List<string> ExtractWords(IEnumerable<string> matrix)
        {
            if (matrix == null) throw new ArgumentNullException(nameof(matrix));
            var rows = matrix.ToArray();
            if (rows.Any(r => r == null))
                throw new ArgumentException("Matrix rows cannot contain null values.");
            int rowCount = rows.Length;
            if (rowCount == 0) return new List<string>();

            int colCount = rows[0].Length;
            var words = new List<string>(rowCount + colCount);
            words.AddRange(rows);
            for (int c = 0; c < colCount; c++)
            {
                var buf = new char[rowCount];
                for (int r = 0; r < rowCount; r++)
                    buf[r] = rows[r][c];
                words.Add(new string(buf));
            }
            return words;
        }
    }
}
