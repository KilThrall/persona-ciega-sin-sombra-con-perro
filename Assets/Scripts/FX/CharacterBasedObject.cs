using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBasedObject : MonoBehaviour
{
    [SerializeField]
    private bool shouldBeSeenByBlind = true;
    [Tooltip("What component needs to be deactivated. Leave null for whole game object")]
    [SerializeField]
    private MonoBehaviour[] targetComponents;

    [SerializeField]
    private DistanceBasedSound source;

    private void Awake()
    {
        if (source == null)
        {
            source = GetComponent<DistanceBasedSound>();
        }
    }

    private void Start()
    {
        ActionsManager.SubscribeToAction(CameraManager.ON_CHARACTER_SWITCH_KEY, OnCharacterSwitch);
        OnCharacterSwitch(CameraManager.IsFollowingBlind);
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

        foreach (var targetComponent in targetComponents)
        {
            if (targetComponent != null)
            {
                targetComponent.enabled = isBlind == shouldBeSeenByBlind;
            }
        }

        source.Mute(!(isBlind == shouldBeSeenByBlind));
    }
}
