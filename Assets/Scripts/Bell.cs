using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public GameObject LightPrefab;

    public Transform SpawnPos;

    public void CreateLight()
    {
        Instantiate(LightPrefab, SpawnPos.position, Quaternion.identity);
    }
}
