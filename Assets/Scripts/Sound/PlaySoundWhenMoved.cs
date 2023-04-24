using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundWhenMoved : MonoBehaviour
{

    [SerializeField]
    private AudioSource onMovementSound;

    private Rigidbody2D rb;
    private float soundTimer, soundCooldown = 0.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!(Mathf.Abs(rb.velocity.x) - 0.1f < 0 || Mathf.Abs(rb.velocity.y) > 0.2f) && onMovementSound != null)
        {
            FootStepSound();
        }

    }

    private void FootStepSound()
    {
        soundTimer = soundTimer > 0 ? soundTimer - Time.deltaTime : 0;
        if (soundTimer == 0)
        {
            onMovementSound.PlayOneShot(onMovementSound.clip);
            soundTimer = soundCooldown;
        }
    }
}
