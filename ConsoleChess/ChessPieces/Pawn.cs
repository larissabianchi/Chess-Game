using ChessBoard;
using ConsoleChess;

namespace ChessPieces
{
    internal class Pawn : Piece
    {
        private Match match;

        public Pawn(Color color, Board Board, Match match) : base(color, Board)
        {
            this.match = match;
        }

        public override string ToString()
        {
            return "P";
        }

        protected override bool PieceCanMove(Position position)
        {
            if (Board.IsValidPosition(position))
                return Board.Piece(position) == null;

            return false;
        }

        private bool ThereIsAnEnemy(Position position)
        {
            Piece piece = Board.Piece(position);
            return piece != null && piece.Color != Color;
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] movements = new bool[Board.Lines, Board.Columns];

            Position position = new(0, 0);

            if (Color == Color.White)
            {
                position.DefineValues(Position.Column, Position.Line - 1);
                if (PieceCanMove(position))
                    movements[position.Line, position.Column] = true;

                position.DefineValues(Position.Column, Position.Line - 2);
                Position secondPosition = new(Position.Column, Position.Line - 1);
                if (PieceCanMove(secondPosition) && PieceCanMove(position) && MovimentsCount == 0)
                    movements[position.Line, position.Column] = true;

                position.DefineValues(Position.Column - 1, Position.Line - 1);
                if (Board.IsValidPosition(position) && ThereIsAnEnemy(position))
                    movements[position.Line, position.Column] = true;

                position.DefineValues(Position.Column + 1, Position.Line - 1);
                if (Board.IsValidPosition(position) && ThereIsAnEnemy(position))
                    movements[position.Line, position.Column] = true;

                if (Position.Line == 3) //en passant
                {
                    Position left = new(Position.Column - 1, Position.Line);
                    if (Board.IsValidPosition(left) && ThereIsAnEnemy(left) && Board.Piece(left) == match.VulnerableToEnPassant)
                        movements[left.Line - 1, left.Column] = true;

                    Position right = new(Position.Column + 1, Position.Line);
                    if (Board.IsValidPosition(right) && ThereIsAnEnemy(right) && Board.Piece(right) == match.VulnerableToEnPassant)
                        movements[right.Line - 1, right.Column] = true;
                }
            }
            else
            {
                position.DefineValues(Position.Column, Position.Line + 1);
                if (PieceCanMove(position))
                    movements[position.Line, position.Column] = true;

                position.DefineValues(Position.Column, Position.Line + 2);
                Position secondPosition = new(Position.Column, Position.Line + 1);
                if (PieceCanMove(secondPosition) && PieceCanMove(position) && MovimentsCount == 0)
                    movements[position.Line, position.Column] = true;

                position.DefineValues(Position.Column - 1, Position.Line + 1);
                if (Board.IsValidPosition(position) && ThereIsAnEnemy(position))
                    movements[position.Line, position.Column] = true;

                position.DefineValues(Position.Column + 1, Position.Line + 1);
                if (Board.IsValidPosition(position) && ThereIsAnEnemy(position))
                    movements[position.Line, position.Column] = true;

                if (Position.Line == 4) //en passant
                {
                    Position left = new(Position.Column - 1, Position.Line);
                    if (Board.IsValidPosition(left) && ThereIsAnEnemy(left) && Board.Piece(left) == match.VulnerableToEnPassant)
                        movements[left.Line + 1, left.Column] = true;

                    Position right = new(Position.Column + 1, Position.Line);
                    if (Board.IsValidPosition(right) && ThereIsAnEnemy(right) && Board.Piece(right) == match.VulnerableToEnPassant)
                        movements[right.Line + 1, right.Column] = true;
                }
            }

            return movements;
        }
    }
}
