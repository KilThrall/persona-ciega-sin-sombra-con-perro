using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordUI : MonoBehaviour
{
    public const string PASSWORD_INTERACT_KEY = "PasswordInteracted";
    public const string PASSWORD_INPUT_KEY = "PasswordInput";


    private void Awake()
    {
        ActionsManager.SubscribeToAction(PASSWORD_INTERACT_KEY, OnInteract);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ActionsManager.UnsubscribeToAction(PASSWORD_INTERACT_KEY, OnInteract);
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
            Debug.LogError($"Tried sending an event for {nameof(PasswordUI)} with the wrong type. Error: " + e);
            throw;
        }
        gameObject.SetActive(hasInteracted);
    }
}
