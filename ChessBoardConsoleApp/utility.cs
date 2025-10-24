using System;
using ChessBoardClassLibrary.Models;

namespace ChessBoardConsoleApp
{
    // helper methods for the console app
    public static class Utility
    {
        // choose one char to show in a cell
        // piece letter wins
        // then star for legal move
        // else space
        private static char CellSymbol(CellModel cell)
        {
            if (!string.IsNullOrWhiteSpace(cell.PieceOccupyingCell))
                return cell.PieceOccupyingCell![0]; // K Q B N R

            if (cell.IsLegalNextMove)
                return '*';

            return ' ';
        }

        // draw a nice board with headers and grid lines
        public static void PrintBoardPretty(BoardModel board)
        {
            int n = board.Size;

            // column headers
            Console.Write("    ");
            for (int c = 0; c < n; c++)
                Console.Write($"  {c} ");
            Console.WriteLine();

            // top border
            Console.Write("    ");
            for (int c = 0; c < n; c++)
                Console.Write("+---");
            Console.WriteLine("+");

            // rows
            for (int r = 0; r < n; r++)
            {
                Console.Write($"{r,2}  ");
                for (int c = 0; c < n; c++)
                {
                    char ch = CellSymbol(board.Grid[r, c]);
                    Console.Write($"| {ch} ");
                }
                Console.WriteLine("|");

                Console.Write("    ");
                for (int c = 0; c < n; c++)
                    Console.Write("+---");
                Console.WriteLine("+");
            }

            Console.WriteLine();
        }

        // loop until the user gives a safe row and col
        public static (int row, int col) GetRowAndColValidated(int size)
        {
            int row = ReadIntInRange($"Enter row 0 to {size - 1}: ", 0, size - 1);
            int col = ReadIntInRange($"Enter col 0 to {size - 1}: ", 0, size - 1);
            return (row, col);
        }

        // keep asking until a number in range is given
        private static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                var s = Console.ReadLine();
                if (int.TryParse(s, out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Please enter a whole number from {min} to {max}.");
            }
        }
    }
}
