using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class Queen : Piece
    {
        public override List<Vector2Int> GenerateAvailableMove()
        {
            listOfAvaliableMoves.Clear();
            
            listOfAvaliableMoves = MoveGenerator.GenerateMoveByDirection(coordinate, pieceType, board);
            
            return listOfAvaliableMoves;
        }
        
        public override void Move(Vector2Int coords)
        {
            base.Move(coords);
        }
    }
}
