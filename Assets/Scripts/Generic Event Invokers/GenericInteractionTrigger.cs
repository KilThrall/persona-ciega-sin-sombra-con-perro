using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericInteractionTrigger : BaseInteractableObject
{
    #region Serialized Variables
    [SerializeField]
    private UnityEvent triggerEvent;
    [SerializeField]
    private bool useOnce;
    #endregion
    private bool used;
    protected override void OnPlayerInteraction(GameObject go)
    {
        if (useOnce)
        {
            if (!used)
            {
                triggerEvent.Invoke();
                used = true;
            }
        }
        else
        {
            triggerEvent.Invoke();
        }


    }
}