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
		if (passed < delaySeconds && isPaused)
		{
			passed += Time.deltaTime;
			isPaused = true;
		}

		if (passed > delaySeconds)
		{
			isPaused = false;
		}
		

		if (Input.GetKeyDown(KeyCode.Escape))
		{
				
				Pause();


		}

		if (Input.GetKeyDown(KeyCode.A))
		{
			//get random position in distance 4 from vector2
			Vector2 randomPosition = Random.insideUnitCircle * 4;
			this.SpawnNewPlanet(planetPrefab, (Vector2)FindObjectOfType<Sun>().transform.position + new Vector2(randomPosition.x, randomPosition.y));
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

	public void SpawnNewPlanet(GameObject prefab, Vector2 position){
		Sun sun = GameObject.FindObjectOfType<Sun>();
		GameObject newPlanet = Instantiate(prefab, position, Quaternion.identity);
		newPlanet.transform.position = position;
		newPlanet.GetComponent<StartVelocity>().startVelocity = CalculateOrbitalVelocity(
			(float)sun.GetComponent<CelestialBody>().calculateThisBodyAttraction(newPlanet),
			sun.gameObject.transform.position, position, sun.GetComponent<CelestialBody>().mass,
			(float)GameManager.Current.gravityForceScale)*10;
		UpdateCelestialBodiesListFromScene();
		newPlanet.GetComponent<CelestialBody>().RecalculateTrajectory(true);
		
	}

	public void Win()
	{
		

	}



	
	
	

	public void Lose()
	{
		
	}
	

	public void Pause(bool showTrajectory = true, bool countAsPause = true)
	{
		isPaused = true;

	}

	public void Resume()
	{
		isPaused = false;
	}

	public void TogglePause()
	{
		
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
