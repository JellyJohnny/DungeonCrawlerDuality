
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AppleIdleState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        
        Debug.Log("idle");
        m.canMove = true;
        m.canRotate = true;

        m.anim.SetBool("isMoving", false);
        m.UpdateButtons(true);
        m.attackButton.interactable = false;

        if(m.currentEnemy != null)
        {
            m.SwitchState(m.attackState);
        }

        foreach (var e in m.enemies)
        {
            RaycastHit hit;
            Vector3 eDir = e.transform.position - m.transform.position;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(m.transform.position, eDir, out hit, Mathf.Infinity, ~m.playerLayer))
            {
                if (hit.distance <= 6)
                {
                    Debug.DrawRay(m.transform.position, eDir * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");
                }
            }

        }

    }

    public override void UpdateState(Movement m)
    {
       
    }

    

}
