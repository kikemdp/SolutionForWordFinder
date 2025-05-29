using WordFinderApp.UI;

namespace WordFinderApp.Utility
{
    public class MatrixInputHandler : IMatrixInputHandler
    {
        private readonly IUserInterface _ui;

        public MatrixInputHandler(IUserInterface ui)
        {
            _ui = ui;
        }

        public (int rows, int cols) GetMatrixSize()
        {
            while (true)
            {
                _ui.ShowMessage("Enter the size of the matrix (rows and columns, separated by a space):");
                var sizeInput = _ui.GetInput();
                
                if (IsValidSize(sizeInput, out int rows, out int cols))
                {
                    return (rows, cols);
                }

                _ui.ShowMessage("Invalid matrix size. It must be a maximum of 64x64.");
                _ui.ShowMessage("Please enter a valid size.");
            }
        }

        public List<string> GetMatrix(int rows, int cols)
        {
            var matrix = new List<string>();

            for (int i = 0; i < rows; i++)
            {
                string row;
                do
                {
                    _ui.ShowMessage($"Please enter row {i + 1}:");
                    row = _ui.GetInput();
                    if (string.IsNullOrEmpty(row) || row.Length != cols)
                    {
                        _ui.ShowMessage($"Invalid row. It must contain exactly {cols} characters.");
                    }
                } while (string.IsNullOrEmpty(row) || row.Length != cols);

                matrix.Add(row);
            }

            return matrix;
        }

        private static bool IsValidSize(string sizeInput, out int rows, out int cols)
        {
            rows = 0;
            cols = 0;

            if (string.IsNullOrEmpty(sizeInput)) return false;

            var parts = sizeInput.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !int.TryParse(parts[0], out rows) || !int.TryParse(parts[1], out cols)) return false;

            return rows > 0 && rows <= 64 && cols > 0 && cols <= 64;
        }
    }
}
