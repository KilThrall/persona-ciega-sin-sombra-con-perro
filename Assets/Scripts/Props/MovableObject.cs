using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public void OnObjectInteraction()
    {

    }

    #region Serialized Variables
    [SerializeField]
    private Rope rope;
    #endregion
    private bool isGrabbedByPlayer;

    private GenericInteractionTrigger trigger;
    private Plug plug;
    private Transform holdedPosition;
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        trigger = GetComponent<GenericInteractionTrigger>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog")&& holdedPosition==null)
        {
            holdedPosition = collision.GetComponent<DogMouth>().MouthPosition;
        }
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
    #endregion

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
}
