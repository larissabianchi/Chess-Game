using ChessBoard;

namespace ChessPieces
{
    internal class Rock : Piece
    {
        public Rock(Color color, Board board) : base(color, board)
        {
        }

        public override string ToString()
        {
            return "R";
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] movements = new bool[Board.Lines, Board.Columns];

            Position position = new(0, 0);

            position.DefineValues(Position.Column, Position.Line - 1); //north
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;

                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.Line--;
            }

            position.DefineValues(Position.Column, Position.Line + 1); //south
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;

                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.Line++;
            }

            position.DefineValues(Position.Column + 1, Position.Line); //east
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;

                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.Column++;
            }

            position.DefineValues(Position.Column - 1, Position.Line); //west
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;

                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.Column--;
            }

            return movements;
        }
    }
}
