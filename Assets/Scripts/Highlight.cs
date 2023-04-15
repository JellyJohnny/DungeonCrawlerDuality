using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    private void Start()
    {
        Material mymat = GetComponent<Renderer>().material;
        mymat.SetColor("_EmissionColor", Color.clear);
    }

    private void OnMouseEnter()
    {
        
        Material mymat = GetComponent<Renderer>().material;
        mymat.SetColor("_EmissionColor", Color.white);
    }

    private void OnMouseExit()
    {
        Material mymat = GetComponent<Renderer>().material;
        mymat.SetColor("_EmissionColor", Color.clear);
    }
}
