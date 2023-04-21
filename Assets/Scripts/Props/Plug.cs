using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Plug : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onConnectionEvent;

    public void Connect()
    {
        onConnectionEvent.Invoke();
    }
}