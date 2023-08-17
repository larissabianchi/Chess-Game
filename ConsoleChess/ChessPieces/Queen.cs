using ChessBoard;

namespace ChessPieces
{
    internal class Queen : Piece
    {
        public Queen(Color color, Board board) : base(color, board)
        {

        }

        public override string ToString()
        {
            return "Q";
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] movements = new bool[Board.Lines, Board.Columns];

            Position position = new(0, 0);

            position.DefineValues(Position.Column - 1, Position.Line); //west
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column - 1, position.Line);
            }

            position.DefineValues(Position.Column + 1, Position.Line); //east
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column + 1, position.Line);
            }

            position.DefineValues(Position.Column, Position.Line - 1); //north
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column, position.Line - 1);
            }

            position.DefineValues(Position.Column, Position.Line + 1); //south
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column, position.Line + 1);
            }

            position.DefineValues(Position.Column - 1, Position.Line - 1); //north-west
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column - 1, position.Line - 1);
            }

            position.DefineValues(Position.Column + 1, Position.Line - 1); //north-east
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column + 1, position.Line - 1);
            }

            position.DefineValues(Position.Column + 1, Position.Line + 1); //south-east
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column + 1, position.Line + 1);
            }

            position.DefineValues(Position.Column - 1, Position.Line + 1); //south-west
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column - 1, position.Line + 1);
            }

            return movements;
        }
    }
}
