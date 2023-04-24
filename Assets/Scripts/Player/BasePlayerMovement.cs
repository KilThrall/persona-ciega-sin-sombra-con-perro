using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInput))]
public class BasePlayerMovement : MonoBehaviour
{
    #region Serialized variables

    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float acceleration = 3;
    [SerializeField]
    private ContactBasedLight[] footstepLights;
    [SerializeField]
    private AudioSource footStepAudioSource;

    #endregion

    //AGREGADO SIN PR!!!
    private float soundTimer, soundCooldown=0.5f;

    private IInput input;
    private Rigidbody2D rb;
    private Animator anim;
    private float desiredDir;
    private float facingDir = 1;

    #region MonoBehaviour callbacks
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<IInput>();
        anim = GetComponent<Animator>();
        input.OnMovementInput += OnMovementInput;
        input.OnDisabled += StopPlayer;
    }
    private void OnDestroy()
    {
        input.OnMovementInput -= OnMovementInput;
        input.OnDisabled -= StopPlayer;
    }


    private void FixedUpdate()
    {
        var dirDif = desiredDir*moveSpeed - rb.velocity.x;
        var resultingSpeed = dirDif * acceleration * Time.deltaTime;
        resultingSpeed = Mathf.Clamp(resultingSpeed, -moveSpeed, moveSpeed);
        rb.velocity = new Vector2(rb.velocity.x + resultingSpeed, rb.velocity.y);
        //CAMBIADO SIN PR !!!!!
        if ( !(Mathf.Abs(rb.velocity.x) - 0.1f < 0 || Mathf.Abs(rb.velocity.y) > 0.2f) && footStepAudioSource!=null)
        {
            FootStepSound();
        }
        //---------
    }

    #endregion
    //AGREGADO SIN PR!!!!!
    private void FootStepSound()
    {
        soundTimer = soundTimer > 0 ? soundTimer - Time.deltaTime : 0;
        if(soundTimer==0)
        {
            footStepAudioSource.PlayOneShot(footStepAudioSource.clip);
            soundTimer = soundCooldown;
        }
    }
    public void StopPlayer()
    {
        OnMovementInput(0);
        rb.velocity = Vector2.up * rb.velocity.y;
    }

    private void ChangeFootstepsStatus(bool state)
    {
        foreach (var item in footstepLights)
        {
            item.enabled = state; 
        }
    }

    public void OnMovementInput(float dir)
    {
       
        if (dir < 0)
        {
            anim.SetBool("Running", true);
            dir = -1;
        }else if (dir > 0)
        {
            anim.SetBool("Running", true);
            dir = 1;
        }
        else
        {
            anim.SetBool("Running", false);
        }
        desiredDir = dir;
        if (dir != 0 && facingDir != dir)
        {
            facingDir = dir;
            anim.SetTrigger("Rotate");
        }

        if (dir!=0)
        {
            if (transform.localScale.x != dir)
            {
                Vector2 scale = transform.localScale;
                scale.x = dir;
                transform.localScale = scale;
            }
        }

    }

    public float GetDesiredDir()
    {
        return desiredDir;
    }
}
