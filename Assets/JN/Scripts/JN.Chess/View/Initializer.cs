using System;
using TMPro;
using UnityEngine;

namespace JN.Chess
{
    public class Initializer : MonoBehaviour
    {
        public BoardGameData boardGameData;
        public BoardGame boardGame;
        public PieceSpawner pieceSpawner;
        public TextMeshProUGUI textTurn;

        void Awake()
        {
            ChessGameController.Instance.boardGameData = this.boardGameData;
            ChessGameController.Instance.boardGame = this.boardGame;
            ChessGameController.Instance.pieceSpawner = this.pieceSpawner;
            ChessGameController.Instance.Init();
        }

        void OnEnable()
        {
            ChessGameController.Instance.OnActivePlayerChanged += UpdateActivePlayer;
        }

        void OnDisable()
        {
            if(ChessGameController.Instance != null)
                ChessGameController.Instance.OnActivePlayerChanged -= UpdateActivePlayer;
        }

        void Start()
        {
            UpdateActivePlayer();
        }

        private void UpdateActivePlayer()
        {
            textTurn.text = ChessGameController.Instance.ActiveTeam().ToString() + " Turn";
        }
    }
}