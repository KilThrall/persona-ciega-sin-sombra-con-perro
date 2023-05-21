using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Transform cameraT;
    public float parallaxEffectMultiplier;
    public bool ignoreY;
    Vector3 lastPos;
    float width;
    // Start is called before the first frame update
    void Start()
    {
        cameraT = Camera.main.transform;
        //parallaxEffectMultiplier= Mathf.Clamp(0, 1, parallaxEffectMultiplier);
        lastPos = cameraT.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ignoreY)
        {

            Vector3 deltaMov = cameraT.position - lastPos;

            transform.position += deltaMov * parallaxEffectMultiplier;

            lastPos = cameraT.position;
        }
        else
        {
            Vector3 deltaMov = cameraT.position - lastPos;

            transform.position += new Vector3(deltaMov.x,0) * parallaxEffectMultiplier;

            lastPos = cameraT.position;
        }
    }
}
