using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalFootstepSoundAsigner : MonoBehaviour
{
    [SerializeField]
    private StepSounds customSoundList;
    private StepSoundEmission playerStepSoundEmission;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Blind"))
        {
            if (playerStepSoundEmission==null)
            {
                playerStepSoundEmission = collision.GetComponent<StepSoundEmission>();
            }
            AssignSound();
        }
    }

    public void AssignSound()
    {
        playerStepSoundEmission.CurrentStepSounds = customSoundList.SoundClips;
    }
}
