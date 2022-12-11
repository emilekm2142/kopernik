using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AudioClip menu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play()
    {
        GetComponent<AudioSource>().DOFade(0, 1);
        DOVirtual.Vector3(
            FindObjectOfType<CatMoodService>().gameObject.transform.position,
            FindObjectOfType<CatMoodService>().gameObject.transform.position + new Vector3(5, 0, 0),
            1f,
            (x) => FindObjectOfType<CatMoodService>().gameObject.transform.position = x
        ).SetEase(Ease.InOutSine).OnComplete(
            () =>
            {
                   
                 
            });
        FindObjectOfType<DontDestroyOnLoad>().Tween(() =>
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        });
    }

    
    // Update is called once per frame
    void Update()
    {
    }
}
