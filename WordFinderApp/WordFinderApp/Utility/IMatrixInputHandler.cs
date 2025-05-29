namespace WordFinderApp.Utility
{
    public interface IMatrixInputHandler
    {
        /// <summary>
        /// Get the matrix size
        /// </summary>
        /// <returns></returns>
        (int rows, int cols) GetMatrixSize();
        /// <summary>
        /// Get the matrix
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <returns></returns>
        List<string> GetMatrix(int rows, int cols);
    }
}
