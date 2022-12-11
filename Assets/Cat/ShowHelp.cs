using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHelp : MonoBehaviour
{
    private bool isVisible = false;
    public GameObject helpTable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        if (isVisible)
            helpTable.GetComponent<SpriteRenderer>().enabled = false;
        else
            helpTable.GetComponent<SpriteRenderer>().enabled = true;

            isVisible = !isVisible;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
