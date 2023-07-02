using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundsForCinematic : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] cinematicAudioClips;
    private int index;
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Se llamará mediante un AnimationEvent
    /// </summary>
    public void PlaySound()
    {
        if (index>cinematicAudioClips.Length)
        {
            return;
        }

        source.PlayOneShot(cinematicAudioClips[index]);
        index++;
    }
}
