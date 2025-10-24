using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ChessBoardClassLibrary.Models;
using ChessBoardClassLibrary.Services.BusinessLogicLayer;

namespace ChessBoardGUIApp
{
    // windows form that draws the board with buttons and a theme picker
    public class FrmChessBoard : Form
    {
        private BoardModel _board;
        private BoardLogic _boardLogic;
        private Button[,] _buttons;

        private Panel pnlChessBoard;
        private ComboBox cmbChessPieces;
        private ComboBox cmbTheme;
        private Label lblInstr;
        private Label lblPieces;
        private Label lblTheme;

        // map theme name 
        private readonly Dictionary<string, (Color light, Color dark)> _themes =
            new()
            {
                ["Classic"] = (Color.White, Color.LightGray),
                ["Warm"]    = (Color.MistyRose, Color.Salmon),
                ["Cool"]    = (Color.AliceBlue, Color.SteelBlue),
                ["Pastel"]  = (Color.Honeydew, Color.MediumAquamarine),
                ["Neon"]    = (Color.Black, Color.Lime)
            };

        public FrmChessBoard()
        {
            // form setup
            this.Text = "Chess Board";
            this.Width = 860;   // room for labels
            this.Height = 680;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Dpi;

            // instruction text
            lblInstr = new Label {
                Left = 20,
                Top = 15,
                AutoSize = true,
                Text = "Select a chess piece and its location on the board and see the legal moves."
            };

            // piece label and dropdown
            lblPieces = new Label {
                Left = 20,
                Top = 45,
                AutoSize = true,
                Text = "Pieces:"
            };

            cmbChessPieces = new ComboBox {
                Top = 40,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbChessPieces"
            };
            cmbChessPieces.Left = lblPieces.Left + lblPieces.Width + 10;
            cmbChessPieces.Items.AddRange(new object[] { "King", "Queen", "Bishop", "Knight", "Rook" });
            cmbChessPieces.SelectedIndex = 3; // Knight

            // theme label and dropdown
            lblTheme = new Label {
                Top = 45,
                AutoSize = true,
                Text = "Theme:"
            };
            lblTheme.Left = cmbChessPieces.Left + cmbChessPieces.Width + 30;

            cmbTheme = new ComboBox {
                Top = 40,
                Width = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbTheme"
            };
            cmbTheme.Left = lblTheme.Left + lblTheme.Width + 10;
            cmbTheme.Items.AddRange(new object[] { "Classic", "Warm", "Cool", "Pastel", "Neon" });
            cmbTheme.SelectedIndex = 0; // Classic
            cmbTheme.SelectedIndexChanged += (_, __) => ApplyTheme();

            // panel that holds the board
            pnlChessBoard = new Panel {
                Left = 20,
                Top = 80,
                Width = 500,
                Height = 500,
                BorderStyle = BorderStyle.FixedSingle,
                Name = "pnlChessBoard"
            };

            // add controls
            this.Controls.Add(lblInstr);
            this.Controls.Add(lblPieces);
            this.Controls.Add(cmbChessPieces);
            this.Controls.Add(lblTheme);
            this.Controls.Add(cmbTheme);
            this.Controls.Add(pnlChessBoard);

            // models and logic
            _board = new BoardModel(8);
            _boardLogic = new BoardLogic();
            _buttons = new Button[8, 8];

            SetUpButtons();
            ApplyTheme();
            UpdateButtons();
        }

        // create eight by eight buttons
        private void SetUpButtons()
        {
            int size = Math.Min(pnlChessBoard.Width, pnlChessBoard.Height) / _board.Size;
            pnlChessBoard.Width = pnlChessBoard.Height = size * _board.Size;

            for (int r = 0; r < _board.Size; r++)
            {
                for (int c = 0; c < _board.Size; c++)
                {
                    var btn = new Button
                    {
                        Width = size,
                        Height = size,
                        Left = c * size,
                        Top = r * size,
                        Tag = new Point(c, r),
                        Text = ""
                    };

                    // bigger symbols so screenshots are clear
                    btn.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);

                    btn.Click += BtnSquareClickEH;

                    _buttons[r, c] = btn;
                    pnlChessBoard.Controls.Add(btn);
                }
            }
        }

        // set button colors based on current theme
        private void ApplyTheme()
        {
            var name = cmbTheme.SelectedItem?.ToString() ?? "Classic";
            if (!_themes.TryGetValue(name, out var t))
                t = _themes["Classic"];

            for (int r = 0; r < _board.Size; r++)
            {
                for (int c = 0; c < _board.Size; c++)
                {
                    var btn = _buttons[r, c];
                    bool lightSquare = ((r + c) % 2 == 0);
                    btn.BackColor = lightSquare ? t.light : t.dark;
                    btn.ForeColor = name == "Neon" ? Color.White : Color.Black;
                }
            }
        }

        // copy state from the board into the buttons
        private void UpdateButtons()
        {
            for (int r = 0; r < _board.Size; r++)
            {
                for (int c = 0; c < _board.Size; c++)
                {
                    var cell = _board.Grid[r, c];
                    var btn = _buttons[r, c];

                    if (!string.IsNullOrWhiteSpace(cell.PieceOccupyingCell))
                        btn.Text = cell.PieceOccupyingCell; // K Q B N R
                    else if (cell.IsLegalNextMove)
                        btn.Text = "*"; // star means legal move
                    else
                        btn.Text = "";
                }
            }
        }

        // click a square to place the piece and show legal moves
        private void BtnSquareClickEH(object? sender, EventArgs e)
        {
            if (sender is not Button btn) return;
            if (btn.Tag is not Point p) return;

            // clear any old piece letter
            foreach (var cell in _board.Grid)
            {
                if (cell.PieceOccupyingCell is "N" or "K" or "Q" or "B" or "R")
                    cell.PieceOccupyingCell = null;
            }

            var current = _board.Grid[p.Y, p.X];
            var piece = cmbChessPieces.SelectedItem?.ToString() ?? "Knight";

            _board = _boardLogic.MarkLegalMoves(_board, current, piece);
            UpdateButtons();
        }
    }
}
