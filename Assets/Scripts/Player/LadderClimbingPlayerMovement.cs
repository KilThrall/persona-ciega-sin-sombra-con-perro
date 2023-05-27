using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInput))]
public class LadderClimbingPlayerMovement : MonoBehaviour
{
    #region Serialized variables
    [SerializeField]
    private string characterLayerName ="Character";
  
    [SerializeField]
    private string characterClimbingLayerName = "CharacterClimbing";
    [SerializeField]
    private float climbSpeed = 5;
    [SerializeField]
    private float acceleration = 3;
    [SerializeField]
    private ContactBasedLight[] handLights;
    //TODO: Cuando trepe que hagan sonido sus manos tambien, permitiendo ver un poco para arriba
    #endregion
    private const string LADDER_CLIMB_ANIMATOR_PARAMETER= "LadderClimbing";
    private int characterLayer = 3;
    private int characterClimbingLayer = 9;
    private IInput input;
    private Rigidbody2D rb;
    private Animator anim;
    private BasePlayerMovement basePlayerMovement;
    private float desiredDir;
    private bool isClimbingLadder;

    private int leftDirection=1;

    #region MonoBehaviour callbacks
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<IInput>();
        anim = GetComponent<Animator>();
        basePlayerMovement = GetComponent<BasePlayerMovement>();
        characterLayer= LayerMask.NameToLayer(characterLayerName);
        characterClimbingLayer= LayerMask.NameToLayer(characterClimbingLayerName);
        input.OnVerticalMovementInput += OnVerticalMovementInput;
    }

    private void FixedUpdate()
    {
        if (isClimbingLadder)
        {
            //Calculo de fisicas del ciego al subir la escalera
            var dirDif = desiredDir * climbSpeed - rb.velocity.x;
            var resultingSpeed = dirDif * acceleration * Time.deltaTime;
            resultingSpeed = Mathf.Clamp(resultingSpeed, -climbSpeed, climbSpeed);
            rb.velocity = new Vector2(rb.velocity.x, resultingSpeed);

            if (Mathf.Abs(rb.velocity.x) - 0.1f < 0 || Mathf.Abs(rb.velocity.y) > 0.2f)
            {
                ChangeHandStatus(false);
            }
            else
            {
                ChangeHandStatus(true);
            }
        }
    }

    private void OnDestroy()
    {
        input.OnVerticalMovementInput -= OnVerticalMovementInput;
    }
    #endregion
    /// <summary>
    /// Interaccion con la lader, si estaba subiendo la escalera deja de subir(si puede) o sino comienza a subirla
    /// </summary>
    public void InteractWithLadder()
    {
        if (isClimbingLadder)
        {
            FinishLadderClimbing();
        }
        else
        {
            ChangeHandStatus(true);
            gameObject.layer = characterClimbingLayer;
            basePlayerMovement.IsWalkEnabled = false;
            rb.gravityScale = 0;
            isClimbingLadder = true;
            basePlayerMovement.SetFacingDirection(leftDirection);
        }
    }

    /// <summary>
    /// Se baja de la escalera el ciego
    /// </summary>
    public void FinishLadderClimbing()
    {
        gameObject.layer = characterLayer;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        basePlayerMovement.IsWalkEnabled = true;
        ChangeHandStatus(false);
        rb.gravityScale = 1;
        isClimbingLadder = false;
        OnVerticalMovementInput(0);
    }

    private void ChangeHandStatus(bool state)
    {
        foreach (var item in handLights)
        {
            item.enabled = state;
        }
    }

    private void OnVerticalMovementInput(float dir)
    {
        if (isClimbingLadder)
        {
            if (dir < 0)
            {
                anim.SetBool(LADDER_CLIMB_ANIMATOR_PARAMETER, true);
                dir = -1;
            }
            else if (dir > 0)
            {
                anim.SetBool(LADDER_CLIMB_ANIMATOR_PARAMETER, true);
                dir = 1;
            }
        }
        else
        {
            anim.SetBool(LADDER_CLIMB_ANIMATOR_PARAMETER, false);
        }
        desiredDir = dir;
    }
}
