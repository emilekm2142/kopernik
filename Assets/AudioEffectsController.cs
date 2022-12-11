using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffectsController : MonoBehaviour
{
    public AudioSource audioSource;
    public Vector2 volumeRange;
    public Vector2 pitchRange;
    public AudioClip planetHitSound;
    // Start is called before the first frame update
    void Start()
    {
     this.audioSource= GetComponent<AudioSource>();   
    }

    public void PlayPlanetCollide()
    {
        //randomize volume and pitch
        float volume = Random.Range(volumeRange.x, volumeRange.y);
        float pitch = Random.Range(pitchRange.x, pitchRange.y);
        //play sound
        audioSource.pitch= pitch;
        audioSource.volume= volume;
        audioSource.PlayOneShot(planetHitSound);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
