using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlanetCollision : MonoBehaviour
{
    public bool isDestroyable = true;
    public bool isColliding = false;
    public PlanetTypes planetType;
    CelestialBody celestialBody;
    // Start is called before the first frame update
    void Start()
    {
        this.celestialBody = GetComponent<CelestialBody>();
    }
    public static void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff);
    }

    
    private void OnCollisionEnter2D(Collision2D col)
    {
        
      //if col has component CelestialBody, then get the planet type
        if (col.gameObject.GetComponent<CelestialBody>() != null && !isColliding)
        {
            if (col.gameObject.GetComponent<Sun>() != null)
            {
                LevelManager.Current.celestialBodies.Remove(this.gameObject.GetComponent<CelestialBody>());
                Destroy(this.gameObject);
                
                DOVirtual.Float(0, 1, 0.3f, v => LevelManager.Current.bloomVolume.weight = v).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo);

                return;
            }

            Debug.Log("collision...");
            isColliding = true;
            col.gameObject.GetComponent<PlanetCollision>().isColliding = true;
            var otherPlanetType = col.gameObject.GetComponent<PlanetCollision>().planetType;
            var newPlanetType = PlanetCollisionsStore.GetNewPlanetType(this.planetType, otherPlanetType);
            Debug.Log(newPlanetType);
            if (newPlanetType != PlanetTypes.None)
            {
                var newPlanetPrefab = LevelManager.Current.basicPlanets.First(x => x.type == newPlanetType).obj;
                var newInstance = Instantiate(newPlanetPrefab, col.contacts[0].point, Quaternion.identity);
                //zzp
                // newInstance.GetComponent<StartVelocity>().startVelocity =
                //     celestialBody.GetVelocity() * celestialBody.mass +
                //     col.gameObject.GetComponent<CelestialBody>().GetVelocity() *
                //     col.gameObject.GetComponent<CelestialBody>().mass;
                newInstance.GetComponent<StartVelocity>().startVelocity =
                    celestialBody.GetComponent<Slingshoot>().lastTouchedTimestamp > col.gameObject.GetComponent<Slingshoot>().lastTouchedTimestamp?
                        celestialBody.GetVelocity():col.gameObject.GetComponent<CelestialBody>().GetVelocity();
                newInstance.GetComponent<CelestialBody>().RecalculateTrajectory(true);
                newInstance.GetComponent<PlanetCollision>().isColliding = true;
               
                LevelManager.Current.celestialBodies.Add(newInstance.GetComponent<CelestialBody>());
                if (FindObjectOfType<CatPositiveNewPlanetSpawns>().planetTypes.Contains(newPlanetType))
                {
                    FindObjectOfType<CatMoodService>().SetMood(CatMood.HAPPY);
                }
            }

            SpawnParticles();
            if (isDestroyable) Destroy(gameObject);
           if (col.gameObject.GetComponent<PlanetCollision>().isDestroyable) Destroy(col.gameObject);

            DOVirtual.Float(0, 1, 0.1f, v => LevelManager.Current.bloomVolume.weight = v).SetEase(Ease.OutQuart).SetLoops(2, LoopType.Yoyo);
            Camera.main.DOShakePosition(0.2f, 1f, 10, 90f, true);
            // col.gameObject.GetComponent<Explodable>().explode();
            // gameObject.GetComponent<Explodable>().explode();
            // foreach (var fragment in gameObject.GetComponent<Explodable>().fragments)
            // {
            //
            //     AddExplosionForce(fragment.GetComponent<Rigidbody2D>(),
            //         1, col.contacts[0].point, 15);
					       //
            // }
            //
       //     LevelManager.Current.UpdateCelestialBodiesListFromScene();
       if (isDestroyable)  LevelManager.Current.celestialBodies.Remove(this.gameObject.GetComponent<CelestialBody>());
       if (col.gameObject.GetComponent<PlanetCollision>().isDestroyable)    LevelManager.Current.celestialBodies.Remove(col.gameObject.GetComponent<CelestialBody>());
            LevelManager.Current.GetComponent<CatTarget>().ScanIfWon();
          
        }
    }

    private void SpawnParticles()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
