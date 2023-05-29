using UnityEngine;
using System;
public class ElectricDependantTrigger : GenericInteractionTrigger
{
    [SerializeField][Tooltip("Enchufe del cual depende el funcionamiento de este objeto interactuable")]
    private ItemSocket itemSocket;

    [SerializeField]
    private GenericSoundEmmiter notConnectedWireSoundEmmission;
 
    // se podria suscribir al evento de coneccion una funcion para que suene algo al conectarse
    protected override void OnPlayerInteraction(GameObject go)
    {
        if (itemSocket.ItemConnected)
        {
            triggerEvent.Invoke();
        }
        else
        {
            notConnectedWireSoundEmmission.EmitSound();
        }
    }
}
