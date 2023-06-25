using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordButton : MonoBehaviour
{
    [SerializeField]
    private int position;

    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private PasswordUI ui;
    [SerializeField]
    private string actionKeyModifier;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        ActionsManager.InvokeAction(GetModifiedKey(PasswordUI.PASSWORD_INPUT_KEY), position);
        if (clip == null)
        {
            Debug.LogWarning("No clip found for button");
            return;
        }
        ui.PlaySound(clip);
    }
    private string GetModifiedKey(string ogText)
    {
        return string.Concat(actionKeyModifier, ogText);
    }
}
