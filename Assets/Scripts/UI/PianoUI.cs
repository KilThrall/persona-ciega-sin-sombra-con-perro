using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoUI : MonoBehaviour
{
    public const string PIANO_INTERACT_KEY = "PianoInteracted";
    public const string PIANO_PASSWORD_INPUT_KEY = "PianoPasswordInput";


    private void Awake()
    {
        ActionsManager.SubscribeToAction(PIANO_INTERACT_KEY, OnInteract);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ActionsManager.UnsubscribeToAction(PIANO_INTERACT_KEY, OnInteract);
    }

    public void PlaySound(AudioClip clip)
    {
        GameUIManager.Instance.PlaySoundOneShot(clip);
    }

    private void OnInteract(object obj)
    {
        bool hasInteracted = false;
        try
        {
            hasInteracted = (bool)obj;

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Tried sending an event for {nameof(PianoUI)} with the wrong type. Error: " + e);
            throw;
        }
        gameObject.SetActive(hasInteracted);
    }
}
