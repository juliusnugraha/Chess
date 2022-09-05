using System;
using System.Collections.Generic;
using JN.Utils;
using UnityEngine;

namespace JN.Chess
{
    public class PieceSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _listPiecePrefab;
        [SerializeField] private Material _blackMaterial;
        [SerializeField] private Material _whiteMaterial;

        private Dictionary<PieceType, GameObject> dictPiece;

        public void Init()
        {
            dictPiece = new Dictionary<PieceType, GameObject>();

            foreach(GameObject obj in _listPiecePrefab)
            {
                PieceType pieceType = obj.GetComponent<Piece>().pieceType;

                if(pieceType != PieceType.None)
                {
                    dictPiece.Add(pieceType, obj);
                }
                else
                {
                    Debug.LogError("Cannot find PieceType with object name " + pieceType);
                }
            }    
        }

        public GameObject Spawn(PieceType pieceType)
        {
            GameObject prefab = dictPiece[pieceType];

            if (prefab)
            {
                GameObject newPiece = Instantiate(prefab);
                return newPiece;
            }
            
            return null;
        }

        public Material GetPieceMaterial(TeamColor pColor)
        {
            return pColor == TeamColor.White ? _whiteMaterial : _blackMaterial;
        }
    }
}