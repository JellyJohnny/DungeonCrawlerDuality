
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AppleMoveState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        Debug.Log("move");

        m.anim.SetBool("isMoving", true);
        m.targetPosition = m.wallCheck.position;
        m.anim.speed = 2;
        m.UpdateButtons(false);
        m.canMove = false;
        m.StartCoroutine(m.PlayFootStep());
        m.uiAud.Play();
    }

    

    public override void UpdateState(Movement m)
    {
        if (m.MoveTowards())
        {
            m.anim.speed = Mathf.Lerp(m.anim.speed, 0, m.moveSpeed * Time.deltaTime);
            m.transform.position = Vector3.Lerp(m.transform.position, m.targetPosition, m.moveSpeed * Time.deltaTime);
        }
        else
        {
            m.transform.position = m.targetPosition;
            m.SwitchState(m.idleState);
        }
    }
}
