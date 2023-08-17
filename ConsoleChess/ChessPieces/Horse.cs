using ChessBoard;

namespace ChessPieces
{
    internal class Horse : Piece
    {
        public Horse(Color color, Board board) : base(color, board)
        {

        }

        public override string ToString()
        {
            return "H";
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] movements = new bool[Board.Lines, Board.Columns];

            Position position = new(0, 0);

            position.DefineValues(Position.Column - 2, Position.Line - 1);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column - 1, Position.Line - 2);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column + 1, Position.Line - 2);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column + 2, Position.Line - 1);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column + 2, Position.Line + 1);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column + 1, Position.Line + 2);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column - 1, Position.Line + 2);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            position.DefineValues(Position.Column - 2, Position.Line + 1);
            if (PieceCanMove(position))
                movements[position.Line, position.Column] = true;

            return movements;
        }
    }
}
