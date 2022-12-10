using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public CatMoodService catMoodService;
    public CatMouth catMouth;
    public CatEye catEye;
    public WhimEnum catWhim;
    
    private void OnMouseDown()
    {
        if(catMouth != CatMouth.None)
            catMoodService.SetMouth(catMouth);
        
        if(catEye != CatEye.NONE)
            catMoodService.SetEye(catEye);
        
        if(catWhim != WhimEnum.None)
            catMoodService.SetWhim(catWhim);
        
    }
}
