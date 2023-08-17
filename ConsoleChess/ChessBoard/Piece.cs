namespace ChessBoard
{
    internal abstract class Piece
    {
        public Position Position { get; set; }
        public Color Color { get; protected set; }
        public int MovimentsCount { get; protected set; }
        public Board Board { get; protected set; }

        public Piece (Color color, Board board)
        {
            Position = null;
            Color = color;
            Board = board;
            MovimentsCount = 0;
        }

        public void IncreaseMovement() => MovimentsCount++;

        public void DecreaseMovement() => MovimentsCount--;

        protected virtual bool PieceCanMove(Position position)
        {
            if (Board.IsValidPosition(position))
            {
                Piece piece = Board.Piece(position);
                return piece == null || piece.Color != Color;
            }                

            return false;
        }

        public bool IsThereAnyPossibleMovements()
        {
            bool[,] movements = PossibleMovements();

            for(int i = 0; i < Board.Lines; i++)
            {
                for(int j = 0; j < Board.Columns; j++)
                {
                    if(movements[i, j])
                        return true;
                }
            }

            return false;
        }

        public bool CanMoveToDestiny(Position position)
        {
            return PossibleMovements()[position.Line, position.Column];
        }

        public abstract bool[,] PossibleMovements();
    }
}
