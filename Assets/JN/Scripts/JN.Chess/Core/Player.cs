using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace JN.Chess
{
    public enum PlayerType
    {
        Human,
        AI,
    }
    
    [System.Serializable]
    public class Player
    {
        public TeamColor team;
        public BoardGame board;
        public PlayerType playerType;
        public List<Piece> listActivePiece {get; private set;}

        public Player(TeamColor team, BoardGame board)
        {
            listActivePiece = new List<Piece>();
            this.board = board;
            this.team = team;

            ChessGameController.Instance.OnActivePlayerChanged += OnActivePlayerChanged;
        }

        public void AddPiece(Piece piece)
        {
            if(listActivePiece.Contains(piece) == false)
                listActivePiece.Add(piece);
            else
                LogError("Piece already inserted " + piece.pieceType);
        }

        public void RemovePiece(Piece piece)
        {
            if(listActivePiece.Contains(piece) == true)
                listActivePiece.Remove(piece);
        }

        public void GeneratePossibleMoves()
        {
            foreach(Piece piece in listActivePiece)
            {
                if(board.HasPiece(piece))
                    piece.GenerateAvailableMove();
            }
        }

        private void OnActivePlayerChanged(Player player)
        {
            if(player != this)
                return;

            if(playerType == PlayerType.AI)
                ChessGameController.Instance.CalculateAIMove();
        }

        public Piece[] GetPieceOfType<T>() where T : Piece
        {
            return listActivePiece.Where(x=>x is T).ToArray();
        }

        void LogError (object message)
        {
            Debug.LogError("[Player]." + message);
        }
    }
}