using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteractable : BaseInteractableObject
{
    #region Serialized Variables
    [SerializeField]
    GameObject door;
    #endregion
    protected override void OnPlayerInteraction()
    {
        door.SetActive(false);
    }
}
