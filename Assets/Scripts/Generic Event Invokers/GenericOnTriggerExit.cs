using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GenericOnTriggerExit : MonoBehaviour
{
    [SerializeField][Tooltip("Tags de gameObjects que pueden triggerear el evento")]
    private string[] tags;
   
    [SerializeField]
    protected UnityEvent genericOntriggerExitEvent;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckTags(collision.tag))
        {
            genericOntriggerExitEvent?.Invoke();
        }
    }

    protected bool CheckTags(string tagToCheck)
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
