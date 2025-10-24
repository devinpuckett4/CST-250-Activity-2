using ChessBoardClassLibrary.Models;

namespace ChessBoardClassLibrary.Services.BusinessLogicLayer
{
    // Handles the rules for marking legal moves on the board
    public class BoardLogic
    {
        public BoardModel ResetBoard(BoardModel board)
        {
            // Clear any previous legal move marks
            foreach (var cell in board.Grid)
                cell.IsLegalNextMove = false;

            return board;
        }

        // Checks that a row and column are inside the board
        private static bool IsOnBoard(BoardModel board, int r, int c)
            => r >= 0 && r < board.Size && c >= 0 && c < board.Size;

        public BoardModel MarkLegalMoves(BoardModel board, CellModel current, string piece)
        {
            // Start fresh before marking new moves
            ResetBoard(board);

            // Route to the right rule set based on the piece
            switch (piece.ToLower())
            {
                case "knight":
                case "n":
                    MarkValidKnightMoves(board, current);
                    current.PieceOccupyingCell = "N";
                    break;

                case "rook":
                case "r":
                    MarkValidRookMoves(board, current);
                    current.PieceOccupyingCell = "R";
                    break;

                case "bishop":
                case "b":
                    MarkValidBishopMoves(board, current);
                    current.PieceOccupyingCell = "B";
                    break;

                case "queen":
                case "q":
                    // Queen combines rook and bishop moves
                    MarkValidQueenMoves(board, current);
                    current.PieceOccupyingCell = "Q";
                    break;

                case "king":
                case "k":
                    MarkValidKingMoves(board, current);
                    current.PieceOccupyingCell = "K";
                    break;

                default:
                    // Unknown piece
                    current.PieceOccupyingCell = "?";
                    break;
            }

            return board;
        }

        // Knight moves in L shapes
        private void MarkValidKnightMoves(BoardModel board, CellModel current)
        {
            int[] rOff = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] cOff = { -1, 1, -2, 2, -2, 2, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int nr = current.Row + rOff[i];
                int nc = current.Column + cOff[i];

                // Only mark if the target is on the board
                if (IsOnBoard(board, nr, nc))
                    board.Grid[nr, nc].IsLegalNextMove = true;
            }
        }

        // Rook moves straight along rows and columns
        private void MarkValidRookMoves(BoardModel board, CellModel current)
        {
            // Up
            for (int r = current.Row - 1; r >= 0; r--)
                board.Grid[r, current.Column].IsLegalNextMove = true;

            // Down
            for (int r = current.Row + 1; r < board.Size; r++)
                board.Grid[r, current.Column].IsLegalNextMove = true;

            // Left
            for (int c = current.Column - 1; c >= 0; c--)
                board.Grid[current.Row, c].IsLegalNextMove = true;

            // Right
            for (int c = current.Column + 1; c < board.Size; c++)
                board.Grid[current.Row, c].IsLegalNextMove = true;
        }

        // Bishop moves along diagonals
        private void MarkValidBishopMoves(BoardModel board, CellModel current)
        {
            // Up left
            for (int r = current.Row - 1, c = current.Column - 1; r >= 0 && c >= 0; r--, c--)
                board.Grid[r, c].IsLegalNextMove = true;

            // Up right
            for (int r = current.Row - 1, c = current.Column + 1; r >= 0 && c < board.Size; r--, c++)
                board.Grid[r, c].IsLegalNextMove = true;

            // Down left
            for (int r = current.Row + 1, c = current.Column - 1; r < board.Size && c >= 0; r++, c--)
                board.Grid[r, c].IsLegalNextMove = true;

            // Down right
            for (int r = current.Row + 1, c = current.Column + 1; r < board.Size && c < board.Size; r++, c++)
                board.Grid[r, c].IsLegalNextMove = true;
        }

        // Queen uses both rook and bishop patterns
        private void MarkValidQueenMoves(BoardModel board, CellModel current)
        {
            MarkValidRookMoves(board, current);
            MarkValidBishopMoves(board, current);
        }

        // King moves one square in any direction
        private void MarkValidKingMoves(BoardModel board, CellModel current)
        {
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    // Skip the current square
                    if (dr == 0 && dc == 0) continue;

                    int nr = current.Row + dr;
                    int nc = current.Column + dc;

                    // Only mark if the target is on the board
                    if (IsOnBoard(board, nr, nc))
                        board.Grid[nr, nc].IsLegalNextMove = true;
                }
            }
        }
    }
}