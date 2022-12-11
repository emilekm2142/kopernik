using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = System.Random;

public class Slingshoot : MonoBehaviour
{
    public float maxDistanceFromSun = 10f;
    public long lastTouchedTimestamp = 0;
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
      //  UnityEngine.Cursor.SetCursor(LevelManager.Current.cursorTexture, Vector2.zero, CursorMode.Auto);

        celestialBody= GetComponent<CelestialBody>();
        var spriteCmp = GetComponentInChildren<SpriteRenderer>();
        if (celestialBody.doesMove)
        {
            spriteCmp.transform.localScale *= LevelManager.Current.planetSizeCoefficient+UnityEngine.Random.Range(-LevelManager.Current.planetSizeVariation,LevelManager.Current.planetSizeVariation );
        }

        var targetScale = spriteCmp.transform.localScale;
        spriteCmp.transform.localScale = Vector3.zero;
        spriteCmp.transform.DOScale(targetScale, 1f).SetEase(Ease.OutBack);
        arrowSprite = LevelManager.Current.arrow.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<PlanetCollision>().isDestroyable && LevelManager.Current.sun != null && Vector2.Distance(LevelManager.Current.sun.transform.position, this.transform.position) > maxDistanceFromSun)
        {
            LevelManager.Current.celestialBodies.Remove(this.gameObject.GetComponent<CelestialBody>());
            Destroy(this.gameObject);
        }
        if (this.draggedCount > 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1) &&LevelManager.Current.isPaused)
        {
       //     UnityEngine.Cursor.SetCursor(LevelManager.Current.cursorTexture, Vector2.zero, CursorMode.Auto);

            isBeingDragged = false;
            LevelManager.Current.isAnyBeingDragged = false;
            celestialBody.RecalculateTrajectory(true);
        }
        if (Input.GetMouseButtonUp(0) && LevelManager.Current.isPaused)
        {
            if (LevelManager.Current.isAnyBeingDragged && isBeingDragged)
            {
             //   UnityEngine.Cursor.SetCursor(LevelManager.Current.cursorTexture, Vector2.zero, CursorMode.Auto);
                isBeingDragged = false;
                LevelManager.Current.isAnyBeingDragged = false;
                LevelManager.Current.Resume();
                draggedCount++;
                DOVirtual.Vector3(arrowSprite.transform.localScale, Vector3.zero, 0.1f,
                    v =>arrowSprite.transform.localScale = v).SetEase(Ease.InBounce);
                Camera.main.DOShakePosition(0.4f, 0.4f, 10, 90f, true);
            }
        }

        if (Input.GetMouseButton(0) && isBeingDragged && LevelManager.Current.isPaused )
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
            lastTouchedTimestamp = DateTime.Now.Ticks;
          
            celestialBody.RecalculateTrajectory(true, x,y, true);
        }
    }

    private void OnMouseDown()
    {
        if (LevelManager.Current.isAnyBeingDragged == false && LevelManager.Current.isPaused)
        {
         //   UnityEngine.Cursor.SetCursor(LevelManager.Current.cursorTextureDown, Vector2.zero, CursorMode.Auto);

            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isBeingDragged = true;
            LevelManager.Current.isAnyBeingDragged = true;
        }
    }
    
}
