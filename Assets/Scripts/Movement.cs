using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float checkDistance;
    public bool canMove;
    public bool canRotate;
    public Quaternion targetRotation;
    Vector3 targetPosition;
    public float turnSpeed;
    public float moveSpeed;
    public Animator anim;
    LayerMask playerLayer;
    public Transform wallCheck;
    public float distanceThreshold;
    public Button[] buttons;
    Combat combat;

    private void Start()
    {
        combat = GetComponent<Combat>();    
        targetRotation = transform.rotation;
        targetPosition = transform.position;
        UpdateButtons(true);
    }
    
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!combat.inCombat)
        {
            if (MoveTowards())
            {
                UpdateButtons(false);

                anim.speed = Mathf.Lerp(anim.speed, 0, moveSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                canMove = true;
                canRotate = true;
                if (anim.GetBool("isMoving") != false)
                {
                    anim.SetBool("isMoving", false);

                }
                UpdateButtons(true);
                transform.position = targetPosition;
            }
            //transform.position = targetPosition;
        }

        else
        {
            if (MoveTowards())
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition;
            }
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (!combat.inCombat)
        { 
            if (transform.rotation != targetRotation)
            {
                canRotate = false;
                canMove = false;
                UpdateButtons(false);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = targetRotation;
                if (canRotate == false && canMove == false)
                {
                    canRotate = true;
                    canMove = true;
                    UpdateButtons(true);
                }
            }
        }
        else
        {
            if (MoveTowards())
            {
                if (transform.rotation != targetRotation)
                {

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }
                else
                {
                    transform.rotation = targetRotation;
                }
            }
        }
    }

    public void UpdateButtons(bool dis)
    {
        foreach (var b in buttons)
        {
            b.interactable = dis;
        }
    }

    bool MoveTowards()
    {
        float dist = Vector3.Distance(targetPosition, transform.position);
        return (dist > distanceThreshold);
    }

    public void Forward()
    {
        if (canMove)
        {
            if (transform.position == targetPosition)
            {
                if (canMove && CheckObstructions() == false)
                {
                    targetPosition = wallCheck.position;

                    //anim.SetBool("isMoving", true);
                    anim.speed = 2;
                    UpdateButtons(false);
                    canMove = false;
                }
            }
        }
    }

    bool CheckObstructions()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        return (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, checkDistance, ~playerLayer));
    }
    

    public void TurnRight()
    {
        if (canRotate && canMove)
        {
            Quaternion lr = transform.rotation * Quaternion.Euler(0,90,0);
            targetRotation = lr;
            anim.SetBool("isMoving", true);
        }
    }

    public void TurnLeft()
    {
        if (canRotate && canMove)
        {
            Quaternion lr = transform.rotation * Quaternion.Euler(0, -90, 0);
            targetRotation = lr;
            anim.SetBool("isMoving", true);
        }
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
