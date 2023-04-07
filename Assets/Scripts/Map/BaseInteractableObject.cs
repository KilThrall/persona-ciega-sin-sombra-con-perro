using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class BaseInteractableObject : MonoBehaviour
{
    //(1/3)
    //PARA EVITAR USAR DELEGADOS SIN CONOCMIENTO GUARDO UNA REF AL PLAYER 
    protected GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            if (player==null)
            {
                player = collision.gameObject;
            }
            collision.GetComponent<IInput>().OnInteract +=OnPlayerInteraction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            //(2/3)
            //COMO NO SE USA UN DELEGATE AHORA SE BORRA BIEN TODO
            collision.GetComponent<IInput>().OnInteract -= OnPlayerInteraction;
            OnPlayerExitTrigger();
        }

    }
    //(3/3)
    //LA FUNCION LLAMADA LLAMA A SU VEZ A UNA HOMONIMA, DE PARAMETRO LA PASA LA REFERENCIA AL PLAYER
    public void OnPlayerInteraction()
    {
        OnPlayerInteraction(player);
    }
    protected abstract void OnPlayerExitTrigger();
    protected abstract void OnPlayerInteraction(GameObject go);
    
}
