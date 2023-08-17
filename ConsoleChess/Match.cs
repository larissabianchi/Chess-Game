using ChessBoard;
using ChessException;
using ChessPieces;
using System.Linq;

namespace ConsoleChess
{
    internal class Match
    {
        public Board Board { get; private set; }
        public int Turn { get; private set; }
        public Color PlayerColor { get; private set; }
        public bool Finished { get; private set; }
        public bool Check { get; private set; }
        public Piece? VulnerableToEnPassant { get; private set; }

        private readonly HashSet<Piece> pieces = new();

        private readonly HashSet<Piece> captureds = new();

        public Match()
        {
            Board = new Board(8, 8);
            Turn = 1;
            PlayerColor = Color.White;
            Finished = false;
            Check = false;
            VulnerableToEnPassant = null;

            AddPieces();
        }

        public Piece ExecuteMoviment(Position origin, Position destiny)
        {
            Piece piece = Board.RemovePiece(origin);
            piece.IncreaseMovement();
            Piece capturedPiece = Board.RemovePiece(destiny);
            Board.AddPiece(piece, destiny);

            if(capturedPiece != null)
                captureds.Add(capturedPiece);

            if(piece is King && destiny.Column == origin.Column + 2) //right castling
            {
                Position rockOrigin = new(origin.Column + 3, origin.Line);
                Position rockDestiny = new(destiny.Column + 1, destiny.Line);

                Piece rock = Board.RemovePiece(rockOrigin);
                rock.IncreaseMovement();
                Board.AddPiece(rock, rockDestiny);
            }

            if (piece is King && destiny.Column == origin.Column - 2) //left castling
            {
                Position rockOrigin = new(origin.Column - 4, origin.Line);
                Position rockDestiny = new(destiny.Column - 1, destiny.Line);

                Piece rock = Board.RemovePiece(rockOrigin);
                rock.IncreaseMovement();
                Board.AddPiece(rock, rockDestiny);
            }

            if(piece is Pawn && origin.Column != destiny.Column && capturedPiece == null) //en passant
            {
                Position pawnPosition;

                if(piece.Color == Color.White)
                    pawnPosition = new(destiny.Column, destiny.Line + 1);
                else 
                    pawnPosition = new(destiny.Column, destiny.Line - 1);

                capturedPiece = Board.RemovePiece(pawnPosition);
                captureds.Add(capturedPiece);
            }

            return capturedPiece;
        }

        public void UndoMovement(Position origin, Position destiny, Piece capturedPiece)
        {
            Piece piece = Board.RemovePiece(destiny);
            piece.DecreaseMovement();

            if(capturedPiece != null)
            {
                Board.AddPiece(capturedPiece, destiny);
                captureds.Remove(capturedPiece);
            }

            Board.AddPiece(piece, origin);

            if (piece is King && destiny.Column == origin.Column + 2) //right castling
            {
                Position rockOrigin = new(origin.Column + 3, origin.Line);
                Position rockDestiny = new(destiny.Column + 1, destiny.Line);

                Piece rock = Board.RemovePiece(rockDestiny);
                rock.DecreaseMovement();
                Board.AddPiece(rock, rockOrigin);
            }

            if (piece is King && destiny.Column == origin.Column - 2) //left castling
            {
                Position rockOrigin = new(origin.Column - 4, origin.Line);
                Position rockDestiny = new(destiny.Column - 1, destiny.Line);

                Piece rock = Board.RemovePiece(rockDestiny);
                rock.DecreaseMovement();
                Board.AddPiece(rock, rockOrigin);
            }

            if (piece is Pawn && origin.Column != destiny.Column && capturedPiece == VulnerableToEnPassant) //en passant
            {
                Piece pawn = Board.RemovePiece(destiny);
                Position pawnPosition;

                if (piece.Color == Color.White)
                    pawnPosition = new(destiny.Column, 3);
                else
                    pawnPosition = new(destiny.Column, 4);

                Board.AddPiece(pawn, pawnPosition);
            }
        }

        public void PerformTheMove(Position origin, Position destiny)
        {
            Piece capturedPiece = ExecuteMoviment(origin, destiny);
            VulnerableToEnPassant = null;
            Check = false;

            if (IsInCheck(PlayerColor))
            {
                UndoMovement(origin, destiny, capturedPiece);
                throw new BoardException("You can not put yourself in check!");
            }

            Piece piece = Board.Piece(destiny);

            if(piece is Pawn && ((piece.Color == Color.White && destiny.Line == 0) || (piece.Color == Color.Black && destiny.Line == 7))) //promote
                PromotePawn(destiny);

            if (IsInCheck(Adversary(PlayerColor)))
                Check = true;                

            if(IsInCheckmate(Adversary(PlayerColor)))
                Finished = true;
            else
            {
                Turn++;
                ChangePlayer();
            }
            
            if (piece is Pawn && (destiny.Line == origin.Line + 2 || destiny.Line == origin.Line - 2))
                VulnerableToEnPassant = piece;
        }

        public void CheckOriginPosition(Position position)
        {
            if (Board.Piece(position) == null)
                throw new BoardException("There is no piece in the origin position informed.");

            if (PlayerColor != Board.Piece(position).Color)
                throw new BoardException("The piece in the origin position does not belong to you!");

            if (!Board.Piece(position).IsThereAnyPossibleMovements())
                throw new BoardException("There are no possible movements for the chosen piece.");
        }

        public void CheckDestinyPosition(Position origin, Position destiny)
        {
            if (!Board.Piece(origin).CanMoveToDestiny(destiny))
                throw new BoardException("Destiny's position is not valid.");
        }        

        public HashSet<Piece> CapturedPiece(Color color)
        {
            HashSet<Piece> aux = new();

            foreach(Piece piece in captureds)
            {
                if (piece.Color == color)
                    aux.Add(piece);
            }

            return aux;
        }

        public HashSet<Piece> PiecesInTheGame(Color color)
        {
            HashSet<Piece> aux = new();

            foreach (Piece piece in pieces)
            {
                if (piece.Color == color)
                    aux.Add(piece);
            }

            aux.ExceptWith(CapturedPiece(color));
            return aux;
        }

        public void AddNewPiece(char column, int line, Piece piece)
        {
            Board.AddPiece(piece, new Coordinates(column, line).ToPosition());
            pieces.Add(piece);
        }

        public bool IsInCheck(Color color)
        {
            Piece king = King(color);

            if (king == null)
                throw new BoardException($"There is no {color} King in the board!");

            foreach(Piece piece in PiecesInTheGame(Adversary(color)))
            {
                bool[,] movements = piece.PossibleMovements();
                
                if(movements[king.Position.Line, king.Position.Column])
                    return true;
            }

            return false;
        }

        public bool IsInCheckmate(Color color)
        {
            if (!IsInCheck(color))
                return false;

            foreach(Piece piece in PiecesInTheGame(color))
            {
                bool[,] movements = piece.PossibleMovements();

                for(int i = 0; i < Board.Lines; i++)
                {
                    for(int j = 0; j < Board.Columns; j++)
                    {
                        if (movements[i, j])
                        {
                            Position origin = piece.Position;
                            Position destiny = new(j,i);

                            Piece capturedPiece = ExecuteMoviment(origin, destiny);

                            bool testCheck = IsInCheck(color);
                        
                            UndoMovement(origin, destiny, capturedPiece);

                            if(!testCheck)
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        private void PromotePawn(Position destiny)
        {
            Piece pawn = Board.RemovePiece(destiny);
            pieces.Remove(pawn);

            string option = Screen.PrintPromoteTextAndGetUserOption(PlayerColor);

            Piece? promotePiece;
            switch (option)
            {
                case "b":
                    promotePiece = new Bishop(pawn.Color, Board);
                    break;

                case "h":
                    promotePiece = new Horse(pawn.Color, Board);
                    break;

                case "r":
                    promotePiece = new Rock(pawn.Color, Board);
                    break;

                case "q":
                    promotePiece = new Queen(pawn.Color, Board);
                    break;

                default:
                    Console.WriteLine("This option is not valid! Promoting Pawn to Queen.");
                    Console.WriteLine("Press enter to continue.");
                    Console.ReadLine();
                    promotePiece = new Queen(pawn.Color, Board);
                    break;
            }
            
            Board.AddPiece(promotePiece, destiny);
            pieces.Add(promotePiece);
        }

        private void ChangePlayer()
        {
            if (PlayerColor == Color.White)
                PlayerColor = Color.Black;
            else
                PlayerColor = Color.White;
        }

        private static Color Adversary(Color color)
        {
            if (color == Color.White)
                return Color.Black;
            else
                return Color.White;
        }

        private Piece King(Color color)
        {
            foreach (var piece in PiecesInTheGame(color).Where(piece => piece is King))
                return piece;

            return null;
        }

        private void AddPieces()
        {
            AddNewPiece('a', 8, new Rock(Color.Black, Board));
            AddNewPiece('b', 8, new Horse(Color.Black, Board));
            AddNewPiece('c', 8, new Bishop(Color.Black, Board));
            AddNewPiece('d', 8, new Queen(Color.Black, Board));
            AddNewPiece('e', 8, new King(Color.Black, Board, this));
            AddNewPiece('f', 8, new Bishop(Color.Black, Board));
            AddNewPiece('g', 8, new Horse(Color.Black, Board));
            AddNewPiece('h', 8, new Rock(Color.Black, Board));
            AddNewPiece('a', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('b', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('c', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('d', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('e', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('f', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('g', 7, new Pawn(Color.Black, Board, this));
            AddNewPiece('h', 7, new Pawn(Color.Black, Board, this));


            AddNewPiece('a', 1, new Rock(Color.White, Board));
            AddNewPiece('b', 1, new Horse(Color.White, Board));
            AddNewPiece('c', 1, new Bishop(Color.White, Board));
            AddNewPiece('d', 1, new Queen(Color.White, Board));
            AddNewPiece('e', 1, new King(Color.White, Board, this));
            AddNewPiece('f', 1, new Bishop(Color.White, Board));
            AddNewPiece('g', 1, new Horse(Color.White, Board));
            AddNewPiece('h', 1, new Rock(Color.White, Board));
            AddNewPiece('a', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('b', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('c', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('d', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('e', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('f', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('g', 2, new Pawn(Color.White, Board, this));
            AddNewPiece('h', 2, new Pawn(Color.White, Board, this));
        }
    }
}
