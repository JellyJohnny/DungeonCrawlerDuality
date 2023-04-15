using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float turnSpeed;
    public bool targetLock;
    public GameObject player;
    public Animator anim;
    public int health;
    public AudioClip swordHit;
    public AudioClip miss;
    public AudioClip death;
    AudioSource aud;
    public string[] deathMessage;

    //highlight tests
    public Mesh mesh;
    public Material material;

    private void Start()
    {
        aud = GetComponent<AudioSource>();  

    }

    private void Update()
    {
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
        if (targetLock && player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            targetLock = true;
            player.GetComponent<Combat>().EnterCombat(this.GetComponent<Enemy>(),transform.position);
        }
    }
    */

    public void TakeDamage(Movement m,GameObject missPrefab,GameObject damagePrefab)
    {
        StartCoroutine(AttackPlayer(m));
        //choose miss prefab or damage prefab
        int r = Random.Range(0, 2);

        

        switch (r)
        {
            case 0:
                Instantiate(missPrefab, m.currentEnemy.transform.position, Quaternion.LookRotation(-transform.forward));
                anim.SetTrigger("isDodging");
                EventText.Instance.UpdateText("The bat dodges the attack!");
                break;
            case 1:

                aud.clip = m.swordHit;
                aud.Play();
                int d = Random.Range(1, 3);

                health -= d;

                GameObject dP = Instantiate(damagePrefab, m.currentEnemy.transform.position, Quaternion.LookRotation(-transform.forward));

                dP.transform.GetChild(0).GetComponent<TextMeshPro>().text = "- " + d.ToString();

                if (health > 0)
                {

                    anim.SetTrigger("isDamaged");
                    EventText.Instance.UpdateText("The bat takes " + d + " point(s) of damage!");
                    Debug.Log("enemy took damage");

                }
                else
                {
                    anim.SetBool("isDead", true);
                    StartCoroutine(RemoveEnemy());
                    m.currentEnemy = null;
                }
                break;
        }

        

        
    }

    public IEnumerator AttackPlayer(Movement m)
    {
        anim.SetTrigger("isAttacking");
        yield return new WaitForSeconds(1f);

        if (health > 0)
        {
            m.TakeDamage();
        }
    }

   IEnumerator RemoveEnemy()
    {

        aud.clip = swordHit;
        aud.Play();
        yield return new WaitForSeconds(0.2f);
        EventText.Instance.UpdateText("The bat is is vanquished!");

        int r = Random.Range(0, deathMessage.Length);

        EventText.Instance.UpdateText(deathMessage[r]);
        aud.clip = death;
        aud.Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
