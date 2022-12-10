using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//make this serializable
[System.Serializable]
public class LinePoint
{
	public Vector2 pos;
	public Vector2 velocity;

}
public class CelestialBody : MonoBehaviour
{
    private List<LinePoint> trajectoryPoints = new List<LinePoint>();
    public float precalculatedMovementTime = 3f;
    public bool doesMove = true;
    public double multiplier = 1;
    public bool significant = false;
    public float mass = 1;
    public int stepInTrajectoryMovement = 0;
    private Rigidbody2D _rigidbody;
    private LineRenderer _lineRenderer;
    private List<Vector3> _linePoints = new List<Vector3>();
    
    public int TrajectoryPointsInterval = 5;

    public double calculateThisBodyAttraction(GameObject body)
    {
        // Newton's law of gravity G*((Mm)/(r^2))
        var localMultiplier = GameManager.Current.gravityForceScale*multiplier;
        double newtonsForce = localMultiplier*
                             ((this.mass * body.GetComponent<CelestialBody>().mass) /
                              (Mathf.Pow(
                                  Vector2.Distance(this.gameObject.transform.position,
                                      body.gameObject.transform.position), 2)));
        return newtonsForce;
    }
    public double calculateThisBodyAttraction(Vector2 position, float mass)
    {
        // Newton's law of gravity G*((Mm)/(r^2))
        var localMultiplier = GameManager.Current.gravityForceScale*multiplier;
        double newtonsForce = localMultiplier*
                              ((this.mass * mass) /
                               (Mathf.Pow(
                                   Vector2.Distance(this.gameObject.transform.position,
                                       position), 2)));
        return newtonsForce;
    }

    public Vector2 getDirectionalForce(Vector2 position, float mass)
    {
        var dstX = transform.position.x - position.x;
        var dstY = transform.position.y - position.y;
        var distance = Vector2.Distance(position, this.transform.position);
        var theta = Mathf.Atan2(dstY, dstX);
        var force = (float)calculateThisBodyAttraction(position, mass);
        //convert force from double to float
 
        //Debug.Log(theta * Mathf.Rad2Deg);
        //check if is nan
        if (float.IsNaN(Mathf.Sin(theta) * force))
        {
            theta = 0;
        }
        return new Vector2(Mathf.Cos(theta) * force, Mathf.Sin(theta) * force);
    }

    public void addForceToBody(GameObject body)
    {
        double forceMagnitude = this.calculateThisBodyAttraction(body);
        var f = this.getDirectionalForce(body.transform.position, body.GetComponent<CelestialBody>().mass);
        body.GetComponent<Rigidbody2D>().AddForce(f);
    }
    public void Attract(GameObject body)
    {
        addForceToBody(body);
    }

    public void RemoveCurrentStep()
    {
	    trajectoryPoints.RemoveAt(0);
	    _linePoints.RemoveAt(0);
    }

    public bool breakAtNext = false;
    public long stepsCalculatedCounter = 0;
    public void CalculateSingleNextPoint()
    {
	    var lastPoint = trajectoryPoints[^1];
	    float timeStep =Time.fixedDeltaTime;
	    Vector2 position = lastPoint.pos;
	    Vector2 velocity = lastPoint.velocity;
	    Vector2 force= new Vector2(0,0);
	    foreach (var body in LevelManager.Current.celestialBodies)
	    {
		    if (!body.significant)
		    {
			    continue;
		    }

		    if ( body != this && body!=null )
		    {
			    var forceExertedOnThis = body.getDirectionalForce(position, mass);
					
			    force.x += forceExertedOnThis.x;
			    force.y += forceExertedOnThis.y;
		    }
	    }
			
	    velocity.x += force.x / mass * timeStep;
	    velocity.y += force.y / mass * timeStep;

	    var oldPos = new Vector2(position.x,position.y);
	    position.x = position.x + (velocity.x * timeStep);
	    position.y = position.y + (velocity.y * timeStep);
	    LinePoint lp = new LinePoint();
	    lp.velocity = velocity;
	    lp.pos = position;
			
	    //if (stepsCalculatedCounter%TrajectoryPointsInterval==0) 
			trajectoryPoints.Add(lp);
	    if (stepsCalculatedCounter%1==0){ 
		    _linePoints.Add(new Vector3(position.x,position.y,0));
		    _lineRenderer.positionCount = _linePoints.Count;
		    _lineRenderer.SetPositions(_linePoints.ToArray());
	    }
	  
            
    }
    public void RecalculateTrajectory(bool addToPreview)
    {
	    
		int i = -1;
		int c = 0;
	
		trajectoryPoints.Clear();
		
		GetComponent<LineRenderer>().SetPositions(new Vector3[0]);
		bool addCurrentFrame = true;
		//float timeStep =Time.fixedDeltaTime / Physics2D.velocityIterations/1;
		float timeStep =Time.fixedDeltaTime;
		float time = 0;
		Vector2 position = transform.position;
		Vector2 velocity = GetComponent<Rigidbody2D>().velocity==Vector2.zero?GetComponent<StartVelocity>().startVelocity:GetComponent<Rigidbody2D>().velocity;
		//Vector2 velocity = new Vector2(30,0);

		while (time <= precalculatedMovementTime)
		{
			
			i++;
			Vector2 force= new Vector2(0,0);
			foreach (var body in LevelManager.Current.celestialBodies)
			{
				if (!body.significant)
				{
					continue;
				}
				
				if ( body != this && body!=null )
				{
					var forceExertedOnThis = body.getDirectionalForce(position, mass);
					
					force.x += forceExertedOnThis.x;
					force.y += forceExertedOnThis.y;
				}
			}
			
			velocity.x += force.x / mass * timeStep;
			velocity.y += force.y / mass * timeStep;

			var oldPos = new Vector2(position.x,position.y);
			position.x = position.x + (velocity.x * timeStep);
			position.y = position.y + (velocity.y * timeStep);
			LinePoint lp = new LinePoint();
			lp.velocity = velocity;
			lp.pos = position;
			
			trajectoryPoints.Add(lp);
			if (addCurrentFrame){ _linePoints.Add(new Vector3(position.x,position.y,0));
				c++;
				addCurrentFrame = true;
			}

			bool breakAtNext = false;
			foreach (var body in LevelManager.Current.collisionsObjects)
			{

				
				if (body!=this  && body!=null)
					if (body.GetComponent<Collider2D>().OverlapPoint(position))
					{
						
							breakAtNext = true;
						
					}
					else
					{
						addCurrentFrame = true;
						
					}

				if (breakAtNext)
				{
					goto dwa;
				}
				
			}

			time += timeStep;
		}
		dwa:{}
		if(addToPreview)
		{
			GetComponent<LineRenderer>().positionCount = _linePoints.Count;
			GetComponent<LineRenderer>().SetPositions(_linePoints.ToArray());
		}
		Debug.Log(this.trajectoryPoints.Count);
	}

    
    public void DoStep()
    {
	    _rigidbody.position = trajectoryPoints[0].pos;
	    //	Debug.Log(String.Format("{0}; {1}", trajectoryPoints[i].pos,trajectoryPoints[i].velocity));
	    _rigidbody.velocity = trajectoryPoints[0].velocity;
	    this.RemoveCurrentStep();
	    this.CalculateSingleNextPoint();
	     stepsCalculatedCounter++;
    }
    // Start is called before the first frame update
    void Start()
    {
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
