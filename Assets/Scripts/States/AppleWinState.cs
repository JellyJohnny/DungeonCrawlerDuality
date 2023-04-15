using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleWinState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        m.winScreen.SetActive(true);
    }

    public override void UpdateState(Movement m)
    {

    }
}
