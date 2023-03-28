using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(JumpPlayerMovement))]
public class WallJumpPlayerMovement : MonoBehaviour
{
    #region Serialized variables

    [Header("Jump modifiers")]
    [SerializeField]
    private float initialJumpImpulse = 100;
    [Tooltip("Used when the player does a wall jump without holding a directional key")]
    [SerializeField]
    private float initialNeutralHorizontalImpulse = 100;
    [Tooltip("Used when the player does a wall jump while holding a directional key")]
    [SerializeField]
    private float initialDirectionalHorizontalImpulse = 200;
    [SerializeField]
    private float directionalJumpForce;
    [SerializeField]
    private float jumpForce = 10;
    [Tooltip("The constant force multiplier applied when the player does a directional wall jump")]
    [SerializeField]
    private float directionalWallJumpMultiplier = 2.5f;
    [SerializeField]
    private float maxJumpVectorSize = 150;
    [SerializeField]
    private float maxConstantJumpVectorSize = 20;
    [SerializeField]
    private float maxJumpDuration = 0.3f;

    [Header("Wall detection")]
    [SerializeField]
    private float coyoteTime = 0.25f;
    [SerializeField]
    private float yDetectionOffset = 0.3f;
    [SerializeField]
    private float wallDetectionDistance = 0.3f;
    [SerializeField]
    private LayerMask whatIsWall = 0;

    #endregion

    private PlayerInput input;
    private Rigidbody2D rb;
    private Animator anim;
    private JumpPlayerMovement jumpController;
    private BasePlayerMovement movementController;

    private bool isHoldingJump = false;
    private bool canJump = false;
    private bool isNormalJumpEnabled = true;
    private bool grabbingWall = false;
    private bool wasGrabbingWallLastFrame = false;
    
    private int wallDir;

    private float timeHeld = 0;
    private float timeSinceLeftWall;
    private float multiplier =1;

    #region MonoBehaviour callbacks
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        jumpController = GetComponent<JumpPlayerMovement>();
        movementController = GetComponent<BasePlayerMovement>();
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
            var dir = Vector2.up * jumpForce + Vector2.right * directionalJumpForce * multiplier * -wallDir;
            dir = dir.normalized * maxConstantJumpVectorSize;
            rb.AddForce(dir);
        }
        wasGrabbingWallLastFrame = grabbingWall;
        UpdateWallDetection(1);
        UpdateWallDetection(-1);
        anim.SetBool("Sliding", grabbingWall);

        if (wasGrabbingWallLastFrame != grabbingWall)
        {
            jumpController.ExternalJumpBlock(!grabbingWall);
        }

        if (!grabbingWall)
        {
            timeSinceLeftWall += Time.deltaTime;
            canJump = timeSinceLeftWall <= coyoteTime;
        }
        else
        {
            canJump = true;
            timeSinceLeftWall = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        var startingPoint = transform.position + Vector3.up * yDetectionOffset;
        Gizmos.DrawLine(startingPoint, startingPoint + Vector3.right*wallDetectionDistance);
        Gizmos.DrawLine(startingPoint, startingPoint + Vector3.right * -wallDetectionDistance);
        var startingPoint2 = transform.position + Vector3.up * -yDetectionOffset;
        Gizmos.DrawLine(startingPoint2, startingPoint2 + Vector3.right * wallDetectionDistance);
        Gizmos.DrawLine(startingPoint2, startingPoint2 + Vector3.right * -wallDetectionDistance);
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
        var dir = Vector2.up * initialJumpImpulse + Vector2.right * GetXForce();
        dir = dir.normalized * maxJumpVectorSize;
        rb.AddForce(dir);
        canJump = false;
        anim.SetTrigger("Jump");
    }

    private float GetXForce()
    {
        multiplier = 1;
        var force = initialNeutralHorizontalImpulse;
        if (movementController.GetDesiredDir() != 0)
        {
            force = initialDirectionalHorizontalImpulse;
            multiplier = directionalWallJumpMultiplier;
        }
        force *= -wallDir;
        return force;
    }

    private void UpdateWallDetection(float yMultiplier)
    {
        var startingPoint = transform.position + Vector3.up * yDetectionOffset *yMultiplier;

        bool leftSideFloor = Physics2D.Raycast(startingPoint, Vector2.left, wallDetectionDistance, whatIsWall);

        if (leftSideFloor)
        {
            wallDir = -1;
            grabbingWall = true;
            return;
        }

        bool rightSideFloor = Physics2D.Raycast(startingPoint, Vector2.right, wallDetectionDistance, whatIsWall);

        grabbingWall = rightSideFloor;
        wallDir = 1;
    }
}
