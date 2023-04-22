using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PianoButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private PianoUI ui;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        ui.PlaySound(clip);
    }
}
