namespace ChessBoard
{
    internal class Position
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public Position(int column, int line)
        {
            Column = column;
            Line = line;            
        }

        public void DefineValues(int column, int line)
        {
            Column = column;
            Line = line;            
        }

        public override string ToString()
        {
            return Line + ", " + Column;
        }
    }
}
