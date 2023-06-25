using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PasswordUI : MonoBehaviour
{
    public const string PASSWORD_INTERACT_KEY = "PasswordInteracted";
    public const string PASSWORD_INPUT_KEY = "PasswordInput";
    public const string PASSWORD_FAIL_KEY = "PasswordFail";

    [SerializeField]
    private string actionKeyModifier;
    [SerializeField]
    private UnityEvent onPasswordFail;


    private void Awake()
    {
        ActionsManager.SubscribeToAction(GetModifiedKey(PASSWORD_INTERACT_KEY), OnInteract);
        ActionsManager.SubscribeToAction(GetModifiedKey(PASSWORD_FAIL_KEY), OnPasswordFail);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ActionsManager.UnsubscribeToAction(GetModifiedKey(PASSWORD_INTERACT_KEY), OnInteract);
        ActionsManager.UnsubscribeToAction(GetModifiedKey(PASSWORD_FAIL_KEY), OnPasswordFail);
    }

    public void PlaySound(AudioClip clip)
    {
        GameUIManager.Instance.PlaySoundOneShot(clip);
    }

    private void OnPasswordFail(object obj)
    {
        onPasswordFail?.Invoke();
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

    private string GetModifiedKey(string ogText)
    {
        return string.Concat(actionKeyModifier, ogText);
    }
}
