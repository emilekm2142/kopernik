using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script could be utilized for testing or creating simple systems. Attach it to any Gravity object */
[Serializable]
[RequireComponent(typeof(CelestialBody))]
public class StartVelocity : MonoBehaviour
{
	public Vector2 startVelocity;
	void Start () {
		accelerate();
	}

	public void accelerate()
	{
		GetComponent<Rigidbody2D>().velocity = startVelocity;
	}
	
}
