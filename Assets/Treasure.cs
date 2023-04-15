using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Animator anim;


    public void OpenChest()
    {
        anim.SetBool("isOpen", true);
        Movement.Instance.SwitchState(Movement.Instance.winState);
    }

}
