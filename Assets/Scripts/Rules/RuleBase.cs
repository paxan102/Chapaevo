using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnPlayerWin : UnityEvent<int> { }

public class RuleBase : MonoBehaviour {

    public EventOnPlayerWin OnPlayerWin = new EventOnPlayerWin();

    public void Init(UIManager ui)
    {
        this.ui = ui;
    }

    public void SetupPlayers(FirstPlayer firstPlayer, SecondPlayer secondPlayer)
    {
        this.firstPlayer = firstPlayer;
        this.secondPlayer = secondPlayer;

        firstPlayer.DisableController();
        secondPlayer.DisableController();

        ResetWins();
        SubscribeOnEvents();
        SetupBothRows();

        EnableFirstPlayer();
    }

    public void ResetWins()
    {
        countOfFirstPlayerWinsRounds = 0;
        countOfSecondPlayerWinsRounds = 0;
    }

    #region private

    protected const string IS_ALL_STOPPED = "IsAllStopped";
    protected const string CHECK_SHOULD_FIRST_PLAYER_END_TURN = "CheckShouldFirstPlayerEndTurn";
    protected const string CHECK_SHOULD_SECOND_PLAYER_END_TURN = "CheckShouldSecondPlayerEndTurn";
    const int FIRST_PLAYER = 0;
    const int SECOND_PLAYER = 1;
    
    FirstPlayer firstPlayer;    
    SecondPlayer secondPlayer;
    UIManager ui;
    int countOfFirstPlayerWinsRounds = 0;
    int countOfSecondPlayerWinsRounds = 0;
    int countOfFirstPlayersCheckers = 0;
    int countOfSecondPlayerCheckers = 0;
    bool isFirstPlayerLoseCheckers = false;
    bool isSecondPlayerLoseCheckers = false;
    bool firstPlayerWinRound = false;
    bool secondPlayerWinRound = false;

    void ResetLoseCheckers()
    {
        isFirstPlayerLoseCheckers = false;
        isSecondPlayerLoseCheckers = false;
    }

    void ResetCharacteristics()
    {        
        countOfFirstPlayersCheckers = 0;
        countOfSecondPlayerCheckers = 0;
        isFirstPlayerLoseCheckers = false;
        isSecondPlayerLoseCheckers = false;
        firstPlayerWinRound = false;
        secondPlayerWinRound = false;
    }

    void HandleOnFirstPlayerEndOfTurn()
    {
        Invoke(CHECK_SHOULD_FIRST_PLAYER_END_TURN, 1.5f);
    }

    void HandleOnSecondPlayerEndOfTurn()
    {
        Invoke(CHECK_SHOULD_SECOND_PLAYER_END_TURN, 1.5f);
    }

    protected virtual void CheckShouldFirstPlayerEndTurn()
    {
        if (!IsAllStopped())
        {
            Invoke(CHECK_SHOULD_FIRST_PLAYER_END_TURN, 1f);
            return;
        }

        if (IsSomebodyWinRound(FIRST_PLAYER))
        {
            NextRound();
            return;
        }

        if (!isFirstPlayerLoseCheckers && isSecondPlayerLoseCheckers)
        {
            EnableFirstPlayer();
            return;
        }

        EnableSecondPlayer();
    }

    protected virtual void CheckShouldSecondPlayerEndTurn()
    {
        if (!IsAllStopped())
        {
            Invoke(CHECK_SHOULD_SECOND_PLAYER_END_TURN, 1f);
            return;
        }

        if (IsSomebodyWinRound(SECOND_PLAYER))
        {
            NextRound();
            return;
        }

        if (!isSecondPlayerLoseCheckers && isFirstPlayerLoseCheckers)
        {
            EnableSecondPlayer();
            return;
        }

        EnableFirstPlayer();
    }

    protected void NextRound()
    {      
        if (firstPlayerWinRound)
        {
            firstPlayer.Forward();
            if (firstPlayer.GetWinGame())
            {
                firstPlayer.ResetWinGame();
                firstPlayer.ResetCurrentRow();
                secondPlayer.ResetCurrentRow();
                UnsubscribeOnEvents();
                OnPlayerWin.Invoke(FIRST_PLAYER);
                return;
            }

            if (firstPlayer.GetCurrentRow() == secondPlayer.GetCurrentRow())
                secondPlayer.Back();

            countOfFirstPlayerWinsRounds++;
            SetupBothRows();
            EnableFirstPlayer();
        }

        if (secondPlayerWinRound)
        {
            secondPlayer.Forward();
            if (secondPlayer.GetWinGame())
            {
                secondPlayer.ResetWinGame();
                firstPlayer.ResetCurrentRow();
                secondPlayer.ResetCurrentRow();
                UnsubscribeOnEvents();
                OnPlayerWin.Invoke(SECOND_PLAYER);
                return;
            }

            if (firstPlayer.GetCurrentRow() == secondPlayer.GetCurrentRow())
                firstPlayer.Back();

            countOfSecondPlayerWinsRounds++;
            SetupBothRows();
            EnableSecondPlayer();
        }
    }

    protected bool IsSomebodyWinRound(int whoEndTurn)
    {
        if (countOfFirstPlayersCheckers == 0 && countOfSecondPlayerCheckers == 0)
        {
            SetupBothRows();

            if(whoEndTurn == FIRST_PLAYER)
                isFirstPlayerLoseCheckers = true;
            else
                isSecondPlayerLoseCheckers = true;

            return false;
        }

        if (countOfFirstPlayersCheckers == 0)
        {
            secondPlayerWinRound = true;
            return true;
        }

        if (countOfSecondPlayerCheckers == 0)
        {
            firstPlayerWinRound = true;
            return true;
        }

        return false;
    }

    protected bool IsAllStopped()
    {
        foreach(var checker in firstPlayer.GetCheckersPool())
        {
            Debug.Log(checker.GetVelocity().magnitude);
            if (checker.GetVelocity().magnitude > 0.01f)
            {
                return false;
            }
        }

        foreach (var checker in secondPlayer.GetCheckersPool())
        {
            Debug.Log(checker.GetVelocity().magnitude);
            if (checker.GetVelocity().magnitude > 0.01f)
            {
                return false;
            }
        }

        return true;
    }

    void HandleOnDeadCheckerInFirstPlayer()
    {
        countOfFirstPlayersCheckers--;
        isFirstPlayerLoseCheckers = true;
        firstPlayer.GetUIPlayer().SetCountOfAliveCheckers(countOfFirstPlayersCheckers);
    }

    void HandleOnDeadCheckerInSecondPlayer()
    {
        countOfSecondPlayerCheckers--;
        isSecondPlayerLoseCheckers = true;
        secondPlayer.GetUIPlayer().SetCountOfAliveCheckers(countOfSecondPlayerCheckers);
    }

    void HandleOnRespawnCheckerInFirstPlayer()
    {
        countOfFirstPlayersCheckers++;
        firstPlayer.GetUIPlayer().SetCountOfAliveCheckers(countOfFirstPlayersCheckers);
    }

    void HandleOnRespawnCheckerInSecondPlayer()
    {
        countOfSecondPlayerCheckers++;
        secondPlayer.GetUIPlayer().SetCountOfAliveCheckers(countOfSecondPlayerCheckers);
    }
    
    void SubscribeOnEvents()
    {
        firstPlayer.OnEndOfTurn.AddListener(HandleOnFirstPlayerEndOfTurn);
        secondPlayer.OnEndOfTurn.AddListener(HandleOnSecondPlayerEndOfTurn);

        foreach (var checker in firstPlayer.GetCheckersPool())
        {
            checker.OnDead.AddListener(HandleOnDeadCheckerInFirstPlayer);
            checker.OnRespawn.AddListener(HandleOnRespawnCheckerInFirstPlayer);
        }

        foreach (var checker in secondPlayer.GetCheckersPool())
        {
            checker.OnDead.AddListener(HandleOnDeadCheckerInSecondPlayer);
            checker.OnRespawn.AddListener(HandleOnRespawnCheckerInSecondPlayer);
        }
    }

    void UnsubscribeOnEvents()
    {
        foreach (var checker in firstPlayer.GetCheckersPool())
        {
            checker.OnDead.RemoveListener(HandleOnDeadCheckerInFirstPlayer);
            checker.OnRespawn.RemoveListener(HandleOnRespawnCheckerInFirstPlayer);
        }

        foreach (var checker in secondPlayer.GetCheckersPool())
        {
            checker.OnDead.RemoveListener(HandleOnDeadCheckerInSecondPlayer);
            checker.OnRespawn.RemoveListener(HandleOnRespawnCheckerInSecondPlayer);
        }
        
        firstPlayer.OnEndOfTurn.RemoveListener(HandleOnFirstPlayerEndOfTurn);
        secondPlayer.OnEndOfTurn.RemoveListener(HandleOnSecondPlayerEndOfTurn);
    }

    protected void EnableFirstPlayer()
    {
        ResetLoseCheckers();

        secondPlayer.DisableUIYourMove();
        secondPlayer.ResetUIStrenght();

        firstPlayer.ResetUIStrenght();
        firstPlayer.EnableUIYourMove();
        firstPlayer.EnableController();
    }

    protected void EnableSecondPlayer()
    {
        ResetLoseCheckers();

        firstPlayer.DisableUIYourMove();
        firstPlayer.ResetUIStrenght();

        secondPlayer.ResetUIStrenght();
        secondPlayer.EnableUIYourMove();
        secondPlayer.EnableController();
    }
    
    protected void SetupBothRows()
    {
        ResetCharacteristics();

        firstPlayer.SetupRow();
        firstPlayer.EnableChekers();
        secondPlayer.SetupRow();
        secondPlayer.EnableChekers();

        ui.SetAliveCheckers(countOfFirstPlayersCheckers, countOfSecondPlayerCheckers);
        ui.SetWins(countOfFirstPlayerWinsRounds, countOfSecondPlayerWinsRounds);
    }

    #endregion
}
