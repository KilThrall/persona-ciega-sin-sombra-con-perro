using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogInteraction : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private BasePlayerMovement dogMovement;
    #endregion
    private BaseInteractableObject toggle;
    private IInput dogInput;
    private IInput blindInput;

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        toggle = GetComponent<BaseInteractableObject>();
    }
    #endregion

    public void BeginWalkWithBlind()
    {
        if (blindInput == null)
        {
            blindInput = toggle.Player.GetComponent<IInput>();
            dogInput = dogMovement.gameObject.GetComponent<IInput>();
        }
        //TODO: ver como dejar esto mas lindo
        dogInput.OnDisabled += StopWalkingWitBlind;
        blindInput.OnMovementInput += dogMovement.OnMovementInput;
        blindInput.OnInteract += StopWalkingWitBlind;
        blindInput.OnJump += RemoveListeners;
    }
    public void RemoveListeners(bool t)
    {
        blindInput.OnJump -= RemoveListeners;
        StopWalkingWitBlind();
    }
    public void StopWalkingWitBlind()
    {
        blindInput.OnMovementInput -= dogMovement.OnMovementInput;
        blindInput.OnInteract -= StopWalkingWitBlind;
        dogInput.OnDisabled -= StopWalkingWitBlind;
    }
}
