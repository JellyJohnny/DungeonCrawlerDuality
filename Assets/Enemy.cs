using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float turnSpeed;
    public bool targetLock;
    GameObject player;
    public Animator anim;

    private void Update()
    {
        if(targetLock)
        {
            Vector3 dir = player.transform.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            targetLock = true;
            player.GetComponent<Combat>().EnterCombat(this.GetComponent<Enemy>(),transform.position);
        }
    }

    public void TakeDamage()
    {
        anim.SetTrigger("takeDamage");
        Debug.Log("enemy took damage");
    }
}
