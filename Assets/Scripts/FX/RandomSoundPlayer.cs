using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    public AudioClip[] clips;

    public bool PlayOnAwake = true;

    private AudioSource source;

    private void Awake()
    {
        source =GetComponent<AudioSource>();

        if (!PlayOnAwake)
        {
            return;
        }
        PlayRandom();
    }

    public void PlayRandom()
    {
        var num = Random.Range(0, clips.Length);

        source.clip = clips[num];
        source.Play();
    }
}
