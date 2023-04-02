using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericInteractionTrigger : BaseInteractableObject
{
    #region Serialized Variables
    [SerializeField]
    private UnityEvent triggerEvent;
    #endregion
    protected override void OnPlayerInteraction()
    {
        triggerEvent.Invoke();
    }
}
