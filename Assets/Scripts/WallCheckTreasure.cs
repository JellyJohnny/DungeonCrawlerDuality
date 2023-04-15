using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckTreasure : MonoBehaviour
{
    public bool canOpen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent.GetComponent<Treasure>())
        {
            StartCoroutine(OpenChestDelay(other));
            
        }
    }

    IEnumerator OpenChestDelay(Collider coll)
    {
        yield return new WaitForSeconds(1.5f);
        coll.transform.parent.GetComponent<Treasure>().OpenChest();
    }
}
