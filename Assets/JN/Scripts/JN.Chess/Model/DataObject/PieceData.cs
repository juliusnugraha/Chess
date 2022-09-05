using System;
using UnityEngine;

namespace JN.Chess
{
    [Serializable]
    public class PieceData
    {
        public Vector2Int position;
        public PieceType pieceType;
        public TeamColor teamColor;
    }
}