using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    public PlanetService planetService;
    public float angel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        planetService.pointAround = this.gameObject.transform.position;
        angel = angel * -1;
        // Camera.main.transform.position = this.gameObject.transform.position;
        // Camera.main.transform.parent = this.gameObject.transform;
    }
    

    void FixedUpdate () {
    
        Vector3 axis = new Vector3(0,0,1);
        transform.RotateAround(planetService.pointAround, axis, angel /*Time.deltaTime * 10*/);
    }
}
