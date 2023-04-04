using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AppleTurnState : AppleBaseState
{
    public override void EnterState(Movement m)
    {

        Debug.Log("turn");
        m.canMove = false;
        m.canRotate = false;


        m.UpdateButtons(false);
        m.transform.position = m.targetPosition;

        Quaternion lr = m.transform.rotation * Quaternion.Euler(0, m.turnDirection, 0);
        m.targetRotation = lr;
    }

    public override void UpdateState(Movement m)
    {
        if (m.transform.rotation != m.targetRotation)
        {
            m.transform.rotation = Quaternion.Slerp(m.transform.rotation, m.targetRotation, m.turnSpeed * Time.deltaTime);
        }
        else
        {
            m.SwitchState(m.idleState);
        }
    }
}
