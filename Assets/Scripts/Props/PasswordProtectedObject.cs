using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PasswordProtectedObject : GenericInteractionToggle
{
    [Tooltip("The code needed to activate the event")]
    [SerializeField]
    private List<int> password;
    [SerializeField]
    private UnityEvent onPasswordCorrect;
    [SerializeField]
    private GameObject uiPrefab;

    private int currentAttempt = 0;

    private void Start()
    {
        GameUIManager.Instance.InstanceGameUI(uiPrefab);
        ActionsManager.SubscribeToAction(PasswordUI.PASSWORD_INPUT_KEY, OnPasswordInput);
    }

    private void OnDestroy()
    {
        ActionsManager.UnsubscribeToAction(PasswordUI.PASSWORD_INPUT_KEY, OnPasswordInput);
    }

    public void OnPlayerInteract(bool state)
    {
        if (currentAttempt >= password.Count)
        {
            return;
        }
        ActionsManager.InvokeAction(PasswordUI.PASSWORD_INTERACT_KEY, state);
    }

    protected override void OnPlayerExitTrigger()
    {
        ActionsManager.InvokeAction(PasswordUI.PASSWORD_INTERACT_KEY, false);
    }

    private void OnPasswordInput(object value)
    {

        if (currentAttempt >= password.Count)
        {
            return;
        }
        int input = -1;
        try
        {
            input = (int)value;

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Tried sending an event for {nameof(PasswordUI)} with the wrong type. Error: " + e);
            throw;
        }
        if (input == -1)
        {
            return;
        }

        if (input != password[currentAttempt])
        {
            ActionsManager.InvokeAction(PasswordUI.PASSWORD_FAIL_KEY, null);
            currentAttempt =0;
            return;
        }
        currentAttempt++;

        if (currentAttempt >= password.Count)
        {
            onPasswordCorrect.Invoke();
            OnPlayerExitTrigger();
        }
    }
}
