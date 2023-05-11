using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBasedObject : MonoBehaviour
{
    [SerializeField]
    private bool shouldBeSeenByBlind = true;
    [Tooltip("What component needs to be deactivated. Leave null for whole game object")]
    [SerializeField]
    private MonoBehaviour targetComponent;

    private void Start()
    {
        ActionsManager.SubscribeToAction(CameraManager.ON_CHARACTER_SWITCH_KEY, OnCharacterSwitch);
    }

    private void OnDestroy()
    {
        ActionsManager.UnsubscribeToAction(CameraManager.ON_CHARACTER_SWITCH_KEY, OnCharacterSwitch);
    }

    private void OnCharacterSwitch(object status)
    {
        bool isBlind = false;
        try
        {
            isBlind = (bool)status;

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Tried sending an event for {nameof(CharacterBasedObject)} with the wrong type. Error: " + e);
            throw;
        }

        if (targetComponent != null)
        {
            targetComponent.enabled = isBlind == shouldBeSeenByBlind;
        }
        else
        {
            gameObject.SetActive(isBlind == shouldBeSeenByBlind);
        }
    }
}
