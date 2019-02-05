using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnChooseFirstPlayer = new UnityEvent();
    [HideInInspector] public UnityEvent OnChooseSecondPlayer = new UnityEvent();
    [HideInInspector] public UnityEvent OnRestartButtonClick = new UnityEvent();

    [SerializeField] UIPlayer firstPlayer;
    [SerializeField] UIPlayer secondPlayer;

    [SerializeField] UIScreen firstScreen;
    [SerializeField] UIScreen secondScreen;

    [SerializeField] UIWinScreen winScreen;
    [SerializeField] UISelectPlayers selectPlayersScreen;

    public UIPlayer GetFirstPlayer()
    {
        return firstPlayer;
    }

    public UIPlayer GetSecondPlayer()
    {
        return secondPlayer;
    }

    public void Init()
    {
        winScreen.gameObject.SetActive(false);
        secondScreen.gameObject.SetActive(false);
        selectPlayersScreen.gameObject.SetActive(false);

        firstScreen.Init();
        secondScreen.Init();
        firstPlayer.Init();
        secondPlayer.Init();
        winScreen.Init();
        selectPlayersScreen.Init();

        firstScreen.OnClickButton.AddListener(HandleFirstScreenButtonClick);
        secondScreen.OnClickButton.AddListener(HandleSecondScreenButtonClick);

        selectPlayersScreen.OnClickFirstPlayerButton.AddListener(HandleFirstPlayerButtonChoose);
        selectPlayersScreen.OnClickSecondPlayerButton.AddListener(HandleSecondPlayerButtonChoose);

        winScreen.OnClickRestart.AddListener(HandleClickRestart);
    }

    public void HandleClickRestart()
    {
        winScreen.gameObject.SetActive(false);
        OnRestartButtonClick.Invoke();
    }

    public void SetAliveCheckers(int first, int second)
    {
        firstPlayer.SetCountOfAliveCheckers(first);
        secondPlayer.SetCountOfAliveCheckers(second);
    }

    public void SetWins(int first, int second)
    {
        firstPlayer.SetPlayerWins(first);
        secondPlayer.SetPlayerWins(second);
    }

    public void EnableChooseScreen()
    {
        selectPlayersScreen.gameObject.SetActive(true);
    }

    public void EnableWinScreen(int player)
    {
        winScreen.WinScreenOpen(player);
    }

    public void ResetPlayers()
    {
        firstPlayer.ResetPlayer();
        secondPlayer.ResetPlayer();
    }

    #region private

    const string FIRST_PLAYER_WIN = "1 PLAYER WIN";
    const string SECOND_PLAYER_WIN = "2 PLAYER WIN";

    void HandleFirstScreenButtonClick()
    {
        firstScreen.Disable();
        secondScreen.Enable();
    }

    void HandleSecondScreenButtonClick()
    {
        secondScreen.Disable();
        selectPlayersScreen.Enable();
    }

    void HandleFirstPlayerButtonChoose()
    {
        selectPlayersScreen.Disable();
        OnChooseFirstPlayer.Invoke();
    }

    void HandleSecondPlayerButtonChoose()
    {
        selectPlayersScreen.Disable();
        OnChooseSecondPlayer.Invoke();
    }

    #endregion
}
