using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class BaseInteractableObject : MonoBehaviour
{ 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            collision.GetComponent<PlayerInput>().OnInteract += OnPlayerInteraction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            collision.GetComponent<PlayerInput>().OnInteract -= OnPlayerInteraction;
        }
    }


    protected abstract void OnPlayerInteraction();

}
