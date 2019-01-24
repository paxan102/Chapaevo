using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasePlayer : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnLoseRound = new UnityEvent();
    [HideInInspector] public UnityEvent OnWinRound = new UnityEvent();
    [SerializeField] List<Checker> checkersPool = new List<Checker>();

    public virtual void Init(Board board, CheckerController checkerController, UIManager UI)
    {
        foreach (var checker in checkersPool)
        {
            if (typeOfPlayer == TypeOfPlayer.FIRST)
                checker.Init(TypeOfCheckers.WHITE);
            else
                checker.Init(TypeOfCheckers.BLACK);
        }

        this.board = board;
        
        checkerController.Init(checkersPool, UI, typeOfPlayer);

        SetupRow();

        checkerController.OnLoseRound.AddListener(HandleOnLose);
        checkerController.OnWinRound.AddListener(HandleOnWin);
    }

    public List<Checker> GetCheckersPool()
    {
        return checkersPool;
    }

    public void SetupRow()
    {
        for (int i = 0; i < checkersPool.Count; i++)
        {
            checkersPool[i].SetIsCanSelect(false);
            checkersPool[i].ResetForces();
            checkersPool[i].transform.position = new Vector3(
                board.GetRowXs()[i], board.GetHeight(), board.GetRowZ(currentRow));
            checkersPool[i].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public Rows GetCurrentRow()
    {
        return currentRow;
    }

    public void DisableCheckers()
    {
        foreach (var checker in checkersPool)
            checker.gameObject.SetActive(false);
    }

    public void EnableChekers()
    {
        foreach (var checker in checkersPool)
            checker.gameObject.SetActive(true);
    }

    public bool GetWin()
    {
        return win;
    }

    #region private

    Board board;
    protected Rows currentRow;
    protected TypeOfPlayer typeOfPlayer;
    protected bool win = false;

    void HandleOnLose()
    {
        OnLoseRound.Invoke();
    }

    void HandleOnWin()
    {
        OnWinRound.Invoke();
    }

    #endregion
}

public enum TypeOfPlayer
{
    FIRST,
    SECOND
}