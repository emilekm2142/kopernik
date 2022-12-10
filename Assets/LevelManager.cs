using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
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
