using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovableObject : MonoBehaviour
{
    /// <summary>
    /// De momento solo se puede soltar el item si se está donde se tiene que dejar
    /// </summary>

    [SerializeField]
    private UnityEvent onDropEvent;
    [SerializeField]
    private bool canBeDroppedAnywhere;
    private bool isGrabbedByPlayer;
    public Plug plug; // Use el plug que se usa para la conección del cable, el nombre por ahi se podría que cambiar
    private Transform holdedPosition; //transform de donde se va a quedar cuando este agarraro, pEj: La MouthPosition del perro
    #region MonoBehaviour Callbacks

    private void FixedUpdate()
    {
        if (isGrabbedByPlayer)
        {
            transform.position = holdedPosition.position;
        }
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
    public void OnObjectInteraction()
    {
        if(isGrabbedByPlayer)
        {
            if (plug != null)
            {
                isGrabbedByPlayer = false;
                transform.position = plug.ConnectionPosition;
                plug.Connect();
            }
            else if (canBeDroppedAnywhere)
            {
                isGrabbedByPlayer = false;
                onDropEvent?.Invoke();
            }
        }
        else
        {
            isGrabbedByPlayer = true;
        }
    }
}
