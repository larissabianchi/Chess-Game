namespace ChessBoard
{
    internal class Coordinates
    {
        public char Column { get; set; }
        public int Line { get; set; }

        public Coordinates(char column, int line)
        {
            Column = column;
            Line = line;
        }

        public Position ToPosition()
        {
            return new Position(Column - 'a', 8 - Line);
        }

        public override string ToString()
        {
            return "" + Column + Line;
        }
    }
}
