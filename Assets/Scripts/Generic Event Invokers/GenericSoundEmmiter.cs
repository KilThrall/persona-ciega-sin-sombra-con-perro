/// <summary>
/// Se puede usar para llamar solamente el EmitSound o para que emita sonidos cada X tiempo
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSoundEmmiter : MonoBehaviour
{
    #region SerializedVariables
    [SerializeField]
    private Transform parentToFollow;
    [SerializeField]
    private Transform spawnPosition;
    [SerializeField]
    private GameObject soundPrefab;


    [Header("Repetition")]
    [SerializeField]
    private bool mustRepeatSound;
    [SerializeField]
    private float soundRepetitionCooldown=3;

    #endregion
    
    private float soundRepetitionTimer;
    #region Monobehaviour Callbacks
    private void FixedUpdate()
    {
        if (mustRepeatSound)
        {
            soundRepetitionTimer = soundRepetitionTimer < 0 ? 0 : soundRepetitionTimer - Time.fixedDeltaTime;

            if (soundRepetitionTimer<=0)
            {
                soundRepetitionTimer = soundRepetitionCooldown;
                EmitSound();
            }
        }
    }
    #endregion
    public void EmitSound()
    {
        if (parentToFollow == null)
        {
            Instantiate(soundPrefab, spawnPosition.position, Quaternion.identity);
        }
        else
        {
            Instantiate(soundPrefab, spawnPosition.position, Quaternion.identity, parentToFollow);
        }
    }

    public void ResetTimer()
    {
        soundRepetitionTimer = 0;
    }
}
