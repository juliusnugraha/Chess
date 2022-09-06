using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JN.Chess
{
    public static class MoveGenerator
    {
        public static Vector2Int[] crossDirections = new Vector2Int[] {Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right};
        public static Vector2Int[] diagonalDirections = new Vector2Int[] {new Vector2Int (1,1), new Vector2Int (1,-1), new Vector2Int (-1,1), new Vector2Int (-1,-1)};
        public static Vector2Int[] knightDirections = new Vector2Int[] 
        {
            new Vector2Int (1,2),
            new Vector2Int (1,-2),
            new Vector2Int (-1,2),
            new Vector2Int (-1,-2),
            new Vector2Int (2,1),
            new Vector2Int (2,-1),
            new Vector2Int (-2,1),
            new Vector2Int (-2,-1),
        };


        public static List<Vector2Int> GenerateMoveByDirection(Vector2Int coords, PieceType pieceType, BoardGame board)
        {
            List<Vector2Int> listMove = new List<Vector2Int>();

            switch(pieceType)
            {
                case PieceType.Knight:
                    listMove = GenerateKnightMove(coords, knightDirections, board);
                break;

                case PieceType.Bishop:
                    listMove = GenerateSlidingMove(coords, diagonalDirections, board);
                break;

                case PieceType.Rook:
                    listMove = GenerateSlidingMove(coords, crossDirections, board);
                break;

                case PieceType.Queen:
                    listMove = GenerateSlidingMove(coords, diagonalDirections.Concat(crossDirections).ToArray(), board);
                break;

                case PieceType.King:
                    listMove = GenerateSlidingMove(coords, diagonalDirections.Concat(crossDirections).ToArray(), board);
                break;

                case PieceType.Pawn:
                    listMove = GeneratePawnMove(coords, crossDirections, board);
                break;

                
            }

            return listMove;
        }

        private static List<Vector2Int> CheckMoveCoordinate(List<Vector2Int> listCoords, BoardGame board)
        {
            List<Vector2Int> listMove = new List<Vector2Int>();

            foreach(Vector2Int nextCoords in listCoords)
            {
                if(board.IsCoordinateOnBoard(nextCoords) == false)
                    continue;

                Piece piece = board.GetPieceOnBoard(nextCoords);
                if(piece == null)
                {
                    listMove.Add(nextCoords);
                    continue;
                }
                else
                {
                    if(ChessGameController.Instance.IsCurrentActivePlayerTeam(piece.team))
                    {
                        continue;
                    }
                    else
                    {
                        listMove.Add(nextCoords);
                        continue;
                    }
                }
            }

            return listMove;
        }
        
        private static List<Vector2Int> GenerateKnightMove(Vector2Int coords, Vector2Int[] directions, BoardGame board)
        {
            List<Vector2Int> listCoords = new List<Vector2Int>();

            foreach(Vector2Int direction in directions)
            {
                listCoords.Add(coords + direction);
            }

            return CheckMoveCoordinate(listCoords, board);
        }

        private static List<Vector2Int> GeneratePawnMove(Vector2Int coords, Vector2Int[] directions, BoardGame board)
        {
            List<Vector2Int> listCoords = new List<Vector2Int>();

            foreach(Vector2Int direction in directions)
            {
                listCoords.Add(coords + direction);
            }

            return CheckMoveCoordinate(listCoords, board);
        }

        private static List<Vector2Int> GenerateSlidingMove(Vector2Int coords, Vector2Int[] directions, BoardGame board)
        {
            List<Vector2Int> listMove = new List<Vector2Int>();

            foreach(Vector2Int direction in directions)
            {
                int i = 1;
                while (true)
                {
                    Vector2Int nextCoords = coords + direction * i;
                    if(board.IsCoordinateOnBoard(nextCoords) == false)
                        break;

                    Piece piece = board.GetPieceOnBoard(nextCoords);
                    if(piece == null)
                    {
                        listMove.Add(nextCoords);
                        i++;
                        continue;
                    }
                    else
                    {
                        if(ChessGameController.Instance.IsCurrentActivePlayerTeam(piece.team))
                        {
                            break;
                        }
                        else
                        {
                            listMove.Add(nextCoords);
                            break;
                        }
                    }
                }
            }

            return listMove;
        }
    }
}