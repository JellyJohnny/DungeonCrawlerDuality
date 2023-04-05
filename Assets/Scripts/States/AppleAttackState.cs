
using UnityEngine;

public class AppleAttackState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        Debug.Log("attack");

        m.UpdateButtons(false);
        m.attackButton.interactable = true;
        m.canMove = false;
        Vector3 dir = m.currentEnemy.transform.position - m.transform.position;
        Quaternion lookR = Quaternion.LookRotation(dir);
        m.targetRotation = lookR;
    }

    public override void UpdateState(Movement m)
    {
        if (m.transform.rotation != m.targetRotation)
        {
            m.transform.rotation = Quaternion.Slerp(m.transform.rotation, m.targetRotation, m.turnSpeed * Time.deltaTime);
        }

        if(m.currentEnemy == null)
        {
            m.SwitchState(m.idleState);
        }

        if(m.attackButton.image.fillAmount < 1)
        {
            m.attackButton.image.fillAmount += (0.5f * Time.deltaTime);
        }
        else
        {
            m.attackButton.image.fillAmount = 1;
            m.attackButton.interactable = true;
        }
    }

}
