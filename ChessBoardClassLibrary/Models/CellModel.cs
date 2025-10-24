namespace ChessBoardClassLibrary.Models
{
    /// Cell model for a single square.
    public class CellModel
    {
        public int Row { get; private set; }
        public int Column { get; private set; }
        public string? PieceOccupyingCell { get; set; } = null;
        public bool IsLegalNextMove { get; set; } = false;

        /// Construct with row and column.
        public CellModel(int row, int column)
        {
            Row = row;
            Column = column;
            PieceOccupyingCell = null;
            IsLegalNextMove = false;
        }
    }
}
