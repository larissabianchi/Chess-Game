using ChessBoard;
using ConsoleChess;

namespace ChessPieces
{
    internal class King : Piece
    {
        private Match match;
        public King(Color color, Board board, Match match) : base(color, board)
        {
            this.match = match;
        }

        private bool RockCanPerformCastling(Position position)
        {
            Piece piece = Board.Piece(position);
            return piece != null && piece is Rock && piece.Color == Color && piece.MovimentsCount == 0;
        }

        public override string ToString()
        {
            return "K";
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] movements = new bool[Board.Lines, Board.Columns];

            Position position = new(0, 0);

            position.DefineValues(Position.Column, Position.Line - 1); //north
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column + 1, Position.Line - 1); //north-east
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column + 1, Position.Line); //east
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column + 1, Position.Line + 1); //south-east
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column, Position.Line + 1); //south
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column - 1, Position.Line + 1); //south-west
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column - 1, Position.Line); //west
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column - 1, Position.Line - 1); //north-west
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            if(MovimentsCount == 0 && !match.Check) //castling
            {
                Position rightRockPosition = new(Position.Column + 3, Position.Line); //right castling
                if (RockCanPerformCastling(rightRockPosition))
                {
                    Position position1 = new(Position.Column + 1, Position.Line);
                    Position position2 = new(Position.Column + 2, Position.Line);

                    if(Board.Piece(position1) == null && Board.Piece(position2) == null)
                        movements[Position.Line, Position.Column + 2] = true;
                }

                Position leftRockPosition = new(Position.Line, Position.Column - 4); //left castling
                if (RockCanPerformCastling(leftRockPosition))
                {
                    Position position1 = new(Position.Column - 1, Position.Line);
                    Position position2 = new(Position.Column - 2, Position.Line);
                    Position position3 = new(Position.Column - 3, Position.Line);

                    if (Board.Piece(position1) == null && Board.Piece(position2) == null && Board.Piece(position3) == null)
                        movements[Position.Line, Position.Column - 2] = true;
                }
            }

            return movements;
        }
    }
}
