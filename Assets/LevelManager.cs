using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

[System.Serializable]
public class TypeToObject
{
	
	public PlanetTypes type;
	public GameObject obj;
	public bool canBeSpawned=false;
}
public class LevelManager : MonoSingleton<LevelManager>
{
	public Texture2D cursorTexture;
	public Texture2D cursorTextureDown;
	public Sprite cat0Instructions;
	public Sprite cat1Instructions;
	public AudioEffectsController audioEffectsController;
	public float planetSizeVariation = 0.1f;
	public float planetSizeCoefficient = 1;
	public Volume bloomVolume;
	public GameObject cat;
	private List<GameObject> catsList = new List<GameObject>();
	public int currentCat = 0;
	public bool isColliding = false;
	public GameObject arrow;
	public List<TypeToObject> basicPlanets = new List<TypeToObject>();
	public List<LinePoint> spawnPoints = new List<LinePoint>();
	public List<Tuple<LinePoint, GameObject>> planetsSpawned = new List<Tuple<LinePoint, GameObject>>();
	public bool isAnyBeingDragged = false;
	public float passed = 0;
	public float delaySeconds = 4;
	public Sun sun;
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

    public void SpawnNewCat(GameObject lastCat)
    {
	    currentCat++;
	    
	    if (currentCat > 1)
	    {
		    currentCat = 0;
	    }
	    if (currentCat == 0)
	    {
		    FindObjectOfType<CatMoodService>().SetWhim(WhimEnum.Mushrom);
		    FindObjectOfType<CatTarget>().targetPlanetType = PlanetTypes.PlanetOfMushrooms;
		    GameObject.Find("help").GetComponent<SpriteRenderer>().sprite = cat0Instructions;
	    }
	    if (currentCat == 1)
	    {
		    FindObjectOfType<CatMoodService>().SetWhim(WhimEnum.TreeSapling);
		    FindObjectOfType<CatTarget>().targetPlanetType = PlanetTypes.RockGrassTree;
		    GameObject.Find("help").GetComponent<SpriteRenderer>().sprite = cat1Instructions;
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
			SceneManager.LoadScene(2, LoadSceneMode.Single);

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
		this.audioEffectsController = GetComponent<AudioEffectsController>();
		UpdateCelestialBodiesListFromScene();
		RecalculateTrajectory();
		this.sun = FindObjectOfType<Sun>();
		cat = FindObjectOfType<CatMovement>().gameObject;
		StartCoroutine(SpawningCoroutine());
		FindObjectOfType<CatMoodService>().SetWhim(WhimEnum.Mushrom);
		//GameObject.FindObjectOfType<GameManager>().ShowEndCampaignRewardScreen(100);

	}
	private LinePoint lastSpawnPoint=null;
	private GameObject lastPlanetSpawned=null;
	private IEnumerator SpawningCoroutine()
	{
		while (true)
		{
		
			yield return new WaitForSeconds(3.0f);
			if (celestialBodies.Count>=6) continue;
			//get random planet
			var allowedPlanets = basicPlanets.Where(x => x.canBeSpawned).ToList();
			GameObject planet = allowedPlanets[Random.Range(0, allowedPlanets.Count)].obj;
			LinePoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
			while (true)
			{
				planet = allowedPlanets[Random.Range(0, allowedPlanets.Count)].obj;
				if (lastPlanetSpawned!=planet){
					break;
				}
				else
				{
					continue;
				}
			}
			while (true)
			{
				spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
				if (spawnPoint != lastSpawnPoint)
				{
					break;
				}
				else
				{
					continue;
				}
			}
			Debug.Log(spawnPoint.pos);
			lastSpawnPoint = spawnPoint;
			lastPlanetSpawned = planet;
			if (!isPaused)
				this.SpawnNewPlanet(planet, spawnPoint);
			// Wait for 10 seconds before calling the coroutine again
			
		}
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
