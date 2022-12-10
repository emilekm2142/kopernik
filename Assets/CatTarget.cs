using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatTarget : MonoBehaviour
{
    // Start is called before the first frame update
    private PlanetTypes targetPlanetType;
    void Start()
    {
        
    }

    public void ScanIfWon()
    {
        var x = FindObjectsOfType<PlanetCollision>().FirstOrDefault(x => x.planetType == targetPlanetType);
        if (x != null)
        {
            var cat = FindObjectOfType<CatMovement>().gameObject;
            var prevCat = LevelManager.Current.cat;
            LevelManager.Current.SpawnNewCat(prevCat);
            //tween it
            cat.transform.parent = x.gameObject.transform;
            cat.transform.localPosition = Vector3.zero+Vector3.right;
            cat.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
