using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class Rook : Piece
    {
        public override List<Vector2Int> GenerateAvailableMove()
        {
            listOfAvaliableMoves.Clear();

            listOfAvaliableMoves = MoveGenerator.GenerateMoveByDirection(this, board);
            
            return listOfAvaliableMoves;
        }
        
        public override void Move(Vector2Int coords)
        {
            base.Move(coords);
        }
    }
}
