namespace ChessBoardClassLibrary.Models
{
    /// Board model: Size and Grid of CellModel.
    public class BoardModel
    {
        public int Size { get; private set; }
        public CellModel[,] Grid { get; private set; }

        /// Create an board of cells.
        public BoardModel(int size)
        {
            Size = size;
            Grid = new CellModel[size, size];
            InitializeBoard();
        }

        /// Fill Grid with CellModel instances.
        private void InitializeBoard()
        {
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    Grid[r, c] = new CellModel(r, c);
                }
            }
        }
    }
}
