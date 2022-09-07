using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
using JN.Utils;
using UnityEngine;

namespace JN.Chess
{
    public enum GameMode
    {
        Player_Player,
        Player_AI,
        AI_AI
    }

    public enum GameState
    {
        Init,
        HumanTurn,
        AITurn,
        Finished,
    }

    public class ChessGameController : SingletonMonoBehaviour<ChessGameController>
    {
        public BoardGameData boardGameData;
        public BoardGame boardGame;
        public PieceSpawner pieceSpawner;
        public GameMode gameMode;
        public GameState gameState;

        public event Action<Player> OnGameStart;
        public event Action<Player> OnActivePlayerChanged;
        public event Action<Player> OnGameOver;

        private Player whitePlayer;
        private Player blackPlayer;
        private Player activePlayer;

        public void Init()
        {
            SetState(GameState.Init);
            CreatePlayer();
            pieceSpawner.Init();
        }

        private void CreatePlayer()
        {
            whitePlayer = new Player(TeamColor.White, boardGame);
            blackPlayer = new Player(TeamColor.Black, boardGame);
        }

        private Player GetOpponent(Player player)
        {
            return player == whitePlayer ? blackPlayer : whitePlayer;
        }

        private void ChangeActivePlayer()
        {
            activePlayer = activePlayer.team == TeamColor.White ? blackPlayer : whitePlayer;
            UpdatePlayerTurn();
        }

        private void UpdatePlayerTurn()
        {
            if(activePlayer.playerType == PlayerType.Human)
                SetState(GameState.HumanTurn);
            else
                SetState(GameState.AITurn);

            OnActivePlayerChanged?.Invoke(activePlayer);
        }

        private void GeneratePossiblePlayerMoves(Player player)
        {
            player.GeneratePossibleMoves();
        }

        private void LoadPieceFromData(BoardGameData boardData)
        {
            if (boardData is null)
            {
                throw new ArgumentNullException(nameof(boardData));
            }

            for (int i = 0; i < boardGameData.GetPieceCount(); i++)
            {
                Vector2Int coordinate = boardData.GetPieceCoordinateAtIndex(i);
                TeamColor teamColor = boardData.GetPieceTeamColorAtIndex(i);
                PieceType pieceType = boardData.GetPieceTypeAtIndex(i);

                CreatePieceAndInitialize(coordinate, teamColor, pieceType);
            }
        }

        public void CreatePieceAndInitialize(Vector2Int coords, TeamColor team, PieceType pieceType)
        {
            Piece newPiece = pieceSpawner.SpawnPiece(pieceType);
            newPiece.gameObject.SetActive(true);
            newPiece.SetData(coords, team, pieceType, boardGame);

            Material teamMaterial = pieceSpawner.GetPieceMaterial(team);
            newPiece.SetMaterial(teamMaterial);

            boardGame.SetPieceOnBoard(coords, newPiece);

            Player currentPlayer = team == TeamColor.White ? whitePlayer : blackPlayer;
            currentPlayer.AddPiece(newPiece);
        }

        public void OnPieceRemoved(Piece piece)
        {
            Player pieceOwner = (piece.team == TeamColor.White) ? whitePlayer : blackPlayer;
            pieceOwner.RemovePiece(piece);
        }

        public void CalculateAIMove()
        {
            StartCoroutine(CalculateAIRandomMove());
        }

        public IEnumerator CalculateAIRandomMove() 
        {
            List<Piece> listMoveablePiece = new List<Piece>();
            foreach(Piece moveablePiece in activePlayer.listActivePiece)
            {
                if(moveablePiece.listOfAvailableMoves.Count > 0)
                {
                    listMoveablePiece.Add(moveablePiece);
                }
            }

            int indexPiece = UnityEngine.Random.Range(0, listMoveablePiece.Count);
            Piece piece = listMoveablePiece[indexPiece];
            
            int indexMove = UnityEngine.Random.Range(0, piece.listOfAvailableMoves.Count-1);
            Vector2Int coords = piece.listOfAvailableMoves[indexMove];

            StartCoroutine(boardGame.OnMovePiece(coords, piece));

            yield return null;
        }

        private bool IsTheKingCheckMate()
        {
            Player opponent = GetOpponent(activePlayer);
            Piece king = opponent.GetPieceOfType<King>().FirstOrDefault();

            if(king == null)
                return true;

            return false;
        }

        public bool IsCurrentActivePlayerTeam(TeamColor team)
        {
            return activePlayer.team == team;
        }

        public TeamColor ActiveTeam()
        {
            return activePlayer.team;
        }

        public void SelectGameMode(GameMode gMode)
        {
            gameMode = gMode;
        }

        public void SelectTeam (TeamColor type)
        {
            switch(type)
            {
                case TeamColor.White :
                {
                    whitePlayer.playerType = PlayerType.Human;
                    blackPlayer.playerType = PlayerType.AI;
                    break;
                }
                
                case TeamColor.Black :
                {
                    whitePlayer.playerType = PlayerType.AI;
                    blackPlayer.playerType = PlayerType.Human;
                    break;
                }

                case TeamColor.None :
                {
                    whitePlayer.playerType = PlayerType.AI;
                    blackPlayer.playerType = PlayerType.AI;

                    break;
                }

                case TeamColor.Both :
                {
                    whitePlayer.playerType = PlayerType.Human;
                    blackPlayer.playerType = PlayerType.Human;

                    break;
                }
            }
        }

        public void SetState(GameState state)
        {
            gameState = state;
        }

        public void StartGame()
        {
            LoadPieceFromData(boardGameData);

            activePlayer = whitePlayer;
            GeneratePossiblePlayerMoves(activePlayer);
            UpdatePlayerTurn();

            OnGameStart?.Invoke(activePlayer);
        }

        public void EndTurn()
        {
            GeneratePossiblePlayerMoves(activePlayer);
            GeneratePossiblePlayerMoves(GetOpponent(activePlayer));

            if(IsTheKingCheckMate())
                EndGame();
            else
                ChangeActivePlayer();
        }

        public void EndGame()
        {
            SetState(GameState.Finished);
            OnGameOver?.Invoke(activePlayer);
        }

        public void ResetGame()
        {
            boardGame.ResetBoard();
        }
    }
}