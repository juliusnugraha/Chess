using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    public class King : Piece
    {
        public override List<Vector2Int> listOfAvailableMove()
        {
            avaliableMoves.Clear();

            avaliableMoves.Add(coordinate + new Vector2Int(0,1));

            return avaliableMoves;
        }
        
        public override void Move(Vector2Int coords)
        {
            base.Move(coords);
        }
    }
}
