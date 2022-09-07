using System;
using System.Collections.Generic;
using JN.Utils;
using UnityEngine;

namespace JN.Chess
{
    [RequireComponent(typeof(MaterialSetter))]
    [RequireComponent(typeof(InstantTweener))]
    public abstract class Piece : MonoBehaviour
    {
        [SerializeField] private MaterialSetter materialSetter;
        [SerializeField] private InstantTweener tweener;
        public PieceType pieceType;
        public List<Vector2Int> listOfAvailableMoves;
        
        public BoardGame board { protected get; set; }
        public Vector2Int coordinate { get; set; }
        public TeamColor team { get; set; }
        public bool hasMoved { get; private set; }

        public abstract List<Vector2Int> GenerateAvailableMove();

        public virtual void Move(Vector2Int coords)
        {
            coordinate = coords;
            hasMoved = true;

            Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
            tweener.MoveTo(transform, targetPosition);
        }

        virtual internal void Awake()
        {
            listOfAvailableMoves = new List<Vector2Int>();
        }

        public void SetMaterial(Material selectedMaterial)
        {
            materialSetter.SetSingleMaterial(selectedMaterial);
        }

        public void SetData(Vector2Int coords, TeamColor team, PieceType pieceType, BoardGame board)
        {
            coordinate = coords;
            this.team = team;
            this.board = board;
            this.pieceType = pieceType;
            hasMoved = false;

            float rotationValue = team == TeamColor.White ? 180 : 0;
            transform.SetParent(board.pieceContainer);
            transform.SetPositionAndRotation(board.CalculatePositionFromCoords(coords), Quaternion.Euler(0, rotationValue, 0));
        }

        public bool CanMoveTo(Vector2Int coords)
        {
            return listOfAvailableMoves.Contains(coords);
        }
        
        public bool IsAttackingPieceOfType<T>() where T : Piece
        {
            foreach (var coords in listOfAvailableMoves)
            {
                if (board.GetPieceOnBoard(coords) is T)
                    return true;
            }

            return false;
        }
    }
}
