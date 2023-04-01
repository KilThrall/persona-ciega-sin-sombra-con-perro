using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        input.OnMovementInput += OnMovementInput;
        input.OnJump += OnClimb;
    }
    private void OnDestroy()
    {
        input.OnMovementInput -= OnMovementInput;
        input.OnJump -= OnClimb;
    }
    #endregion
    private void FixedUpdate()
    {
     
        CheckSorroundings();  
    }

    private void OnMovementInput(float dir)
    {
        if (dir > 0)
        {
            facingRight = true;
        }
        if(dir<0)
        {
            facingRight = false;
        }
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


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;


        Gizmos.DrawLine(wallCheck.position, transform.right);
        Gizmos.DrawLine(ledgeCheck.position, transform.right);

    }

    private void OnClimb(bool pressed)
    {
        if (pressed)
        {
            print("Space");
            if (ledgeDetected && !canClimbLedge)
            {
                canClimbLedge = true;

                if (facingRight)
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
    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        ledgeDetected = false;

        anim.SetBool("isClimbingLedge", canClimbLedge);
    }

    IEnumerator WaitingForClimb()
    {
        yield return new WaitForSeconds(1);
        FinishLedgeClimb();
    }

}
