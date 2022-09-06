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

    public class ChessGameController : SingletonMonoBehaviour<ChessGameController>
    {
        public BoardGameData boardGameData;
        public BoardGame boardGame;
        public PieceSpawner pieceSpawner;
        public GameMode gameMode;

        public event Action<Player> OnGameStart;
        public event Action<Player> OnActivePlayerChanged;
        public event Action OnGameOver;

        private Player whitePlayer;
        private Player blackPlayer;
        private Player activePlayer;

        public void Init()
        {
            CreatePlayer();
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
            OnActivePlayerChanged?.Invoke(activePlayer);
        }

        private void GeneratePossiblePlayerMoves(Player player)
        {
            player.GeneratePossibleMoves();
        }

        private void LoadPieceFromData(BoardGameData broadData)
        {
            if (broadData is null)
            {
                throw new ArgumentNullException(nameof(broadData));
            }

            for (int i = 0; i < boardGameData.GetPieceCount(); i++)
            {
                Vector2Int coordinate = broadData.GetPieceCoordinateAtIndex(i);
                TeamColor teamColor = broadData.GetPieceTeamColorAtIndex(i);
                PieceType pieceType = broadData.GetPieceTypeAtIndex(i);

                CreatePieceAndInitialize(coordinate, teamColor, pieceType);
            }
        }

        private void CreatePieceAndInitialize(Vector2Int coords, TeamColor team, PieceType pieceType)
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
            }
        }

        public void StartGame()
        {
            pieceSpawner.Init();
            LoadPieceFromData(boardGameData);

            activePlayer = whitePlayer;
            GeneratePossiblePlayerMoves(activePlayer);

            OnGameStart?.Invoke(activePlayer);
        }

        public void EndTurn()
        {
            GeneratePossiblePlayerMoves(activePlayer);
            GeneratePossiblePlayerMoves(GetOpponent(activePlayer));

            ChangeActivePlayer();
        }

        public void EndGame()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            whitePlayer.playerType = PlayerType.Human;
            blackPlayer.playerType = PlayerType.Human;
        }
    }
}