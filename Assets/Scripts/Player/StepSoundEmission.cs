using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StepSounds
{
    public string Tag => tag;
    public List<AudioClip> SoundClips
    {
        get
        {
            List<AudioClip> clipsCopy= soundClips;
            return clipsCopy;
        }
    }

    [SerializeField]
    private string tag;
    [SerializeField]
    private List<AudioClip> soundClips= new();
}

public class StepSoundEmission : MonoBehaviour
{
    public List<AudioClip> CurrentStepSounds
    {
        set
        {
            currentStepSounds=value;
        }
    }

    [SerializeField]
    private const string DEFAULT_FOOTSTEP_COLLECTION_TAG="Grass";
    [SerializeField]
    private AudioSource footStepsAudioSource;
    [SerializeField]
    private List<StepSounds> stepSounds = new();

    private Dictionary<string, StepSounds> soundsDictionary = new();
    private List<AudioClip> currentStepSounds;
    private void Awake()
    {
        foreach (var sS in stepSounds)
        {
            soundsDictionary.Add(sS.Tag,sS);
        }

        SetStepSounds(DEFAULT_FOOTSTEP_COLLECTION_TAG);

    }

    public void SetStepSounds(string key)
    {
        currentStepSounds = soundsDictionary[key].SoundClips;
    }

  

    public void PlayStepSound()
    {
        footStepsAudioSource.PlayOneShot(RandomClip());
    }
    private AudioClip RandomClip()
    {
        return currentStepSounds[Random.Range(0,currentStepSounds.Count)];
    }

}
