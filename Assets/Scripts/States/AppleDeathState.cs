using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleDeathState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        m.anim.SetBool("isDead", true);
        m.UpdateButtons(false);
        m.attackButton.interactable = false;
        m.StartCoroutine(m.DisplayRestartScreen());
    }

    public override void UpdateState(Movement m)
    {

    }
}
