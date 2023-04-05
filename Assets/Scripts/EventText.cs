using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventText : MonoBehaviour
{
    public static EventText Instance;
    public TextMeshProUGUI[] text;
    public bool testUpdateText;
    public string newEvent;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if(testUpdateText)
        {
            UpdateText(newEvent);
            testUpdateText = false;
        }
    }

    void CreateTextArray()
    {
        for (int i = 0; i < text.Length; i++)
        {
            text[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            text[i].text = text[i].gameObject.name.ToString();
        }
    }

    public void UpdateText(string newString)
    {
        for (int i = 0; i < text.Length-1; i++)
        {
            text[i].text = text[i+1].text;
        }
        text[text.Length-1].text = newString;
    }


}
