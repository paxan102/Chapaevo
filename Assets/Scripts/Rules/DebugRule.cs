using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRule : RuleBase {

    protected override void CheckShouldFirstPlayerEndTurn()
    {
        if (!IsAllStopped())
        {
            Invoke(CHECK_SHOULD_FIRST_PLAYER_END_TURN, 1f);
            return;
        }

        SetupBothRows();
        EnableFirstPlayer();
    }

}
