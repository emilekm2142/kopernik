using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public CatMoodService catMoodService;
    public CatMouth catMouth;
    public CatEye catEye;
    
    private void OnMouseDown()
    {
        if(catMouth != CatMouth.None)
            catMoodService.SetMouth(catMouth);
        
        if(catEye != CatEye.NONE)
            catMoodService.SetEye(catEye);
    }
}
