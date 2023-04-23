using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoUI : MonoBehaviour
{
    public const string PIANO_INTERACT_KEY = "PianoInteracted";

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        ActionsManager.SubscribeToAction(PIANO_INTERACT_KEY, OnInteract);
        gameObject.SetActive(false);
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError($"Tried to play a sound, but no audio source was found on {nameof(PianoUI)}");
            return;
        }
        audioSource.PlayOneShot(clip);
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
