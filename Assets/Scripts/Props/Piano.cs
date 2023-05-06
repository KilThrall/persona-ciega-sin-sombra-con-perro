using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Piano : GenericInteractionToggle
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
        ActionsManager.SubscribeToAction(PianoUI.PIANO_PASSWORD_INPUT_KEY, OnPianoPasswordInput);
    }

    private void OnDestroy()
    {
        ActionsManager.UnsubscribeToAction(PianoUI.PIANO_PASSWORD_INPUT_KEY, OnPianoPasswordInput);
    }

    public void OnPlayerInteract(bool state)
    {
        if (currentAttempt >= password.Count)
        {
            return;
        }
        ActionsManager.InvokeAction(PianoUI.PIANO_INTERACT_KEY, state);
    }

    protected override void OnPlayerExitTrigger()
    {
        ActionsManager.InvokeAction(PianoUI.PIANO_INTERACT_KEY, false);
    }

    private void OnPianoPasswordInput(object value)
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
            Debug.LogError($"Tried sending an event for {nameof(PianoUI)} with the wrong type. Error: " + e);
            throw;
        }
        if (input == -1)
        {
            return;
        }

        if (input != password[currentAttempt])
        {
            currentAttempt=0;
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
