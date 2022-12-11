using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateSprite : MonoBehaviour
{
    public float rate = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation = Quaternion.Euler(0, 0, this.transform.localRotation.eulerAngles.z + rate);
    }
}
