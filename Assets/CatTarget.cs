using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CatTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public PlanetTypes targetPlanetType;
    void Start()
    {
        
    }

    public void ScanIfWon()
    {
        var x = FindObjectsOfType<PlanetCollision>().FirstOrDefault(x => x.planetType == targetPlanetType);
        if (x != null && x.GetComponent<PlanetCollision>()!=null && x.GetComponent<PlanetCollision>().planetType==targetPlanetType)
        {
            var cat = FindObjectOfType<CatMovement>().gameObject;
            var prevCat = LevelManager.Current.cat;
           // LevelManager.Current.SpawnNewCat(prevCat);
            //tween it
            x.isDestroyable = false;
            cat.transform.parent = x.gameObject.transform;
            // DOVirtual.Vector3(cat.transform.localPosition, Vector3.zero + Vector3.right / 6, 1,
            //     v => cat.transform.localPosition = v).SetEase(Ease.InOutSine);
            DOVirtual.Vector3(cat.transform.localScale, new Vector3(0.4f, 0.4f, 0.4f), 1,
                v => cat.transform.localScale = v).SetEase(Ease.InOutSine);
            Camera.main.DOOrthoSize(2, 1f).SetEase(Ease.OutElastic);
            LevelManager.Current.Pause();
             DOVirtual.Vector3(Camera.main.transform.position, x.transform.position, 0.1f,
                v => Camera.main.transform.position = v).
                 OnComplete(()=>LevelManager.Current.Resume())
                 .SetEase(Ease.InOutSine);;
             Camera.main.transform.parent = x.gameObject.transform;

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
