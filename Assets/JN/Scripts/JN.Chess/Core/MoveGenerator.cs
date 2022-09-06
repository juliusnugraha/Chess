using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JN.Chess
{
    public static class MoveGenerator
    {
        public static Vector2Int[] crossDirections = new Vector2Int[] 
        {
            Vector2Int.up, 
            Vector2Int.down, 
            Vector2Int.left, 
            Vector2Int.right
        };

        public static Vector2Int[] diagonalDirections = new Vector2Int[] 
        {
            new Vector2Int (1,1), 
            new Vector2Int (1,-1), 
            new Vector2Int (-1,1), 
            new Vector2Int (-1,-1)
        };

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


        public static List<Vector2Int> GenerateMoveByDirection(Piece piece, BoardGame board)
        {
            List<Vector2Int> listMove = new List<Vector2Int>();

            switch(piece.pieceType)
            {
                case PieceType.King:
                    listMove = GeneratePointMove(piece, diagonalDirections.Concat(crossDirections).ToArray(), board);
                break;

                case PieceType.Knight:
                    listMove = GeneratePointMove(piece, knightDirections, board);
                break;

                case PieceType.Bishop:
                    listMove = GenerateSlidingMove(piece, diagonalDirections, board);
                break;

                case PieceType.Rook:
                    listMove = GenerateSlidingMove(piece, crossDirections, board);
                break;

                case PieceType.Queen:
                    listMove = GenerateSlidingMove(piece, diagonalDirections.Concat(crossDirections).ToArray(), board);
                break;

            }

            return listMove;
        }
        
        private static List<Vector2Int> GeneratePointMove(Piece piece, Vector2Int[] directions, BoardGame board)
        {
            List<Vector2Int> listDirection = new List<Vector2Int>();

            foreach(Vector2Int direction in directions)
            {
                listDirection.Add(piece.coordinate + direction);
            }

            return CheckMoveCoordinate(piece, listDirection, board);
        }

        public static List<Vector2Int> CheckMoveCoordinate(Piece piece, List<Vector2Int> listCoords, BoardGame board)
        {
            List<Vector2Int> listMove = new List<Vector2Int>();

            foreach(Vector2Int nextCoords in listCoords)
            {
                if(board.IsCoordinateOnBoard(nextCoords) == false)
                    continue;

                Piece nextPiece = board.GetPieceOnBoard(nextCoords);
                if(nextPiece == null)
                {
                    listMove.Add(nextCoords);
                    continue;
                }
                else
                {
                    if(piece.team == nextPiece.team)
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

        private static List<Vector2Int> GenerateSlidingMove(Piece piece, Vector2Int[] directions, BoardGame board)
        {
            List<Vector2Int> listMove = new List<Vector2Int>();

            foreach(Vector2Int direction in directions)
            {
                int i = 1;
                while (true)
                {
                    Vector2Int nextCoords = piece.coordinate + direction * i;
                    if(board.IsCoordinateOnBoard(nextCoords) == false)
                        break;

                    Piece nextPiece = board.GetPieceOnBoard(nextCoords);
                    if(nextPiece == null)
                    {
                        listMove.Add(nextCoords);
                        i++;
                        continue;
                    }
                    else
                    {
                        if(piece.team == nextPiece.team)
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