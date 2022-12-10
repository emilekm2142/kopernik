using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class Slingshoot : MonoBehaviour
{
    public int draggedCount = 0;
    public float maxForce = 10f;
    public float maxDistance = 10f;
    private SpriteRenderer arrowSprite;
    public Vector2 startMousePos;
    public bool isBeingDragged = false;
    public CelestialBody celestialBody;
    // Start is called before the first frame update
    void Start()
    {
        celestialBody= GetComponent<CelestialBody>();
        arrowSprite = LevelManager.Current.arrow.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isBeingDragged = false;
            LevelManager.Current.isAnyBeingDragged = false;
            celestialBody.RecalculateTrajectory(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (LevelManager.Current.isAnyBeingDragged && isBeingDragged)
            {
                isBeingDragged = false;
                LevelManager.Current.isAnyBeingDragged = false;
                LevelManager.Current.Resume();
                draggedCount++;
            }
        }

        if (Input.GetMouseButton(0) && isBeingDragged)
        {
            //get mouse world position
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //get direction
            Vector2 direction = mousePos - startMousePos;
            //clamp the lenght of this vector to maxDistance
            direction = Vector2.ClampMagnitude(direction, maxForce);
            //invert direction
            direction = -direction;
            
            //get distance
            float distance = Vector2.Distance(mousePos, startMousePos);
            //get distance of each component
            float x = direction.x;
            float y = direction.y;
            
            //set max distance
            
            //clamp distance
            distance = Mathf.Clamp(distance, 0, maxDistance);
            arrowSprite.color = Color.red*Mathf.Lerp(0,1,distance/maxDistance);
            arrowSprite.transform.localScale= new Vector3(1*Mathf.Lerp(0,1,distance/maxDistance),1,1);
            //set arrow position
            LevelManager.Current.arrow.transform.position = startMousePos;
            //set arrow rotation
            LevelManager.Current.arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            
            celestialBody.RecalculateTrajectory(true, x,y, true);
        }
    }

    private void OnMouseDown()
    {
        if (LevelManager.Current.isAnyBeingDragged == false)
        {
            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isBeingDragged = true;
            LevelManager.Current.isAnyBeingDragged = true;
        }
    }
    
}
