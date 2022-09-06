using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class Pawn : Piece
    {
        public override List<Vector2Int> GenerateAvailableMove()
        {
            listOfAvaliableMoves.Clear();

            List<Vector2Int> listOfPossibleMove = new List<Vector2Int>();

            int range = hasMoved ? 1 : 2;
            Vector2Int direction = team == TeamColor.White ? Vector2Int.up : Vector2Int.down;

            for (int i = 1; i <= range; i++)
            {
                listOfPossibleMove.Add(coordinate + direction * i);
            }

            // take opponent direction
            Vector2Int[] pawnKillDirection = new Vector2Int[]
            {
                new Vector2Int(1, direction.y),
                new Vector2Int(-1, direction.y)
            };

            for (int i = 0; i < pawnKillDirection.Length; i++)
            {
                Vector2Int nextCoords = coordinate + pawnKillDirection[i];
                if(board.IsCoordinateOnBoard(nextCoords))
                {
                    Piece piece = board.GetPieceOnBoard(nextCoords);
                    if(piece && piece.team != team)
                    {
                        listOfPossibleMove.Add(nextCoords);
                    }
                }
            }

            listOfAvaliableMoves = MoveGenerator.CheckMoveCoordinate(this, listOfPossibleMove, board);

            return listOfAvaliableMoves;
        }
        
        public override void Move(Vector2Int coords)
        {
            base.Move(coords);
        }
    }

}