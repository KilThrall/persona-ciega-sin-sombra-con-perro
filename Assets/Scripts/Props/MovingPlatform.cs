using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private Transform parentTransform;

    private void Start()
    {
        if (parentTransform == null)
        {
            parentTransform = transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.SetParent(parentTransform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.transform.SetParent(null);
    }
}
