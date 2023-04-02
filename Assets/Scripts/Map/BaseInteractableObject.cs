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
            collision.GetComponent<PlayerInput>().OnInteract +=  delegate {OnPlayerInteraction(collision.gameObject);};
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            collision.GetComponent<PlayerInput>().OnInteract -= delegate { OnPlayerInteraction(collision.gameObject); };
        }
    }


    protected abstract void OnPlayerInteraction(GameObject go);

}
