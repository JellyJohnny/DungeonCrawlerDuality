using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public AppleBaseState currentState;
    public AppleIdleState idleState = new AppleIdleState();
    public AppleMoveState moveState = new AppleMoveState();
    public AppleTurnState turnState = new AppleTurnState();
    public AppleAttackState attackState = new AppleAttackState();

    public float checkDistance;
    public bool canMove;
    public bool canRotate;
    public Quaternion targetRotation;
    public Vector3 targetPosition;
    public float turnSpeed;
    public float moveSpeed;
    public Animator anim;
    public Animator swordAnim;
    public LayerMask playerLayer;
    public LayerMask wallLayer;
    public Transform wallCheck;
    public float distanceThreshold;
    public Button[] buttons;
    public Button attackButton;
    public int turnDirection;
    public GameObject[] enemies;
    public float hitDist;

    public Enemy currentEnemy;

    public GameObject missPrefab;
    public GameObject damagePrefab;

    public AudioSource aud;
    public AudioSource uiAud;
    public AudioClip[] footstepClips;
    public AudioClip swordSound;
    public AudioClip swordHit;
    public float footStepDelay;

    private void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
        aud = GetComponent  <AudioSource>();    
    }
    
    private void Update()
    {
        if (currentEnemy != null)
        {
            Vector3 enemyDir = currentEnemy.transform.position - transform.position;
            float enemyDist = Vector3.Distance(currentEnemy.transform.position, transform.position);

            Debug.DrawRay(transform.position, enemyDir, Color.green);
        }

        
        currentState.UpdateState(this);
    }

    public IEnumerator PlayFootStep()
    {
        while (currentState == moveState || currentState == turnState)
        {
            yield return new WaitForSeconds(footStepDelay);
            int r = Random.Range(0, footstepClips.Length);

            aud.clip = footstepClips[r];
            aud.Play();
            Debug.Log("footstep");
        }
        
    }

    public void SwitchState(AppleBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void UpdateButtons(bool dis)
    {
        foreach (var b in buttons)
        {
            b.interactable = dis;
        }
    }

    public bool MoveTowards()
    {
        float dist = Vector3.Distance(targetPosition, transform.position);
        return (dist > distanceThreshold);
    }

    public void Forward()
    {

        //if (transform.position == targetPosition)
        //{
            if (canMove && CheckObstructions() == false)
            {
                SwitchState(moveState);
                
            }
        //}
    }

    bool CheckObstructions()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        return (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, checkDistance, ~playerLayer));
    }

    public void TurnRight()
    {
        
        turnDirection = 90;
        if (canRotate && canMove)
        {
            SwitchState(turnState);
        }
    }

    public void TurnLeft()
    {
        
        turnDirection = -90;
        if (canRotate && canMove)
        {
            SwitchState(turnState);
        }
    }

    public void AttackEnemy()
    {
        if(currentEnemy != null)
        {
            anim.SetTrigger("isAttacking");
            swordAnim.SetTrigger("isAttacking");
            attackButton.interactable = false;
            attackButton.image.fillAmount = 0;
            StartCoroutine(PlaySwordSound());
        }
    }

    IEnumerator PlaySwordSound()
    {
        aud.clip = swordSound;
        yield return new WaitForSeconds(0.25f);
        currentEnemy.TakeDamage(this, missPrefab, damagePrefab);
        aud.Play();
    }

   

    void OnForward()
    {
        Forward();
    }

    void OnTurnRight()
    {
        TurnRight();
    }

    void OnTurnLeft()
    {
        TurnLeft();
    }
}
