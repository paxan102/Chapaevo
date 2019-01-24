using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayer : BasePlayer
{
    public override void Init(Board board, CheckerController checkerController, UIManager UI)
    {
        currentRow = Rows.EIGHTH_ROW;
        typeOfPlayer = TypeOfPlayer.SECOND;

        base.Init(board, checkerController, UI);
    }

    public void Back()
    {
        currentRow++;
    }

    public void Forward()
    {
        currentRow--;
        if (currentRow == Rows.FIRST_ROW)
            win = true;
    }
}
