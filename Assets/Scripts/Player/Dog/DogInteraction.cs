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
    private BasePlayerMovement blindMovement;
    #region MonoBehaviour Callbacks

    private GameObject player;
    private bool walkingWithBlind;
    private void Awake()
    {
        toggle = GetComponent<BaseInteractableObject>();
        player=toggle.Player;
    }
    #endregion

    public void InteractWithDog()
    {
        if (walkingWithBlind)
        {
            StopWalkingWitBlind();
        }
        else
        {
            BeginWalkWithBlind();
        }
    }

    private void BeginWalkWithBlind()
    {
        if (blindInput == null)
        {
            blindInput = toggle.Player.GetComponent<IInput>();
            blindMovement = toggle.Player.GetComponent<BasePlayerMovement>();
            dogInput = dogMovement.gameObject.GetComponent<IInput>();
        }
        //TODO: ver como dejar esto mas lindo
        dogMovement.transform.position = toggle.Player.transform.position;
        blindMovement.StopPlayer();

        dogInput.OnDisabled += StopWalkingWitBlind;
        blindInput.OnMovementInput += dogMovement.OnMovementInput;
        blindInput.OnInteract += StopWalkingWitBlind;
        blindInput.OnJump += RemoveListeners;

        walkingWithBlind=true;
    }
    public void RemoveListeners(bool t)
    {
        blindInput.OnJump -= RemoveListeners;
        StopWalkingWitBlind();
    }
    private void StopWalkingWitBlind()
    {
        dogMovement.StopPlayer();
        blindInput.OnMovementInput -= dogMovement.OnMovementInput;
        blindInput.OnInteract -= StopWalkingWitBlind;
        dogInput.OnDisabled -= StopWalkingWitBlind;

        walkingWithBlind = false;
    }
}
