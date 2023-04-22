using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : GenericInteractionToggle
{
    [SerializeField]
    private GameObject uiPrefab;

    private void Start()
    {
        GameUIManager.Instance.InstanceGameUI(uiPrefab);
    }

    public void OnPlayerInteract(bool state)
    {
        ActionsManager.InvokeAction(PianoUI.PIANO_INTERACT_KEY, state);
    }

    protected override void OnPlayerExitTrigger()
    {
        ActionsManager.InvokeAction(PianoUI.PIANO_INTERACT_KEY, false);
    }
}
