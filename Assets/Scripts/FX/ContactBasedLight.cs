using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactBasedLight : MonoBehaviour
{
    public GameObject LightPrefab;
    public Transform LightPosition;

    public float DelayForNextLight, OffsetForFirstLight;

    private float timeForNextLight;

    private void Awake()
    {
        timeForNextLight = OffsetForFirstLight;
    }

    private void FixedUpdate()
    {
        timeForNextLight -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        timeForNextLight = OffsetForFirstLight;
        if (DelayForNextLight == -1)
        {
            var pos = collision.GetContact(0).point;

            SpawnLight(pos);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (timeForNextLight > 0 || DelayForNextLight == -1)
        {
            return;
        }
        var pos = collision.GetContact(0).point;
        
        SpawnLight(pos);
    }

    private void SpawnLight(Vector3 pos)
    {
        if (LightPosition != null)
        {
            pos = LightPosition.position;
        }
        Instantiate(LightPrefab, pos, Quaternion.identity);
        timeForNextLight = DelayForNextLight;
    }
}
