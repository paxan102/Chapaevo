using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainGame : MonoBehaviour
{
    [SerializeField] Board board;
    [SerializeField] FirstPlayer firstPlayer;
    [SerializeField] SecondPlayer secondPlayer;
    [SerializeField] CheckerControllerUser checkerControllerUser;
    [SerializeField] CheckerControllerAI checkerControllerAI;
    [SerializeField] RuleBase ruleBase;
    [SerializeField] UIManager ui;

    #region private

    const int FIRST_PLAYER = 0;
    const int SECOND_PLAYER = 1;

    private void Start ()
    {
        Physics.gravity = new Vector3(0, -130F, 0);

        ui.Init();
        firstPlayer.Init(board, ui);
        secondPlayer.Init(board, ui);
        ruleBase.Init(ui);

        ruleBase.OnPlayerWin.AddListener(HandleOnWinGame);
        ui.OnChooseFirstPlayer.AddListener(HandleOnClickFirstPlayer);
        ui.OnChooseSecondPlayer.AddListener(HandleOnClickSecondPlayer);
        ui.OnRestartButtonClick.AddListener(HandleOnClickRestart);
    }

    void HandleOnWinGame(int player)
    {
        if (player == FIRST_PLAYER)
            ui.EnableWinScreen(FIRST_PLAYER);
        else
            ui.EnableWinScreen(SECOND_PLAYER);
    }

    void HandleOnClickFirstPlayer()
    {
        firstPlayer.SetupController(checkerControllerUser);
        
        secondPlayer.SetupController(checkerControllerAI);
        checkerControllerAI.SetupEnemyCheckers(firstPlayer.GetCheckersPool());

        ui.ResetPlayers();

        StartGame();
    }

    void HandleOnClickSecondPlayer()
    {
        secondPlayer.SetupController(checkerControllerUser);
        
        firstPlayer.SetupController(checkerControllerAI);
        checkerControllerAI.SetupEnemyCheckers(secondPlayer.GetCheckersPool());
        
        ui.ResetPlayers();

        StartGame();
    }

    void StartGame()
    {
        ruleBase.SetupPlayers(firstPlayer, secondPlayer);
    }

    void HandleOnClickRestart()
    {
        ruleBase.ResetWins();
        ui.EnableChooseScreen();
    }

    #endregion
}
