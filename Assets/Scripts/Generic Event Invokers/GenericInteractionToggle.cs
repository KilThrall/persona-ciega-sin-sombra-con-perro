using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericInteractionToggle : BaseInteractableObject
{

    #region Serialized Variables
    [SerializeField]
    protected UnityEvent OnEvent;
    [SerializeField]
    protected UnityEvent OffEvent;
    #endregion
    private bool isToggled;



    protected override void OnPlayerInteraction(GameObject go)
    {
        if (isToggled)
        {
            OffEvent.Invoke();
        }
        else
        {
            OnEvent.Invoke();
        }
        isToggled = !isToggled;
    }

}