
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class AppleIdleState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        
        Debug.Log("idle");
        m.canMove = true;
        m.canRotate = true;

        m.anim.SetBool("isMoving", false);
        m.UpdateButtons(true);
        

        m.attackButton.image.fillAmount = 1;
        m.attackButton.interactable = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var e in enemies)
        {
            if(!Physics.Linecast(m.transform.position,e.transform.position))
            {
                float dist = Vector3.Distance(e.transform.position, m.transform.position);

                if (dist <= 6)
                {
                    m.currentEnemy = e.GetComponent<Enemy>();
                    break;
                }
            }
        }

        if (m.currentEnemy != null)
        {
            m.SwitchState(m.attackState);
        }

        /*
        for (int i = 0; i < PropSpawner.Instance.enemyCount-1; i++)
        {
            RaycastHit hit;
            Vector3 dir = PropSpawner.Instance.enemies[i].transform.position - m.transform.position;

            if (Physics.Raycast(m.transform.position, dir, out hit, Mathf.Infinity, ~m.playerLayer))
            {
                if (hit.distance <= 6)
                {
                    m.currentEnemy = PropSpawner.Instance.enemies[i].GetComponent<Enemy>();
                    Debug.DrawRay(m.transform.position, dir * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");
                    break;
                }
            }
        }
        */

    }

    public override void UpdateState(Movement m)
    {
       
    }

    

}
