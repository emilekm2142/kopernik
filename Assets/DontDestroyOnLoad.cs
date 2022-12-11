using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject); 
    }
  
    public void Tween(Action action)
    {
        DOVirtual.Color(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), 1, (color) =>
        {
            GetComponent<SpriteRenderer>().color = color;
        }).SetEase(Ease.OutSine).OnComplete(() =>
        {
            action();
            DOVirtual.Color(new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f), 1, (color) =>
            {
                GetComponent<SpriteRenderer>().color = color;
            }).SetEase(Ease.InSine);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
