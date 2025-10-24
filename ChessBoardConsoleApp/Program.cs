using ChessBoardClassLibrary.Models;
using ChessBoardClassLibrary.Services.BusinessLogicLayer;
using ChessBoardConsoleApp;  // this makes Utility visible

// main console app
Console.WriteLine("Chessboard Legal Moves in Console");

// create an eight by eight board and show it
var board = new BoardModel(8);
Utility.PrintBoardPretty(board);

// get the piece from the user
Console.Write("Enter piece King Queen Bishop Knight Rook: ");
var piece = Console.ReadLine() ?? "Knight";

// get a safe row and col from the user
var pos = Utility.GetRowAndColValidated(board.Size);

// mark moves
var logic = new BoardLogic();
board = logic.MarkLegalMoves(board, board.Grid[pos.row, pos.col], piece);

// show the board again with stars
Utility.PrintBoardPretty(board);
