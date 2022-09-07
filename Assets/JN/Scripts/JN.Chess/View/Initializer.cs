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
        public TextMeshProUGUI textGameOver;
        public GameObject panelGameMode;
        public GameObject panelSelectTeam;
        public GameObject panelGameOver;
        public GameObject CameraWhite;
        public GameObject CameraBlack;

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
            CameraWhite.SetActive(true);
            CameraBlack.SetActive(false);

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

        private void OnGameOver(Player activePlayer)
        {
            textGameOver.text = "Player " + activePlayer.team.ToString() + " won!";
            panelGameOver.SetActive(true);
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

            if(ChessGameController.Instance.gameMode == GameMode.Player_Player)
            {
                CameraBlack.SetActive(activePlayer.team == TeamColor.Black ? true : false);
                CameraWhite.SetActive(activePlayer.team == TeamColor.White ? true : false);
            }
        }

        public void SelectGameMode(int val)
        {
            GameMode gMode = (GameMode)val;

            ChessGameController.Instance.SelectGameMode(gMode);
            panelGameMode.SetActive(false);

            switch(gMode)
            {
                case GameMode.Player_AI:
                    panelSelectTeam.SetActive(true);
                    break;

                case GameMode.Player_Player:
                    ChessGameController.Instance.SelectTeam(TeamColor.Both);
                    ChessGameController.Instance.StartGame();
                    break;

                case GameMode.AI_AI:
                    ChessGameController.Instance.SelectTeam(TeamColor.None);
                    ChessGameController.Instance.StartGame();
                    break;
            }
        }

        public void SelectTeam(int val)
        {
            TeamColor team = (TeamColor)val;

            CameraWhite.SetActive(team == TeamColor.White ? true : false);
            CameraBlack.SetActive(team == TeamColor.Black ? true : false);
            
            ChessGameController.Instance.SelectTeam(team);
            panelSelectTeam.SetActive(false);
            
            ChessGameController.Instance.StartGame();
        }

        public void RestartGame()
        {
            ChessGameController.Instance.ResetGame();
            Start();
        }
    }
}