using ChessBoard;
using ChessException;

namespace ConsoleChess
{
    internal static class Screen
    {
        public static void PrintMatch(Match match)
        {
            PrintBoard(match.Board);

            PrintCapturedPieces(match);
            Console.WriteLine("");

            if (!match.Finished)
            {
                if (match.Check)
                    Console.WriteLine("CHECK!");

                Console.WriteLine("");

                Console.WriteLine($"Turn: {match.Turn}");
                Console.WriteLine($"Waiting for the player of the {match.PlayerColor} pieces to move");
            }
            else
            {
                Console.WriteLine("CHECKMATE!");
                Console.WriteLine($"Winner: {match.PlayerColor}");
            }

            Console.WriteLine("");
        }

        public static void PrintCapturedPieces(Match match)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.WriteLine("Captured Pieces");

            Console.Write("White: ");
            PrintHashSet(match.CapturedPiece(Color.White));

            Console.WriteLine("");

            Console.Write("Black: ");            
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            PrintHashSet(match.CapturedPiece(Color.Black));
                        
            Console.WriteLine("");

            Console.ForegroundColor = defaultColor;
        }

        public static void PrintHashSet(HashSet<Piece> captureds)
        {
            Console.Write("[ ");

            foreach (Piece piece in captureds)
                Console.Write($"{piece} ");

            Console.Write("]");
        }

        public static void PrintBoard(Board board)
        {
            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                    PrintPiece(board.Piece(i, j));

                Console.WriteLine();
            }

            PrintBoardLetters();
        }

        public static void PrintBoard(Board board, bool[,] possibleCoordinates)
        {
            ConsoleColor defaultBackground = Console.BackgroundColor;
            ConsoleColor possibleCooBackground = ConsoleColor.Red;

            for (int i = 0; i < board.Lines; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < board.Columns; j++)
                {
                    if (possibleCoordinates[i, j])
                        Console.BackgroundColor = possibleCooBackground;
                    else
                        Console.BackgroundColor = defaultBackground;

                    PrintPiece(board.Piece(i, j));
                    Console.BackgroundColor = defaultBackground;
                }

                Console.WriteLine();
            }

            PrintBoardLetters();
            Console.BackgroundColor = defaultBackground;
        }

        private static void PrintBoardLetters()
        {
            Console.WriteLine("  a b c d e f g h");
            Console.WriteLine("");
        }

        public static Coordinates GetCoordinates()
        {
            try
            {
                string coordinate = Console.ReadLine();
                char column = coordinate[0];
                int line = int.Parse(coordinate[1] + "");
                return new Coordinates(column, line);
            }
            catch
            {
                throw new BoardException("This coordinate is not valid!");
            }
        }

        public static void PrintPiece(Piece piece)
        {
            if (piece == null)
                Console.Write("- ");
            else
            {
                if (piece.Color == Color.White)
                    Console.Write(piece + " ");
                else
                {
                    ConsoleColor defaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(piece + " ");
                    Console.ForegroundColor = defaultColor;
                }
            }                        
        }

        public static string PrintPromoteTextAndGetUserOption(Color player)
        {
            Console.WriteLine();

            Console.WriteLine($"The {player} player must choose the piece to which your pawn will be promoted:");
            Console.WriteLine("B to Bishop");
            Console.WriteLine("H to Horse");
            Console.WriteLine("R to Rock");
            Console.WriteLine("Q to Queen");

            Console.WriteLine();

            return Console.ReadLine();
        }
    }
}
