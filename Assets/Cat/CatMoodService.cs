using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatMoodService : MonoBehaviour
{

    public AudioClip soundClip1;
    public AudioClip soundClip2;
    public AudioClip soundClip3;
    public AudioClip soundClip4;
    
    public GameObject eyeGameObject;
    public GameObject mouthGameObject;
    public GameObject whimGameObject;
    
    public Sprite eyeCuteSprite;
    public Sprite eyeShockSprite;
    public Sprite eyeAngrySprite;
    
    public Sprite mouthHappySprite;
    public Sprite mouthNeutralSprite;
    public Sprite mouthSadSprite;
   
    public Sprite whimAppleSprite;
    public Sprite whimTreeSprite;
    public Sprite whimMushroomSprite;
    
    public int coutdownSecond = 5;

    [HideInInspector]
    private CoutdownTimer eyeTimer = new CoutdownTimer();

    [HideInInspector]
    private CoutdownTimer mouthTimer = new CoutdownTimer();
    
    private void Start()
    {
        eyeTimer.SetDefaultCoutdown(coutdownSecond);
        mouthTimer.SetDefaultCoutdown(coutdownSecond);
    }

    private void StartTimer()
    {
        eyeTimer.StartTimer();
        mouthTimer.StartTimer();
    }

    void Update()
    {
        if (eyeTimer.isInUse())
            if(eyeTimer.Update()) SetEye(CatEye.CUTE);
        if (mouthTimer.isInUse())
            if(eyeTimer.Update()) SetMouth(CatMouth.Neutral);
    }
    
    public virtual void SetMouth(CatMouth _catMouth)
    {
        switch (_catMouth)
        {
            case CatMouth.Happy:
                mouthGameObject.GetComponent<SpriteRenderer>().sprite = mouthHappySprite;
                PlaySound();
                break;
            case CatMouth.Neutral:
                mouthGameObject.GetComponent<SpriteRenderer>().sprite = mouthNeutralSprite;
                break;
            case CatMouth.Sad:
                mouthGameObject.GetComponent<SpriteRenderer>().sprite = mouthSadSprite;
                break;
        }
        
        StartTimer();
    }

    public virtual void SetEye(CatEye _catEye)
    {
        switch (_catEye)
        {
            case CatEye.CUTE:
                eyeGameObject.GetComponent<SpriteRenderer>().sprite = eyeCuteSprite;  
                break;
            case CatEye.ANGRY:
                eyeGameObject.GetComponent<SpriteRenderer>().sprite = eyeAngrySprite;
                PlaySound();
                break;
            case CatEye.SHOCK:
                eyeGameObject.GetComponent<SpriteRenderer>().sprite = eyeShockSprite;
                PlaySound();
                break;
        }
        StartTimer();
    }

    public virtual void SetWhim(WhimEnum _whimEnum)
    {
        switch (_whimEnum)
        {
            case WhimEnum.Apple:
                whimGameObject.GetComponent<SpriteRenderer>().sprite = whimAppleSprite;
                break;
            case WhimEnum.Mushrom:
                whimGameObject.GetComponent<SpriteRenderer>().sprite = whimMushroomSprite;
                break;
            case WhimEnum.TreeSapling:
                whimGameObject.GetComponent<SpriteRenderer>().sprite = whimTreeSprite;
                break;
        }
    }

    void PlaySound()
    {
        float randomNumber = Random.Range(1, 5);
        Debug.Log(randomNumber);
        switch (randomNumber)
        {
            case 1:
                GetComponent<AudioSource>().PlayOneShot(soundClip1);
                break;
            case 2:
                GetComponent<AudioSource>().PlayOneShot(soundClip2);
                break;
            case 3:
                GetComponent<AudioSource>().PlayOneShot(soundClip3);
                break;
            case 4:
                GetComponent<AudioSource>().PlayOneShot(soundClip4);
                break;

        }
    }
    
}
