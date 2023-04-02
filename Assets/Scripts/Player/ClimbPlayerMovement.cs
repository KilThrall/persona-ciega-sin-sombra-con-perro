using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInput))]
public class ClimbPlayerMovement : MonoBehaviour
{
    private PlayerInput input;
    private Animator anim;
    private Rigidbody2D rb;

    #region SerializedVariables
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform rayCastParent;
    [SerializeField]
    private LayerMask whatIsFloor = 0;
    [SerializeField]
    private float wallCheckDistance;
  
    [Header("Offets")]
    [SerializeField][Tooltip("Desplazamiento en X del jugador antes de teletransportarse a la posicion final de trepado")]
    private float ledgeClimbXOffset1=0;
    [SerializeField][Tooltip("Desplazamiento en Y del jugador antes de teletransportarse a la posicion final de trepado")]
    private float ledgeClimbYOffset1=0;
    [SerializeField][Tooltip("Desplazamiento en X de la posicion final de trepado")]
    private float ledgeClimbXOffset2=0;
    [SerializeField][Tooltip("Desplazamiento en Y de la posicion final de trepado")]
    private float ledgeClimbYOffset2=0;

    #endregion

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    private bool isTouchingWall;
    private bool isTouchingLedge;
    private bool isClimbing;
    private bool ledgeDetected=false;
    private bool facingRight;

    

    #region MonoBehaviour callbacks
    private void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        input.OnJump += OnClimb;
    }
    private void OnDestroy()
    {
        input.OnJump -= OnClimb;
    }
    private void FixedUpdate()
    {
        CheckSurroundings();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawRay(wallCheck.position, transform.right*wallCheckDistance);
        Gizmos.DrawRay(ledgeCheck.position,ledgeCheck.right*wallCheckDistance);

        Gizmos.DrawWireSphere(transform.position+new Vector3(ledgeClimbXOffset1, ledgeClimbYOffset1),0.1f);
        Gizmos.DrawWireSphere(transform.position+new Vector3(ledgeClimbXOffset2, ledgeClimbYOffset2),0.1f);
    }
    #endregion


    private void CheckSurroundings()
    {
       isTouchingWall = Physics2D.Raycast(wallCheck.position , new Vector2(transform.localScale.x,0), wallCheckDistance, whatIsFloor);
       isTouchingLedge= Physics2D.Raycast(ledgeCheck.position, new Vector2(transform.localScale.x, 0), wallCheckDistance, whatIsFloor);

        print(transform.right);

        if (isTouchingWall)
        {
            if (!isTouchingLedge && !ledgeDetected)
            {
                ledgeDetected = true;
                ledgePosBot = wallCheck.position;
            }
            else
            {
                ledgeDetected = false;
            }
        }
    }

    private void OnClimb(bool pressed)
    {
        if (pressed)
        {
            print("Space");
            if (ledgeDetected && !isClimbing)
            {
                isClimbing = true;

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
                anim.SetBool("isClimbingLedge", isClimbing);

                //TODO: CAMBIAR POR UNA ANIMACIÓN CUANDO TENGAMOS LOS FRAMES DEL CIEGO TREPANDO!!!!1!
                StartCoroutine(WaitingForClimb());
                 rb.velocity = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// Called once the climbing animation is finished
    /// </summary>
    public void FinishLedgeClimb()
    {
        isClimbing = false;
        transform.position = ledgePos2;
        ledgeDetected = false;
        anim.SetBool("isClimbingLedge", isClimbing);
    }

    IEnumerator WaitingForClimb()
    {
        input.enabled= false;
        yield return new WaitForSeconds(1);
        FinishLedgeClimb();
        input.enabled = true;
    }
}
