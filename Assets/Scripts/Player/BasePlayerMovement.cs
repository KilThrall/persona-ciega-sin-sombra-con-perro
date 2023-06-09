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
  

    #endregion

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
        }
        else
        {
            //que no corra
            desiredDir = 0;//sin esto patina cuando se baja de la escalera
            rb.velocity = Vector2.up * rb.velocity.y;//sin esto patina al subirse de la escalera
        }
    }
    #endregion

    public void StopPlayer()
    {
        OnMovementInput(0); // Para que este sea direccion registrada antes de dejar de detectar inputs
        rb.velocity = Vector2.up * rb.velocity.y; // Que su velocidad ignore su velocidad en X
    }

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
    /// <summary>
    /// Se establece la direccion hacia la que mira el personaje
    /// </summary>
    /// <param name="dir">Direccion, debe ser 1 o -1</param>
    public void SetFacingDirection(float dir)
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
    //estos metodos son para usarlos en el evento de animacion de transicion de la escalera de zona 1 a zona 2
    public void DisableWalk()
    {
        isWalkEnabled = false;
    }
    public void EnableWalk()
    {
        isWalkEnabled = true;
    }
}
