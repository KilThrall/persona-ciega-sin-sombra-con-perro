using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float Lifetime;

    private void FixedUpdate()
    {
        Lifetime -= Time.deltaTime;
        if(Lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
