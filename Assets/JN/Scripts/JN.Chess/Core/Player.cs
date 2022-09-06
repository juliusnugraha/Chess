using System.Collections.Generic;
using UnityEngine;

namespace JN.Chess
{
    [System.Serializable]
    public class Player
    {
        public TeamColor team;
        public BoardGame board;
        public List<Piece> listActivePiece {get; private set;}

        public Player(TeamColor team, BoardGame board)
        {
            listActivePiece = new List<Piece>();
            this.board = board;
            this.team = team;
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

        void LogError (object message)
        {
            Debug.LogError("[Player]." + message);
        }
    }
}