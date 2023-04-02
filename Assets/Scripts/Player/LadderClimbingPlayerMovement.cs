using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInput))]
public class LadderClimbingPlayerMovement : MonoBehaviour
{
    #region Serialized variables

    [SerializeField]
    private float climbSpeed = 5;
    [SerializeField]
    private float acceleration = 3;
    [SerializeField]
    private ContactBasedLight[] handLights;
    //TODO: Cuando trepe que hagan sonido sus manos tambien, permitiendo ver un poco para arriba
    #endregion

    private IInput input;
    private Rigidbody2D rb;
    private Animator anim;
    private float desiredDir;

    private bool isClimbingLadder;


    #region MonoBehaviour callbacks
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<IInput>();
        anim = GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        input.OnVerticalMovementInput -= OnVerticalMovementInput;
    }
    #endregion
    public void InteractWithLadder()
    {
        if (isClimbingLadder)
        {
            FinishLadderClimbing();
        }
        else
        {
            ChangeHandStatus(true);
            rb.gravityScale = 0;
            isClimbingLadder = true;
            input.OnVerticalMovementInput += OnVerticalMovementInput;
        }
    }

    public void FinishLadderClimbing()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        ChangeHandStatus(false);
        rb.gravityScale = 1;
        isClimbingLadder = false;
        input.OnVerticalMovementInput -= OnVerticalMovementInput;
    }


    private void FixedUpdate()
    {
        if (isClimbingLadder)
        {
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


    private void ChangeHandStatus(bool state)
    {
        foreach (var item in handLights)
        {
            item.enabled = state;
        }
    }

    private void OnVerticalMovementInput(float dir)
    {
        if (dir < 0)
        {
            anim.SetBool("LadderClimbing", true);
            dir = -1;
        }
        else if (dir > 0)
        {
            anim.SetBool("LadderClimbing", true);
            dir = 1;
        }
        else
        {
            anim.SetBool("LadderClimbing", false);
        }
        desiredDir = dir;
    }


}
