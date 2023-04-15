
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class AppleIdleState : AppleBaseState
{
    public override void EnterState(Movement m)
    {
        

        m.canMove = true;
        m.canRotate = true;

        m.anim.SetBool("isMoving", false);
        m.UpdateButtons(true);

        m.deathScreen.SetActive(false);


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

        

    }

    public override void UpdateState(Movement m)
    {

    }

    

}
