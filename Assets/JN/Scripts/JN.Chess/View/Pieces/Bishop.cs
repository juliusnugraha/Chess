using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class Bishop : Piece
    {
        public override List<Vector2Int> GenerateAvailableMove()
        {
            listOfAvailableMoves.Clear();

            listOfAvailableMoves = MoveGenerator.GenerateMoveByDirection(this, board);

            return listOfAvailableMoves;
        }
        
        public override void Move(Vector2Int coords)
        {
            base.Move(coords);
        }
    }
}
