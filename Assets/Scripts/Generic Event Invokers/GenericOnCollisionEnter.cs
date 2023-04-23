using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GenericOnCollisionEnter : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Tags de gameObjects que pueden triggerear el evento")]
    private string[] tags;

    [SerializeField]
    private UnityEvent genericOntriggerEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckTags(collision.gameObject.tag))
        {
            genericOntriggerEvent.Invoke();
        }
    }
  
    private bool CheckTags(string tagToCheck)
    {
        foreach (var t in tags)
        {
            if (t == tagToCheck)
            {
                return true;
            }
        }
        return false;
    }
}
