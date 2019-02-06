using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : BasePlayer
{    
    public override void Init(Board board, UIManager ui)
    {
        uiPlayer = ui.GetFirstPlayer();

        ResetCurrentRow();
        foreach (var checker in checkersPool)
        {
            checker.Init(TypeOfCheckers.WHITE);
        }

        base.Init(board, ui);       
    }

    public void ResetCurrentRow()
    {
        //currentRow = Rows.SEVENTH_ROW;
        currentRow = Rows.FIRST_ROW;
    }

    public void Back()
    {
        currentRow--;
    }

    public void Forward()
    {
        currentRow++;
        if (currentRow == Rows.EIGHTH_ROW)
            winGame = true;
    }
}
