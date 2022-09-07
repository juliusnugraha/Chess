using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class Pawn : Piece
    {
        public override List<Vector2Int> GenerateAvailableMove()
        {
            listOfAvailableMoves.Clear();

            List<Vector2Int> listOfPossibleMove = new List<Vector2Int>();

            int range = hasMoved ? 1 : 2;
            Vector2Int direction = team == TeamColor.White ? Vector2Int.up : Vector2Int.down;

            for (int i = 1; i <= range; i++)
            {
                Vector2Int moveCoords = coordinate + direction * i;
                if(board.GetPieceOnBoard(moveCoords) == null)
                    listOfPossibleMove.Add(moveCoords);
                else
                    break;
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

            listOfAvailableMoves = MoveGenerator.CheckMoveCoordinate(this, listOfPossibleMove, board);

            return listOfAvailableMoves;
        }
        
        public override void Move(Vector2Int coords)
        {
            base.Move(coords);
            CheckPromotion();
        }

        private void CheckPromotion()
    {
        int endOfBoardYCoord = team == TeamColor.White ? BoardGame.BOARD_SIZE - 1 : 0;
        if (coordinate.y == endOfBoardYCoord)
        {
            board.PromotePiece(this);
        }
    }
    }

}