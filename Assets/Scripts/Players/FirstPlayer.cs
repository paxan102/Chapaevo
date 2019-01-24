using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : BasePlayer
{    
    public override void Init(Board board, CheckerController checkerController, UIManager UI)
    {
        currentRow = Rows.FIRST_ROW;
        typeOfPlayer = TypeOfPlayer.FIRST;

        base.Init(board, checkerController, UI);       
    }
    
    public void Back()
    {
        currentRow--;
    }

    public void Forward()
    {
        currentRow++;
        if (currentRow == Rows.EIGHTH_ROW)
            win = true;
    }
}
