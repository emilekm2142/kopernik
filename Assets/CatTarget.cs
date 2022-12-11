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
           LevelManager.Current.SpawnNewCat(prevCat);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
