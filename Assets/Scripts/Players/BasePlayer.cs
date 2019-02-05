using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasePlayer : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnEndOfTurn = new UnityEvent();

    [SerializeField] protected List<Checker> checkersPool = new List<Checker>();

    public virtual void Init(Board board, UIManager ui)
    {
        this.board = board;
        this.ui = ui;
    }
    
    public virtual void SetupController(CheckerController checkerController)
    {
        if(this.checkerController)
            this.checkerController.OnEndOfStep.RemoveListener(HandleOnEndOfStep);

        this.checkerController = checkerController;
        checkerController.SetupPlayer(checkersPool, uiPlayer);
        checkerController.OnEndOfStep.AddListener(HandleOnEndOfStep);
    }

    public List<Checker> GetCheckersPool()
    {
        return checkersPool;
    }

    public void SetupRow()
    {
        for (int i = 0; i < checkersPool.Count; i++)
        {
            checkersPool[i].SetIsPlayerCanSelect(false);
            checkersPool[i].ResetForces();
            checkersPool[i].transform.position = new Vector3(
                board.GetRowXs()[i], board.GetHeight(), board.GetRowZ(currentRow));
            checkersPool[i].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public UIPlayer GetUIPlayer()
    {
        return uiPlayer;
    }

    public Rows GetCurrentRow()
    {
        return currentRow;
    }

    public void DisableCheckers()
    {
        foreach (var checker in checkersPool)
            checker.Disable();
    }

    public void EnableChekers()
    {
        foreach (var checker in checkersPool)
            checker.Enable();
    }

    public void DisableController()
    {
        checkerController.Disable();
    }

    public void EnableController()
    {
        checkerController.Enable();
    }

    public void ResetUIStrenght()
    {
        uiPlayer.SetStrengthScale(0);
    }

    public void EnableUIYourMove()
    {
        uiPlayer.SetYourMove(true);
    }

    public void DisableUIYourMove()
    {
        uiPlayer.SetYourMove(false);
    }

    public bool GetWinGame()
    {
        return winGame;
    }

    public void ResetWinGame()
    {
        winGame = false;
    }

    #region private

    Board board;
    CheckerController checkerController;
    protected UIManager ui;
    protected Rows currentRow;
    protected UIPlayer uiPlayer;
    protected bool winGame;

    void HandleOnEndOfStep()
    {
        OnEndOfTurn.Invoke();
    }

    #endregion
}