using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInput))]
public class ClimbPlayerMovement : MonoBehaviour
{
    private PlayerInput input;
    private Animator anim;

    #region SerializedVariables
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform rayCastParent;
    [SerializeField]
    LayerMask whatIsFloor = 0;
    [SerializeField]
    private float wallCheckDistance;
    [SerializeField]
    private float ledgeClimbXOffset1=0;
    [SerializeField]
    private float ledgeClimbXOffset2=0;
    [SerializeField]
    private float ledgeClimbYOffset1=0;
    [SerializeField]
    private float ledgeClimbYOffset2=0;

    #endregion

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool canClimbLedge;
    private bool ledgeDetected=false;
    private bool facingRight;

    

    #region MonoBehaviour callbacks
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        input.OnJump += OnClimb;
    }
    private void OnDestroy()
    {
        input.OnJump -= OnClimb;
    }
    #endregion
    private void FixedUpdate()
    {
        CheckSorroundings();  
    }

  
    private void CheckSorroundings()
    {
       isTouchingWall = Physics2D.Raycast(wallCheck.position , transform.right, wallCheckDistance, whatIsFloor);
       isTouchingLedge= Physics2D.Raycast(ledgeCheck.position, transform.right, wallCheckDistance, whatIsFloor);


        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
           
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    private void OnClimb(bool pressed)
    {
        if (pressed)
        {
            print("Space");
            if (ledgeDetected && !canClimbLedge)
            {
                canClimbLedge = true;

                if (transform.localScale.x>0)
                {
                    ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
                }
                else
                {
                    ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                    ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
                }
                anim.SetBool("isClimbingLedge", canClimbLedge);

                //--------------%%%%===        CAMBIAR POR UNA ANIMACIÓN CUANDO TENGAMOS LOS FRAMES DEL CIEGO TREPANDO!!!!1!     ===%%%%%-----------------------------
                StartCoroutine(WaitingForClimb());


            }

            if (canClimbLedge)
            {
                transform.position = ledgePos1;
            }
        }
        

    }

    /// <summary>
    /// Called once the climbing animation is finished
    /// </summary>
    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        ledgeDetected = false;

        anim.SetBool("isClimbingLedge", canClimbLedge);
    }

    IEnumerator WaitingForClimb()
    {
        input.enabled = false;
        yield return new WaitForSeconds(1);
        FinishLedgeClimb();
        input.enabled = true;
    }

}
