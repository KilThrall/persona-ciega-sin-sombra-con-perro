using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DistanceBasedSound : MonoBehaviour
{
    public float MaxDistance;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        var dist = Vector2.Distance(transform.position, Camera.main.transform.position);
        if (dist > MaxDistance)
        {
            source.volume = 0;
            return;
        }
        var vol = 1-(dist / MaxDistance);

        source.volume = vol;
    }
}
