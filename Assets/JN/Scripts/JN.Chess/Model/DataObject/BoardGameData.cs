using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace JN.Chess
{
    [CreateAssetMenu(menuName = "Chess/BoardGameData")]
    public class BoardGameData : ScriptableObject
    {
        [Button]
        private void LoadFromFEN()
        {
            ChessGameData data = new ChessGameData();
            data.LoadFromFEN(ChessGameData.START_FEN);
            _boardPieceData = data.pieces.ToArray();
        }

        [SerializeField] private PieceData[] _boardPieceData;
        
        public int GetPieceCount()
        {
            return _boardPieceData.Length;
        }

        public Vector2Int GetPieceCoordinateAtIndex(int index)
        {
            return new Vector2Int(_boardPieceData[index].position.x, _boardPieceData[index].position.y);
        }

        public TeamColor GetPieceTeamColorAtIndex(int index)
        {
            return _boardPieceData[index].teamColor;
        }

        public String GetPieceTypeStringAtIndex(int index)
        {
            return _boardPieceData[index].pieceType.ToString();
        }

        public PieceType GetPieceTypeAtIndex(int index)
        {
            return _boardPieceData[index].pieceType;
        }
    }
}