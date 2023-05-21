using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSoundChanger : GenericOnTriggerEnter
{
    [SerializeField]
    private AudioSource ambienceSoundSource;
    [SerializeField]
    private AudioClip newAudioClip;

    private void Awake()
    {
        genericOntriggerEvent.AddListener(SetSound);
    }

    private void SetSound()
    {
        ambienceSoundSource.clip = newAudioClip;
        ambienceSoundSource.Play();
    }
}
