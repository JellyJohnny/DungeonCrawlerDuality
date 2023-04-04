using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    public bool inCombat;
    Movement move;
    public Button attackButton;
    Vector3 enemyPosition;
    Vector3 enemyDirection;
    public LayerMask playerLayer;
    public LayerMask wallLayer;
    public Enemy currentEnemy;
    bool see;

    private void Start()
    {
        move = GetComponent<Movement>();
        attackButton.interactable = false;
    }

    private void Update()
    {
        if(inCombat)
        {
            enemyDirection = enemyPosition - transform.position;
            Quaternion lookR = Quaternion.LookRotation(enemyDirection);
            move.targetRotation = lookR;
        }
    }

    bool canSeePlayer()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if(!Physics.Raycast(transform.position, enemyDirection, out hit, 7f, ~playerLayer) 
            && !Physics.Raycast(transform.position, enemyDirection, out hit, 7f, wallLayer))
        {
            see = true;
        }
        else
        {
            see = false;
        }
        return see;

    }

    public void EnterCombat(Enemy e, Vector3 ePos)
    {
        if (canSeePlayer())
        {
            currentEnemy = e;
            enemyPosition = ePos;

            attackButton.interactable = true;
            move.UpdateButtons(false);
            inCombat = true;
            move.canMove = false;
        }
    }

    public void AttackEnemy()
    {
        if(currentEnemy != null) 
        {
            currentEnemy.TakeDamage();
        }
        
    }
}
