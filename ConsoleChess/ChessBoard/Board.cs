using ChessException;

namespace ChessBoard
{
    internal class Board
    {
        public int Lines { get; set; }
        public int Columns { get; set; }
        
        private readonly Piece[,] Pieces;

        public Board(int columns, int lines)
        {
            Lines = lines;
            Columns = columns;
            Pieces = new Piece[lines, columns];
        }

        public Piece Piece(int column, int line)
        {
            return Pieces[line, column];
        }

        public Piece Piece(Position position)
        {
            return Pieces[position.Column, position.Line];
        }

        public void AddPiece(Piece piece, Position position)
        {
            if (!IsValidMoviment(position))
                throw new BoardException("There is already a piece at this position!");

            Pieces[position.Column, position.Line] = piece;
            piece.Position = position;
        }

        public Piece RemovePiece(Position position)
        {
            if (Piece(position) == null)
                return null;

            Piece piece = Piece(position);
            piece.Position = null;
            Pieces[position.Column, position.Line] = null;
            return piece;
        }

        public bool IsValidMoviment(Position position)
        {
            CheckIfPositionIsValid(position);
            return Piece(position) == null;
        }

        public bool IsValidPosition(Position position)
        {
            if(position.Line < 0 || position.Line >= Lines || position.Column < 0 || position.Column >= Columns)
                return false;

            return true;
        }        

        public void CheckIfPositionIsValid(Position position)
        {
            if (!IsValidPosition(position))
                throw new BoardException("Position is not valid!");
        }
    }
}
