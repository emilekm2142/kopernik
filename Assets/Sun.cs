using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private CelestialBody _celestialBody;
    List<CelestialBody> celestialBodies = new List<CelestialBody>();
    // Start is called before the first frame update
    void Start()
    {
        this._celestialBody = GetComponent<CelestialBody>();
        _celestialBody.doesMove = false;
        UpdateCelestialBodiesListFromScene();
    }
    public void AddCelestialBody(CelestialBody celestialBody)
    {
        celestialBodies.Add(celestialBody);
    }
    public void UpdateCelestialBodiesListFromScene()
    {
        celestialBodies.Clear();
        CelestialBody[] celestialBodiesArray = FindObjectsOfType<CelestialBody>();
        foreach (CelestialBody celestialBody in celestialBodiesArray)
        {
            if (celestialBody!=_celestialBody) celestialBodies.Add(celestialBody);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }
}
