using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CatMoodService : MonoBehaviour
{

    public GameObject eyeGameObject;
    
    public Sprite cuteSprite;
    public Sprite shockSprite;
    public Sprite angrySprite;

    public int secToCute = 5; 
    
    [HideInInspector]
    float timeRemaining = 0;
    [HideInInspector]
    private bool isTimerCoutdown = false;

    private void Start()
    {
        
    }

    private void StartTimer()
    {
        isTimerCoutdown = true;
        timeRemaining = secToCute;
    }

    void Update()
    {
        if (isTimerCoutdown)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                return;
            }
            SetEye(CatEye.CUTE);
            isTimerCoutdown = false;
        }
    }
    
    
    public virtual void SetMood(CatMood _catMood)
    {
        switch (_catMood)
        {
            case CatMood.MOVE_LEFT:
                Debug.Log("Make move left");
                break;
            
            case CatMood.HAPPY:
                Debug.Log("Make Happy");
                break;
        }
        
    }

    public virtual void SetEye(CatEye _catEye)
    {
        switch (_catEye)
        {
            case CatEye.CUTE:
                eyeGameObject.GetComponent<SpriteRenderer>().sprite = cuteSprite;  
                break;
            case CatEye.ANGRY:
                eyeGameObject.GetComponent<SpriteRenderer>().sprite = angrySprite;
                break;
            case CatEye.SHOCK:
                eyeGameObject.GetComponent<SpriteRenderer>().sprite = shockSprite;
                break;
        }
        StartTimer();
    }
    
}
