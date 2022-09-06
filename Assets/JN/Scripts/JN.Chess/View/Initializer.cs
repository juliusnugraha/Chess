using System;
using JN.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JN.Chess
{
    public class Initializer : MonoBehaviour
    {
        public BoardGameData boardGameData;
        public BoardGame boardGame;
        public PieceSpawner pieceSpawner;
        public TextMeshProUGUI textTurn;
        public GameObject panelGameMode;
        public GameObject panelSelectTeam;
        public GameObject panelGameOver;

        void Awake()
        {
            ChessGameController.Instance.boardGameData = this.boardGameData;
            ChessGameController.Instance.boardGame = this.boardGame;
            ChessGameController.Instance.pieceSpawner = this.pieceSpawner;
            ChessGameController.Instance.Init();
        }

        void OnEnable()
        {
            ChessGameController.Instance.OnGameStart += OnGameStart;
            ChessGameController.Instance.OnGameOver += OnGameOver;
            ChessGameController.Instance.OnActivePlayerChanged += OnActivePlayerChanged;
        }

        void OnDisable()
        {
            if(ChessGameController.Instance != null)
            {
                ChessGameController.Instance.OnGameStart -= OnGameStart;
                ChessGameController.Instance.OnGameOver -= OnGameOver;
                ChessGameController.Instance.OnActivePlayerChanged -= OnActivePlayerChanged;
            }
        }

        void Start()
        {
            HideAllPanel();
            panelGameMode.SetActive(true);
        }

        private void HideAllPanel()
        {
            panelGameMode.SetActive(false);
            panelSelectTeam.SetActive(false);
            panelGameOver.SetActive(false);
        }

        private void OnGameStart(Player activePlayer)
        {
            OnActivePlayerChanged(activePlayer);
        }

        private void OnGameOver()
        {

        }

        private void OnActivePlayerChanged(Player activePlayer)
        {
            string message = "";

            if(ChessGameController.Instance.gameMode == GameMode.Player_AI)
            {
                if(activePlayer.playerType == PlayerType.Human)
                {
                    message = "Your Turn";
                }
            }
            else
            {
                message = ChessGameController.Instance.ActiveTeam().ToString() + " Turn";
            }

            textTurn.text = message;
        }

        public void SelectGameMode(int val)
        {
            GameMode gMode = (GameMode)val;

            ChessGameController.Instance.SelectGameMode(gMode);
            panelGameMode.SetActive(false);
            if(gMode == GameMode.Player_AI)
            {
                panelSelectTeam.SetActive(true);
            }
            else
            {
                ChessGameController.Instance.StartGame();
            }
        }

        public void SelectTeam(int val)
        {
            TeamColor team = (TeamColor)val;
            
            ChessGameController.Instance.SelectTeam(team);
            panelSelectTeam.SetActive(false);
            
            ChessGameController.Instance.StartGame();
        }
    }
}