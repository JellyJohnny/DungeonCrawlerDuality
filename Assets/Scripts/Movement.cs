using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public static Movement Instance;
    public AppleBaseState currentState;
    public AppleIdleState idleState = new AppleIdleState();
    public AppleMoveState moveState = new AppleMoveState();
    public AppleTurnState turnState = new AppleTurnState();
    public AppleAttackState attackState = new AppleAttackState();
    public AppleDeathState deathState = new AppleDeathState();
    public AppleWinState winState = new AppleWinState();     

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

    public float health;
    float maxHP;
    public Image hpAmount;
    public float changeRate;
    float targetHP;
    public Animator hurtAnim;
    public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject optionsScreen;
    bool displayOptions;
    public Camera cam;

    private void Start()
    {
        Instance = this;
        currentState = idleState;
        currentState.EnterState(this);
        aud = GetComponent  <AudioSource>();
        maxHP = health;
        targetHP = health / maxHP;
    }
    
    private void Update()
    {
        if (currentEnemy != null)
        {
            Vector3 enemyDir = currentEnemy.transform.position - transform.position;
            float enemyDist = Vector3.Distance(currentEnemy.transform.position, transform.position);

            Debug.DrawRay(transform.position, enemyDir, Color.green);
        }

        if(hpAmount.fillAmount != targetHP)
        {
            hpAmount.fillAmount = Mathf.Lerp(hpAmount.fillAmount, health / maxHP, changeRate * Time.deltaTime);
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

    public void TakeDamage()
    {
        //choose miss prefab or damage prefab
        int r = Random.Range(0, 2);



        switch (r)
        {
            case 0:
                EventText.Instance.UpdateText("The bat missed it's attack!");
                break;
            case 1:

                aud.clip = swordHit;
                aud.Play();
                int d = Random.Range(1, 3);

                health -= d;

                hurtAnim.SetTrigger("isHurt");

                if (health > 0)
                {
                    targetHP = health / maxHP;
                    anim.SetTrigger("isDamaged");
                    EventText.Instance.UpdateText("The bat deals " + d + " point(s) of damage!");
                    Debug.Log("player took damage");

                }
                else
                {
                    SwitchState(deathState);
                    currentEnemy = null;
                }
                break;
        }




    }

    public IEnumerator DisplayRestartScreen()
    {
        yield return new WaitForSeconds(1f);
        deathScreen.SetActive(true);
    }



    public void Win()
    {
        StartCoroutine(Wi());
    }

    public void OptionsDisplay()
    {
        displayOptions = !displayOptions;
        optionsScreen.SetActive(displayOptions);
    }

    public IEnumerator Wi()
    {
        yield return new WaitForSeconds(2f);
        winScreen.SetActive(true);
    }

    public void Rest()
    {
        StartCoroutine(RestartApplication());
    }

    public void Qu()
    {
        StartCoroutine(QuitApplication());
    }

    public IEnumerator RestartApplication()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Main");
    }

    public IEnumerator QuitApplication()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
