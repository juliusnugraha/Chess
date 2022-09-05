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

        public BoardGame board { protected get; set; }
        public Vector2Int coordinate { get; set; }
        public TeamColor team { get; set; }
        public bool hasMoved { get; private set; }
        public List<Vector2Int> avaliableMoves;
        public abstract List<Vector2Int> listOfAvailableMove();

        public virtual void Move(Vector2Int coords)
        {
            coordinate = coords;
            hasMoved = true;

            Vector3 targetPosition = board.CalculatePositionFromCoords(coords);
            tweener.MoveTo(transform, targetPosition);
        }

        virtual internal void Awake()
        {
            avaliableMoves = new List<Vector2Int>();
            hasMoved = false;
        }

        protected void TryToAddMove(Vector2Int coords)
        {
            avaliableMoves.Add(coords);
        }

        public void SetMaterial(Material selectedMaterial)
        {
            materialSetter.SetSingleMaterial(selectedMaterial);
        }

        public bool IsFromSameTeam(Piece piece)
        {
            return team == piece.team;
        }

        public bool CanMoveTo(Vector2Int coords)
        {
            return avaliableMoves.Contains(coords);
        }
        
        public void SetData(Vector2Int coords, TeamColor team, PieceType pieceType, BoardGame board)
        {
            coordinate = coords;
            this.team = team;
            this.board = board;
            this.pieceType = pieceType;

            float rotationValue = team == TeamColor.White ? 180 : 0;
            transform.SetParent(board.pieceContainer);
            transform.SetPositionAndRotation(board.CalculatePositionFromCoords(coords), Quaternion.Euler(0, rotationValue, 0));
        }

        public bool IsAttackingPieceOfType<T>() where T : Piece
        {
            foreach (var square in avaliableMoves)
            {
                // if (board.GetPieceOnSquare(square) is T)
                //     return true;
            }
            return false;
        }
    }
}
