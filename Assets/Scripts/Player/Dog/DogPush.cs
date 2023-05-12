using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPush : MonoBehaviour
{
    private Transform pushedObject;
    private const string PUSHEABLE_TAG = "Pusheable";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PUSHEABLE_TAG))
        {
            collision.transform.SetParent(transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(PUSHEABLE_TAG))
        {
            collision.transform.SetParent(null);
        }

    }
}
