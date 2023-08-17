using ChessBoard;
using ChessPieces;
using ConsoleChess;
using ChessException;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Match match = new();

                while (!match.Finished)
                {
                    try
                    {
                        Console.Clear();
                        
                        Screen.PrintMatch(match);

                        Console.Write("Origin: ");
                        Position origin = Screen.GetCoordinates().ToPosition();
                        match.CheckOriginPosition(origin);

                        bool[,] possibleCoordinates = match.Board.Piece(origin).PossibleMovements();

                        Console.Clear();

                        Screen.PrintBoard(match.Board, possibleCoordinates);

                        Console.Write("Destiny: ");
                        Position destiny = Screen.GetCoordinates().ToPosition();
                        match.CheckDestinyPosition(origin, destiny);

                        match.PerformTheMove(origin, destiny);
                    }
                    catch (BoardException e)
                    {
                        Console.WriteLine(e.Message);
                        Console.WriteLine("Press enter to continue.");
                        Console.ReadLine();
                    }
                }

                Console.Clear();

                Screen.PrintMatch(match);
            }
            catch (BoardException e)
            {
                Console.WriteLine(e.Message);
            }            
        }
    }
}