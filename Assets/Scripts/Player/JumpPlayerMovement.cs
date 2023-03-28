using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IInput))]
public class JumpPlayerMovement : MonoBehaviour
{
    #region Serialized variables

    [Header("Jump modifiers")]
    [SerializeField]
    private float initialJumpImpulse = 10;
    [SerializeField]
    private float jumpForce = 5;
    [SerializeField]
    private float maxJumpDuration = 0.3f;

    [Header("Floor detection")]
    [SerializeField]
    private float coyoteTime = 0.25f;
    [SerializeField]
    private float xDetectionOffset = 0.3f;
    [SerializeField]
    private float floorDetectionDistance = 0.3f;
    [SerializeField]
    private LayerMask whatIsFloor = 0;

    #endregion

    private PlayerInput input;
    private Rigidbody2D rb;
    private Animator anim;

    private bool isHoldingJump = false;
    private bool canJump = false;
    private bool isNormalJumpEnabled = true;
    private bool onFloor = false;
    private float timeHeld = 0;
    private float timeSinceLeftFloor;

    #region MonoBehaviour callbacks
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        input.OnJump += OnJump;
    }
    private void OnDestroy()
    {
        input.OnJump -= OnJump;
    }

    private void FixedUpdate()
    {
        if (!isNormalJumpEnabled)
        {
            return;
        }
        if (isHoldingJump && timeHeld < maxJumpDuration)
        {
            timeHeld += Time.deltaTime;
            rb.AddForce(Vector2.up * jumpForce);
        }

        UpdateFloorDetection();
        anim.SetBool("OnFloor", onFloor);
        if (!onFloor)
        {
            timeSinceLeftFloor += Time.deltaTime;
            canJump = timeSinceLeftFloor <= coyoteTime;
        }
        else
        {
            canJump = true;
            timeSinceLeftFloor = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var startingPoint1 = transform.position + Vector3.right * xDetectionOffset;
        Gizmos.DrawLine(startingPoint1, startingPoint1 + new Vector3(0, -floorDetectionDistance));
        var startingPoint2 = transform.position - Vector3.right * xDetectionOffset;
        Gizmos.DrawLine(startingPoint2, startingPoint2 + new Vector3(0, -floorDetectionDistance));
    }
    #endregion

    private void OnJump(bool pressed)
    {
        if (!isNormalJumpEnabled)
        {
            return;
        }
        isHoldingJump = pressed;
        if (!pressed || !canJump)
        {
            isHoldingJump = false;
            return;
        }

        timeHeld = 0;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * initialJumpImpulse);
        canJump = false;
        anim.SetTrigger("Jump");
    }

    private void UpdateFloorDetection()
    {
        var startingPoint1 = transform.position + Vector3.right*xDetectionOffset;

        bool leftSideFloor = Physics2D.Raycast(startingPoint1,Vector2.down, floorDetectionDistance, whatIsFloor);

        if (leftSideFloor)
        {
            onFloor = true;
            return;
        }

        var startingPoint2 = transform.position - Vector3.right * xDetectionOffset;

        bool rightSideFloor = Physics2D.Raycast(startingPoint2, Vector2.down, floorDetectionDistance, whatIsFloor);

        onFloor = rightSideFloor;
    }

    public void ExternalJumpBlock(bool state)
    {
        isNormalJumpEnabled = state;
        isHoldingJump = false;
    }
}
