using System;
using JN.Utils;
using UnityEngine;

namespace JN.Chess
{
    public class ChessGameController : SingletonMonoBehaviour<ChessGameController>
    {
        public BoardGameData boardGameData;
        public BoardGame boardGame;
        public PieceSpawner pieceSpawner;

        private Player whitePlayer;
        private Player blackPlayer;
        private Player activePlayer;

        public void Init()
        {
            CreatePlayer();
            StartNewGame();
        }

        private void StartNewGame()
        {
            pieceSpawner.Init();
            LoadPieceFromData(boardGameData);
            activePlayer = whitePlayer;
            GeneratePossiblePlayerMoves(activePlayer);
        }

        private void GeneratePossiblePlayerMoves(Player player)
        {
            player.GeneratePossibleMoves();
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

        public void CreatePieceAndInitialize(Vector2Int coords, TeamColor team, PieceType pieceType)
        {
            Piece newPiece = pieceSpawner.Spawn(pieceType).GetComponent<Piece>();
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

        public void EndTurn()
        {
            GeneratePossiblePlayerMoves(activePlayer);
            GeneratePossiblePlayerMoves(GetOpponent(activePlayer));
            ChangeActivePlayer();
        }
    }
}