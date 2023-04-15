using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    //TODO: ESTE SCRITP DEBE ADMINISTRAR SI ESTA SIENDO AGARRADO POR EL PLAYER, Y ES A QUIEN SE DEBE PREGUNTAR ESTO AL MOMENTO DE QUE EL PLAYER INTERACTUE CON PLUG.CS
    private bool isGrabbedByPlayer;

    private Rope rope;
    private GenericInteractionTrigger trigger;

    private Plug plug;
    private void Awake()
    {
        rope = GetComponentInParent<Rope>();
        trigger = GetComponent<GenericInteractionTrigger>();
    }

    public void PlayerInteraction()
    {
        if (isGrabbedByPlayer)
        {
            if (plug == null)
            {
                DropWire();
                rope.DropRope();
            }
            else
            {
                DropWire();
                rope.SetEndPoint(plug.transform);
                plug.Connect();
            }
         
        }
        else
        {
            GrabWire();

        }
    }


    public bool CheckGrabState()
    {
        return isGrabbedByPlayer;
    }

    public void GrabWire()
    {
        isGrabbedByPlayer = true;
        rope.SetEndPoint(trigger.Player.transform);
    }
    public void DropWire()
    {
        isGrabbedByPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
 
        if (collision.CompareTag("Plug"))
        {
            plug = collision.GetComponent<Plug>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Plug"))
        {
            plug = null;
        }
    }
}
