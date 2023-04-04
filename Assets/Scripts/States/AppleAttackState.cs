
using UnityEngine;

public class AppleAttackState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        Debug.Log("attack");

        m.UpdateButtons(false);
        m.attackButton.interactable = true;
        m.canMove = false;
    }

    public override void UpdateState(Movement m)
    {

    }

}
