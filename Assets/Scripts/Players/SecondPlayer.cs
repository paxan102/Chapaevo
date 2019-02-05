using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayer : BasePlayer
{
    public override void Init(Board board, UIManager ui)
    {
        uiPlayer = ui.GetSecondPlayer();

        ResetCurrentRow();
        foreach (var checker in checkersPool)
        {
            checker.Init(TypeOfCheckers.BLACK);
        }

        base.Init(board, ui);
    }

    public void ResetCurrentRow()
    {
        currentRow = Rows.EIGHTH_ROW;
    }

    public void Back()
    {
        currentRow++;
    }

    public void Forward()
    {
        currentRow--;
        if (currentRow == Rows.FIRST_ROW)
            winGame = true;
    }
}
