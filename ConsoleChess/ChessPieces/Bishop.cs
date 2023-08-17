using ChessBoard;

namespace ChessPieces
{
    internal class Bishop : Piece
    {
        public Bishop(Color color, Board board) : base(color, board)
        {

        }

        public override string ToString()
        {
            return "B";
        }

        public override bool[,] PossibleMovements()
        {
            bool[,] movements = new bool[Board.Lines, Board.Columns];

            Position position = new(0, 0);
                        
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

            position.DefineValues(Position.Column - 1, Position.Line + 1); //south-west
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column - 1, position.Line + 1);
            }

            position.DefineValues(Position.Column + 1, Position.Line + 1); //south-east
            while (PieceCanMove(position))
            {
                movements[position.Line, position.Column] = true;
                if (Board.Piece(position) != null && Board.Piece(position).Color != Color)
                    break;

                position.DefineValues(position.Column + 1, position.Line + 1);
            }            

            return movements;
        }
    }
}
