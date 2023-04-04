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
    Quaternion targetRotation;
    Vector3 targetPosition;
    public float turnSpeed;
    public float moveSpeed;
    public Animator anim;
    LayerMask playerLayer;
    public Transform wallCheck;
    public float distanceThreshold;
    public Button[] buttons;

    private void Start()
    {
        targetRotation = transform.rotation;
        targetPosition = transform.position;
        foreach (var b in buttons)
        {
            b.interactable = true;
        }
    }
    
    private void Update()
    {
        float dist = Vector3.Distance(targetPosition, transform.position);
        if(dist > distanceThreshold)
        {
            canMove = false;
            canRotate = false;
            foreach (var b in buttons)
            {
                b.interactable = true;
            }
            anim.speed = Mathf.Lerp(anim.speed, 0, moveSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            canMove = true;
            canRotate = true;
            if(anim.GetBool("isMoving") != false)
            {
                anim.SetBool("isMoving", false);
               
            }
            foreach (var b in buttons)
            {
                b.interactable = true;
            }
            transform.position = targetPosition;
        }

        if (transform.rotation != targetRotation)
        {
            canRotate = false;
            canMove = false;
            foreach (var b in buttons)
            {
                b.interactable = false;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = targetRotation;
            if (canRotate == false && canMove == false)
            {
                canRotate = true;
                canMove = true;
                foreach (var b in buttons)
                {
                    b.interactable = true;
                }
            }
        }

    }

    public void Forward()
    {
        if (transform.position == targetPosition)
        {
            if (canMove && CheckObstructions() == false)
            {
                targetPosition = wallCheck.position;

                anim.SetBool("isMoving", true);
                anim.speed = 2;
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
        if (canRotate)
        {
            Quaternion lr = transform.rotation * Quaternion.Euler(0,90,0);
            targetRotation = lr;
            anim.SetBool("isMoving", true);
        }
    }

    public void TurnLeft()
    {
        if (canRotate)
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
