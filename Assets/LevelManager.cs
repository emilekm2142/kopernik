using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoSingleton<LevelManager>
{
	public GameObject planetPrefab;
	public List<LinePoint> spawnPoints = new List<LinePoint>();
	public List<Tuple<LinePoint, GameObject>> planetsSpawned = new List<Tuple<LinePoint, GameObject>>();
	
	public float passed = 0;
	public float delaySeconds = 4;
	public bool isPaused=false;
    // Start is called before the first frame update
    public void MoveObjects()
    {
	    
    }
    public void UpdateCelestialBodiesListFromScene()
    {
	    celestialBodies.Clear();
	    CelestialBody[] celestialBodiesArray = FindObjectsOfType<CelestialBody>();
	    foreach (CelestialBody celestialBody in celestialBodiesArray)
	    {
		    celestialBodies.Add(celestialBody);
	    }
        
    }
    public List<CelestialBody> celestialBodies = new List<CelestialBody>();
    public List<GameObject> collisionsObjects = new List<GameObject>();
    void FixedUpdate()
	{
		if (!isPaused)
		{
			foreach (CelestialBody celestialBody in celestialBodies)
			{
				if (celestialBody.doesMove) celestialBody.DoStep();
			}
		}
			
	}

	void Update()
	{
		
		

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			TogglePause();
		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			//get random spawn point
			LinePoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
			this.SpawnNewPlanet(planetPrefab, spawnPoint);
		}

		if (   (Input.GetKeyDown("space")||Input.GetKeyDown(KeyCode.Pause)) && !Application.isEditor)
		{
			TogglePause();
			
		}
		
	}

	public void RecalculateTrajectory()
	{
		foreach (var celestialBody in celestialBodies)
		{
			if (celestialBody.doesMove) celestialBody.RecalculateTrajectory(true);
		}
	}
	private void Start()
	{
		UpdateCelestialBodiesListFromScene();
		RecalculateTrajectory();
		
		//GameObject.FindObjectOfType<GameManager>().ShowEndCampaignRewardScreen(100);
		
	}
	private Vector2 CalculateOrbitalVelocity(float gravitationalForce, Vector2 sunPosition, Vector2 planetPosition, float sunMass, float G)
	{
		// Calculate the distance between the sun and the planet
		var distance = Vector2.Distance(sunPosition, planetPosition);

		// Calculate the gravitational force between the sun and the planet
		

		// Calculate the velocity needed to achieve a circular orbit
		var orbitalVelocity = Mathf.Sqrt(gravitationalForce / distance);

		// Calculate the direction of the velocity vector
		var direction = (sunPosition - planetPosition).normalized;
		var projectedDirection = Vector2.Dot(direction, sunPosition.normalized) * sunPosition.normalized;
		// Return the velocity vector
		return projectedDirection * orbitalVelocity;
	}
	public Vector2 GetPointOnUnitCircleCircumference()
	{
		float randomAngle = Random.Range(0f, Mathf.PI * 2f);
		return new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle)).normalized;
	}
	public void SpawnNewPlanet(GameObject prefab, LinePoint lp)
	{
		//instatiate planet
		//get random position on a rim of a circle
		
		var sun = FindObjectOfType<Sun>();
		Vector2 randomPosition = GetPointOnUnitCircleCircumference() * Vector2.Distance(sun.transform.position, lp.pos);
		GameObject planet = Instantiate(prefab, lp.pos, Quaternion.identity);
		//add to list
		planetsSpawned.Add(new Tuple<LinePoint, GameObject>(lp, planet));
		//get vector of magnitude lp.velocity.magnitude that crosses through lp.pos and is parrarel to sun.transform.position
		Vector2 velocity = (lp.pos - (Vector2)sun.transform.position).normalized * lp.velocity.magnitude;
		planet.GetComponent<StartVelocity>().startVelocity = lp.velocity;
		this.celestialBodies.Add(planet.GetComponent<CelestialBody>());
		planet.GetComponent<CelestialBody>().RecalculateTrajectory(true);

	}
	
	

		public void Win()
	{
		

	}



	
	
	

	public void Lose()
	{
		
	}
	

	public void Pause(bool showTrajectory = true, bool countAsPause = true)
	{
		Debug.Log("Pausing");
		isPaused = true;

	}

	public void Resume()
	{
		isPaused = false;
	}

	public void TogglePause()
	{
		Debug.Log("toggle pause");
		if (!isPaused)
		{
			
			Pause();
		}
		else
		{
		
			Resume();
		}
		
	}

	

}
