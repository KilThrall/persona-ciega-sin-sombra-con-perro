using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ItemSocket : MonoBehaviour
{
    public bool ItemConnected => itemConnected;
    public UnityEvent onConnectionEvent;

    [SerializeField][Tooltip("Donde se va a quedar el item/cable cuando quede enchufado, si es nulo va al centro del sprite")]
    private Transform connectionPosition;

    private bool itemConnected;

    public Vector3 ConnectionPosition
    {
        get
        {
            if (connectionPosition!=null)
            {
                return connectionPosition.position;
            }
            return transform.position;
        }
    } 
    public void Connect()
    {
        onConnectionEvent.Invoke();
        itemConnected = true;
    }
}