using System.Collections.Generic;
using JN.Utils;
using UnityEngine;

namespace JN.Chess
{
    public class PieceSpawner : MonoBehaviour
    {
        [SerializeField] private List<Piece> _listPiecePrefab;
        [SerializeField] private Material _blackMaterial;
        [SerializeField] private Material _whiteMaterial;

        private Dictionary<PieceType, ObjectPool<Piece>> _dictPiecePool;
    
        public void Init()
        {
            _dictPiecePool = new Dictionary<PieceType, ObjectPool<Piece>>();

            foreach(Piece obj in _listPiecePrefab)
            {
                PieceType pieceType = obj.pieceType;

                ObjectPool<Piece> poolPiece = new ObjectPool<Piece>();
                poolPiece.Init(pieceType.ToString(), obj.gameObject);

                if(pieceType != PieceType.None)
                {
                    _dictPiecePool.Add(pieceType, poolPiece);
                }
                else
                {
                    Debug.LogError("Cannot find PieceType with object name " + pieceType);
                }
            }    
        }

        public Piece SpawnPiece(PieceType pieceType)
        {
            return _dictPiecePool[pieceType].GetNewObject();
        }

        public Material GetPieceMaterial(TeamColor pColor)
        {
            return pColor == TeamColor.White ? _whiteMaterial : _blackMaterial;
        }
    }
}