using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovableObject : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private UnityEvent onDropEvent;
    [SerializeField]
    private bool canBeDroppedAnywhere;
    [SerializeField]
    private Transform parentToMove;
    #endregion
    private Collider2D parentCollider; //lo desactivo mientras esta en la boca del perro para que no hayan choques raros,
    private Rigidbody2D parentRigidBody; //va a ser estatico siempre y cuando este agarrado por un pj
    private bool isGrabbedByPlayer;
    private ItemSocket itemSocket; 
    private Transform heldPosition; //transform de donde se va a quedar cuando este agarraro, pEj: La MouthPosition del perro
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        parentRigidBody = parentToMove.GetComponent<Rigidbody2D>();
        parentCollider = parentToMove.GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
        if (isGrabbedByPlayer)
        {
            parentToMove.position = heldPosition.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dog")&& heldPosition == null)
        {
            heldPosition = collision.GetComponent<DogMouth>().MouthPosition;
        }
        if (collision.CompareTag("Plug"))
        {
            itemSocket = collision.GetComponent<ItemSocket>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Plug"))
        {
            itemSocket = null;
        }
    }
    #endregion
    public void OnObjectInteraction()
    {
        if(isGrabbedByPlayer)
        {
            if (itemSocket != null)
            {
                isGrabbedByPlayer = false;

                transform.position = itemSocket.ConnectionPosition;
                itemSocket.Connect();
            }
            else if (canBeDroppedAnywhere)
            {
                parentCollider.enabled = true;
                isGrabbedByPlayer = false;

                parentRigidBody.bodyType = RigidbodyType2D.Dynamic;


                onDropEvent?.Invoke();
            }
        }
        else
        { 
            parentCollider.enabled = false;
            isGrabbedByPlayer= true;

            parentRigidBody.bodyType = RigidbodyType2D.Static;//para que se le quede en la boca y no se caiga
        }
    }
}
