using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainGame : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] FirstPlayer firstPlayer;
    [SerializeField] SecondPlayer secondPlayer;
    [SerializeField] CheckerControllerUser checkerControllerUserFirstPlayer;
    [SerializeField] CheckerControllerUser checkerControllerUserSecondPlayer;
    [SerializeField] CheckerControllerAI checkerControllerAIFirstPlayer;
    [SerializeField] CheckerControllerAI checkerControllerAISecondPlayer;
    [SerializeField] UIManager UI;
    
    #region private

    private void Start ()
    {
        Physics.gravity = new Vector3(0, -130F, 0);

        UI.Init();

        UI.OnClickFirstPlayer.AddListener(HandleOnClickFirstPlayer);
        UI.OnClickSecondPlayer.AddListener(HandleOnClickSecondPlayer);
        UI.OnClickRestart.AddListener(HandleOnClickRestart);
    }

    void HandleOnClickFirstPlayer()
    {
        firstPlayerController = checkerControllerUserFirstPlayer;
        secondPlayerController = checkerControllerAISecondPlayer;

        StartGame();
    }

    void HandleOnClickSecondPlayer()
    {
        firstPlayerController = checkerControllerAIFirstPlayer;
        secondPlayerController = checkerControllerUserSecondPlayer;

        StartGame();
    }

    void StartGame()
    {
        UI.ResetUI();

        firstPlayer.Init(board, firstPlayerController, UI);
        secondPlayer.Init(board, secondPlayerController, UI);

        firstPlayerController.gameObject.SetActive(false);
        secondPlayerController.gameObject.SetActive(false);

        firstPlayerController.SetEnemyCheckersPool(
            GameObject.FindGameObjectWithTag(SECOND_PLAYER_TAG).GetComponent<SecondPlayer>().GetCheckersPool());

        secondPlayerController.SetEnemyCheckersPool(
            GameObject.FindGameObjectWithTag(FIRST_PLAYER_TAG).GetComponent<FirstPlayer>().GetCheckersPool());

        firstPlayerController.OnEndOfStep.AddListener(HandleEndOfStepFirstPlayer);
        secondPlayerController.OnEndOfStep.AddListener(HandleEndOfStepSecondPlayer);

        firstPlayer.OnLoseRound.AddListener(HandleLoseRoundFirstPlayer);
        secondPlayer.OnLoseRound.AddListener(HandleLoseRoundSecondPlayer);

        UI.firstPlayerYourMove.gameObject.SetActive(true);
        firstPlayerController.Enable();
    }
    
    protected const string FIRST_PLAYER_TAG = "first player";
    protected const string SECOND_PLAYER_TAG = "second player";

    CheckerController firstPlayerController;
    CheckerController secondPlayerController;

    int firstPlayerWins = 0;
    int secondPlayersWins = 0;

    void HandleOnClickRestart()
    {
        firstPlayerWins = 0;
        secondPlayersWins = 0;

        UI.chooseScreen.gameObject.SetActive(true);
    }

    void HandleEndOfStepFirstPlayer()
    {
        secondPlayerController.Enable();
        UI.secondPlayerYourMove.gameObject.SetActive(true);
        UI.firstPlayerYourMove.gameObject.SetActive(false);
    }

    void HandleEndOfStepSecondPlayer()
    {
        firstPlayerController.Enable();
        UI.firstPlayerYourMove.gameObject.SetActive(true);
        UI.secondPlayerYourMove.gameObject.SetActive(false);
    }

    void HandleLoseRoundFirstPlayer()
    {
        secondPlayerController.gameObject.SetActive(false);

        if (!(secondPlayerController.GetAlliesCounter() == firstPlayerController.GetAlliesCounter()))
            secondPlayer.Forward();

        if (secondPlayer.GetWin())
        {
            UI.WinScreenOpen(TypeOfPlayer.SECOND);
            return;
        }

        UI.ResetUI();

        if (secondPlayer.GetCurrentRow() == firstPlayer.GetCurrentRow())
            firstPlayer.Back();

        SetupBothRows();

        secondPlayersWins += 1;
        UI.secondPlayerWins.text = secondPlayersWins.ToString();

        secondPlayerController.Enable();
    }

    void HandleLoseRoundSecondPlayer()
    {
        firstPlayerController.gameObject.SetActive(false);
        
        if (!(firstPlayerController.GetAlliesCounter() == secondPlayerController.GetAlliesCounter()))
            firstPlayer.Forward();

        if (firstPlayer.GetWin())
        {
            UI.WinScreenOpen(TypeOfPlayer.FIRST);
            return;
        }

        UI.ResetUI();

        if (firstPlayer.GetCurrentRow() == secondPlayer.GetCurrentRow())
            secondPlayer.Back();

        SetupBothRows();

        firstPlayerWins += 1;
        UI.firstPlayerWins.text = firstPlayerWins.ToString();

        firstPlayerController.Enable();
    }

    void SetupBothRows()
    {
        firstPlayer.SetupRow();
        firstPlayer.EnableChekers();
        secondPlayer.SetupRow();
        secondPlayer.EnableChekers();
    }

    #endregion
}
