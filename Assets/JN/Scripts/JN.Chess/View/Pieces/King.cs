using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class King : Piece
    {
        private Piece leftRook;
        private Piece rightRook;

        private Vector2Int leftCastlingMoveCoord;
        private Vector2Int rightCastlingMoveCoord;
        private bool isCanCastlingMove;

        public override List<Vector2Int> GenerateAvailableMove()
        {
            listOfAvailableMoves.Clear();

            listOfAvailableMoves = MoveGenerator.GenerateMoveByDirection(this, board);
            GenerateCastlingMove();

            return listOfAvailableMoves;
        }

        private void GenerateCastlingMove()
        {
            isCanCastlingMove = false;

            if (hasMoved == false)
            {
                isCanCastlingMove = true;

                leftRook = board.GetPieceInDirection<Rook>(coordinate, team, Vector2Int.left);
                if (leftRook && !leftRook.hasMoved)
                {
                    leftCastlingMoveCoord = coordinate + Vector2Int.left * 2;
                    listOfAvailableMoves.Add(leftCastlingMoveCoord);
                }

                rightRook = board.GetPieceInDirection<Rook>(coordinate, team, Vector2Int.right);
                if (rightRook && !rightRook.hasMoved)
                {
                    rightCastlingMoveCoord = coordinate + Vector2Int.right * 2;
                    listOfAvailableMoves.Add(rightCastlingMoveCoord);
                }
            }
        }

        public override void Move(Vector2Int coords)
        {
            base.Move(coords);

            if (isCanCastlingMove)
            {
                if(coords == leftCastlingMoveCoord)
                {
                    Vector2Int targetCoords = coords + Vector2Int.right;
                    board.UpdateBoardOnMovePiece(targetCoords, leftRook.coordinate, leftRook, null);
                    leftRook.Move(targetCoords);
                }
                else if (coords == rightCastlingMoveCoord)
                {
                    Vector2Int targetCoords = coords + Vector2Int.left;
                    board.UpdateBoardOnMovePiece(targetCoords, rightRook.coordinate, rightRook, null);
                    rightRook.Move(targetCoords);
                }
            }
        }
    }
}
