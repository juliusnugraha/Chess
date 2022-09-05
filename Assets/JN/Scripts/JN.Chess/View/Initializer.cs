using UnityEngine;

namespace JN.Chess
{
    public class Initializer : MonoBehaviour
    {
        public BoardGameData boardGameData;
        public BoardGame boardGame;
        public PieceSpawner pieceSpawner;

        void Awake()
        {
            ChessGameController.Instance.boardGameData = this.boardGameData;
            ChessGameController.Instance.boardGame = this.boardGame;
            ChessGameController.Instance.pieceSpawner = this.pieceSpawner;
            ChessGameController.Instance.Init();
        }
    }
}