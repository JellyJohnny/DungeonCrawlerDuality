using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float turnSpeed;
    Quaternion targetRotation;
    public float distanceCheck;
    public LayerMask enemyLayer;
    public bool seePlayer;

    private void Start()
    {
        seePlayer = LineOfSight();
    }

    bool LineOfSight()
    {
        Vector3 dir = target.position - transform.position;
        RaycastHit hit;
        return (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, ~enemyLayer));
    }

}
