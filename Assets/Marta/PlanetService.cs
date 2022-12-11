using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlanetService : MonoBehaviour
{

    public Vector3 pointAround;
    public GameObject startObject;

    private float lastScale = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        pointAround = startObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       // Camera.main.transform.position = pointAround;
       
       //Camera.main.transform.position = new Vector3 (pointAround.x,pointAround.y, Camera.main.transform.position.z);

       float zPos = CalculateLocalBounds();
       
       Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position,
           new Vector3 (pointAround.x,pointAround.y,  Camera.main.transform.position.z),
           40 * Time.deltaTime);
      //  Camera.main.orthographicSize = zPos;
    }
    
    private float CalculateLocalBounds()
    {
        Quaternion currentRotation = this.transform.rotation;
        this.transform.rotation = Quaternion.Euler(0f,0f,0f);
 
        Bounds bounds = new Bounds(this.transform.position, Vector3.zero);
 
        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
 
        Vector3 localCenter = bounds.center - this.transform.position;
        bounds.center = localCenter;
        //Debug.Log("The local bounds of this model is " + bounds.extents.x);
        this.transform.rotation = currentRotation;

        
        return Math.Max(bounds.extents.x, bounds.extents.y);
    }
}
