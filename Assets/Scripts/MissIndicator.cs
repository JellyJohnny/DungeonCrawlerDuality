using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissIndicator : MonoBehaviour
{
    public TextMeshPro text;

    public float startY;
    public float endY;
    public float moveSpeed;
    Vector3 textStartPos;
    Vector3 textEndPos;

    private void Start()
    {
        textStartPos = new Vector3(text.transform.position.x, text.transform.position.y + startY, text.transform.position.z);
        textEndPos = new Vector3(text.transform.position.x, text.transform.position.y + endY, text.transform.position.z);
    }

    private void Update()
    {
        float dist = Vector3.Distance(textEndPos, text.transform.position);

        if(dist > 0.1f)
        {
            text.transform.position = Vector3.Lerp(text.transform.position, textEndPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}
