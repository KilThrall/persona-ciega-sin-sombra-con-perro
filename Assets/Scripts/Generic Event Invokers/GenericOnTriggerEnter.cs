using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GenericOnTriggerEnter : MonoBehaviour
{
    [SerializeField][Tooltip("Tags de gameObjects que pueden triggerear el evento")]
    private string[] tags;
   
    [SerializeField]
    protected UnityEvent genericOntriggerEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckTags(collision.tag))
        {
            genericOntriggerEvent.Invoke();
        }
    }

    private bool CheckTags(string tagToCheck)
    {
        foreach (var t in tags)
        {
            if (t==tagToCheck)
            {
                return true;
            }
        }
        return false;
    }
}
