using System.Collections;
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
        [SerializeField] private SquareHighlightSpawner _highlightSpawner;

        public Transform pieceContainer;
        public const int BOARD_SIZE = 8;

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
            DeselectPiece();

            _selectedPiece = piece;

            Debug.Log("coord highlight " + piece);

            ShowMoveHighlight(piece.listOfAvailableMoves);
        }

        private void ShowMoveHighlight(List<Vector2Int> listSquare)
        {
            Dictionary<Vector3, bool> dictMovePosition = new Dictionary<Vector3, bool>();
            foreach(Vector2Int coords in listSquare)
            {
                Debug.Log("coord highlight " + CalculatePositionFromCoords(coords));
                dictMovePosition.Add(CalculatePositionFromCoords(coords), GetPieceOnBoard(coords) ? false : true);
            }

            _highlightSpawner.ShowSelection(dictMovePosition);
        }

        private void DeselectPiece()
        {
            _selectedPiece = null;

            _highlightSpawner.ClearSelection();
        }

        private void EndTurn()
        {
            ChessGameController.Instance.EndTurn();
        }

        public bool IsCoordinateOnBoard(Vector2Int coords)
        {
            if(coords.x < 0 || coords.y < 0 || coords.x >= BOARD_SIZE || coords.y >= BOARD_SIZE)
                return false;

            return true;
        }

        public Piece GetPieceOnBoard(Vector2Int coords)
        {
            if(IsCoordinateOnBoard(coords))
                return _grid[coords.x, coords.y];

            return null;
        }

        public void SetPieceOnBoard(Vector2Int coords, Piece newPiece)
        {
            if(IsCoordinateOnBoard(coords))
                _grid[coords.x, coords.y] = newPiece;
        }

        public Piece GetPieceInDirection<T>(Vector2Int coords, TeamColor team, Vector2Int direction)
        {
            int i = 1;
            while(true)
            {
                Vector2Int nextCoords = coords + direction * i;
                if (IsCoordinateOnBoard(nextCoords) == false)
                    return null;

                Piece piece = GetPieceOnBoard(nextCoords);

                if (piece != null)
                {
                    Debug.Log("juju " + piece + " " + piece.team);

                    if (piece.team != team || !(piece is T))
                    {
                        return null;
                    }
                    else if (piece.team == team && piece is T)
                    {
                        return piece;
                    }
                }

                i++;
            }
        }

        public void RemovePieceOnBoard(Piece piece)
        {
            if(HasPiece(piece))
            {
                ChessGameController.Instance.OnPieceRemoved(piece);

                _grid[piece.coordinate.x, piece.coordinate.y] = null;
                piece.gameObject.SetActive(false);
            }
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
            if(ChessGameController.Instance.gameState != GameState.HumanTurn)
                return;

            Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
            Piece pieceTouched = GetPieceOnBoard(coords);

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
                        else
                        {
                            if(_selectedPiece.CanMoveTo(coords))
                            {
                                StartCoroutine(OnMovePiece(coords, _selectedPiece));
                            }
                        }
                    }
                }
                else
                {
                    if(_selectedPiece.CanMoveTo(coords))
                    {
                        StartCoroutine(OnMovePiece(coords, _selectedPiece));
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

        public IEnumerator OnMovePiece(Vector2Int coords, Piece piece)
        {
            Piece target = GetPieceOnBoard(coords);
            if(target)
                RemovePieceOnBoard(target);

            UpdateBoardOnMovePiece(coords, piece.coordinate, piece, null);
            piece.Move(coords);

            yield return new WaitForSeconds(0.1f);

            DeselectPiece();
            EndTurn();
        }

        public void PromotePiece(Piece piece)
        {
            RemovePieceOnBoard(piece);
            ChessGameController.Instance.CreatePieceAndInitialize(piece.coordinate, piece.team, PieceType.Queen);
        }

        public void UpdateBoardOnMovePiece(Vector2Int newCoords, Vector2Int oldCoords, Piece newPiece, Piece oldPiece)
        {
            _grid[oldCoords.x, oldCoords.y] = oldPiece;
            _grid[newCoords.x, newCoords.y] = newPiece;
        }

        public bool HasPiece(Piece piece)
        {
            if(piece == null)
                return false;
                
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

        public void ResetBoard()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    Piece piece = GetPieceOnBoard(new Vector2Int(i, j));
                    RemovePieceOnBoard(piece);
                }
            }
        }
        
    }
}