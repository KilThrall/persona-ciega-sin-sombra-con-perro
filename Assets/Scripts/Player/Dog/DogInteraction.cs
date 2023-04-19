using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogInteraction : MonoBehaviour
{
    private BaseInteractableObject toggle;
    private IInput dogInput;
    private IInput blindInput;
    private BasePlayerMovement dogMovement;
    bool mustWalkWithBlind;

    private void Awake()
    {
        toggle = GetComponent<BaseInteractableObject>();
        dogMovement = GetComponentInParent<BasePlayerMovement>();
    }

    public void BeginWalkWithBlind()
    {
        if (blindInput == null)
        {
            blindInput = toggle.Player.GetComponent<IInput>();
            dogInput = dogMovement.gameObject.GetComponent<IInput>();
        }
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
