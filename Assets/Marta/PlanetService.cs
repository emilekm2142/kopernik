using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetService : MonoBehaviour
{

    public Vector3 pointAround;
    public GameObject startObject;
    
    // Start is called before the first frame update
    void Start()
    {
        pointAround = startObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       // Camera.main.transform.position = pointAround;
       
       Camera.main.transform.position = new Vector3 (pointAround.x,pointAround.y, Camera.main.transform.position.z);
    }
}
