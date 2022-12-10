using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    public CatMoodService catMoodService;
    public CatMood catMood;
    public CatEye catEye;
    
    private void OnMouseDown()
    {
        catMoodService.SetMood(catMood);
        catMoodService.SetEye(catEye);
    }
}
