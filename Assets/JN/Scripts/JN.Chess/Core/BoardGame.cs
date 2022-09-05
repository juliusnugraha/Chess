using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace JN.Chess
{
    public class BoardGame : MonoBehaviour
    {
        [SerializeField] private Transform _bottomLeftSquareTransform;
        [SerializeField] private float _squareSize;
        public const int BOARD_SIZE = 8;
        public Transform pieceContainer;
        private Piece[,] _grid;
        private Piece _selectedPiece;

        void Awake()
        {
            CreateGrid();
        }
        
        void CreateGrid()
        {
            _grid = new Piece[BOARD_SIZE, BOARD_SIZE];
        }

        private void SelectPiece(Piece piece)
        {
            _selectedPiece = piece;
        }

        private void DeselectPiece()
        {
            _selectedPiece = null;
        }

        private Piece GetPieceOnSquare(Vector2Int coords)
        {
            if(IsCoordinateOnBoard(coords))
                return _grid[coords.x, coords.y];

            return null;
        }

        internal void SetPieceOnBoard(Vector2Int coords, Piece newPiece)
        {
            if(IsCoordinateOnBoard(coords))
                _grid[coords.x, coords.y] = newPiece;
        }

        private bool IsCoordinateOnBoard(Vector2Int coords)
        {
            if(coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
                return false;

            return true;
        }

        private void EndTurn()
        {
            ChessGameController.Instance.EndTurn();
        }

        public Vector3 CalculatePositionFromCoords(Vector2Int coords)
        {
            return _bottomLeftSquareTransform.position + new Vector3(coords.x * _squareSize, 0f, coords.y * _squareSize);
        }

        public Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
        {
            int x = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x / _squareSize) + BOARD_SIZE / 2;
            int y = Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z / _squareSize) + BOARD_SIZE / 2;
            return new Vector2Int(x, y);
        }

        public void OnSquareSelected(Vector3 inputPosition)
        {
            Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
            Piece pieceTouched = GetPieceOnSquare(coords);

            Debug.Log("piece coord " + coords);

            if(_selectedPiece != null)
            {
                if(pieceTouched != null)
                {
                    Debug.Log("piece touched " + pieceTouched.pieceType + pieceTouched.team);

                    if(_selectedPiece == pieceTouched)
                    {
                        DeselectPiece();
                    }
                    else
                    {
                        if(ChessGameController.Instance.IsCurrentActivePlayerTeam(pieceTouched.team))
                        {
                            SelectPiece(pieceTouched);
                        }
                    }
                }
                else
                {
                    if(_selectedPiece.CanMoveTo(coords))
                    {
                        OnMovePiece(coords, _selectedPiece);
                    }
                }
            }
            else
            {
                if(pieceTouched != null)
                {
                    Debug.Log("piece touched " + pieceTouched.pieceType + pieceTouched.team);

                    if(ChessGameController.Instance.IsCurrentActivePlayerTeam(pieceTouched.team))
                    {
                        SelectPiece(pieceTouched);
                    }

                }
            }
        }

        public void OnMovePiece(Vector2Int coords, Piece piece)
        {
            UpdateBoardOnMovePiece(coords, piece.coordinate, piece, null);
            _selectedPiece.Move(coords);
            DeselectPiece();
            EndTurn();
        }

        public void UpdateBoardOnMovePiece(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
        {
            _grid[oldCoords.x, oldCoords.y] = oldPiece;
            _grid[newCoords.x, newCoords.y] = newPiece;
        }

        public bool HasPiece(Piece piece)
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    if(_grid[i,j] == piece)
                        return true;
                }
            }

            return false;
        }
    }
}