using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInput))]
public class BasePlayerMovement : MonoBehaviour
{
    public bool IsWalkEnabled
    {
        get => isWalkEnabled;
        set { 
            isWalkEnabled = value;
            if (!value)
            {
                SetFacingDirection(1);//con esto mira para el mismo lado siempre
                anim.SetBool("Running", false); 
            } 
        }
    }

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
    //CAMBIADO SIN PR!!!!!
    private float soundTimer, soundCooldown=1f;

    private IInput input;
    private Rigidbody2D rb;
    private Animator anim;
    private float desiredDir;
    private float facingDir = 1;
    private bool isWalkEnabled=true;

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
        if (isWalkEnabled)
        {
            var dirDif = desiredDir * moveSpeed - rb.velocity.x;
            var resultingSpeed = dirDif * acceleration * Time.deltaTime;
            resultingSpeed = Mathf.Clamp(resultingSpeed, -moveSpeed, moveSpeed);
            rb.velocity = new Vector2(rb.velocity.x + resultingSpeed, rb.velocity.y);
            if (!(Mathf.Abs(rb.velocity.x) - 0.1f < 0 || Mathf.Abs(rb.velocity.y) > 0.2f) && footStepAudioSource != null)
            {
                //CAMBIADO SIN PR!!!!
                FootStepSound();
            }
        }
        else
        {
            //que no corra
            desiredDir = 0;//sin esto patina cuando se baja de la escalera
            rb.velocity = Vector2.up * rb.velocity.y;//sin esto patina al subirse de la escalera
        }
    }
    #endregion
    //AGREGADO SIN PR!!!
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
    //PARA EL CAMBIO DE ANIMACION PARA EL PERRO 
    public void OnMovementInput(float dir)
    {
        if (!isWalkEnabled)
        {
            return;
        }

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

            SetFacingDirection(dir);
            
        }

    }

    private void SetFacingDirection(float dir)
    {
        if (transform.localScale.x != dir)
        {
            Vector2 scale = transform.localScale;
            scale.x = dir;
            transform.localScale = scale;
        }

    }

    public float GetDesiredDir()
    {
        return desiredDir;
    }
}
