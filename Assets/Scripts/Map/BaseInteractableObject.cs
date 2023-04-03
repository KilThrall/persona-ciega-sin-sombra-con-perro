using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class BaseInteractableObject : MonoBehaviour
{
    public event Action OnPlayerLeaveTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            collision.GetComponent<PlayerInput>().OnInteractUE.AddListener(delegate {OnPlayerInteraction(collision.gameObject);});
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            collision.GetComponent<PlayerInput>().OnInteractUE.RemoveAllListeners();
            OnPlayerLeaveTrigger?.Invoke();
        }

    }
    
    protected abstract void OnPlayerInteraction(GameObject go);

}
