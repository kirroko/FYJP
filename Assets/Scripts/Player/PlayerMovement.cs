using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick GetInput { get { return input; } }

    public HoldButton GetJumpButton { get { return jumpButton; } }

    public HoldButton GetDashButton { get { return dashButton; } }

    public float GetLastXDir { get { return lastXDir; } }

    public bool OnGround { get { return isGrounded; } }

    [Header("References")]
    [SerializeField] private Joystick input = null;
    [SerializeField] private HoldButton jumpButton = null;
    [SerializeField] private HoldButton dashButton = null;
    [SerializeField] private LayerMask wallLayer = 0;
    [SerializeField] private LayerMask groundLayer = 0;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dampForce = 0.05f;

    [Header("Air Control")]
    [SerializeField] private float airSpeed = 2f;
    [SerializeField] private float xVelocityMax = 15f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float checkRadius = 0.5f;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashCDDuration = 1f;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForce = 2.5f;
    [SerializeField] private float distanceToWall = 0.5f;
    [SerializeField] private float controlCDDuration = 0.1f;

    //Component References
    private Rigidbody2D rb = null;
    private new Collider2D collider = null;
    private PlayerColor playerColor = null;
    private SpriteRenderer sr = null;

    //Movement
    private float xInput = 0f;
    private float lastXDir = 0;
    private int facingDirection = 0;

    //Jumping
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isWallRiding = false;
    private float controlCD = 0f;

    //Dashing
    private bool isDashing = false;
    private float dashCD = 0f;
    private Vector2 dashDirection = Vector2.zero;
    private Vector2 startPos = Vector2.zero;
    private Vector2 targetPos = Vector2.zero;

    // Interpolate
    private float lerpTime = 1f; // change to dashCDDuration
    private float currentLerpTime;

    [Header("TBR")]
    public TextMeshProUGUI cooldown = null;

    private Vector2 test;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        playerColor = GetComponent<PlayerColor>();
        sr = GetComponent<SpriteRenderer>();
        cooldown.text = "0";
    }

    private void Update()
    {
        // COOLDOWN CODE
        dashCD -= Time.deltaTime;
        controlCD -= Time.deltaTime;

        // INPUT
        xInput = input.Horizontal;
        if (xInput != 0) lastXDir = xInput;
        if (xInput > 0.25f) // force xInput to normalize at 1 or -1
        {
            xInput = 1f;
            sr.flipX = false;
        }
        else if (xInput < -0.25f)
        {
            xInput = -1f;
            sr.flipX = true;
        }

        // BUTTON INPUT
        if (jumpButton.tap && isGrounded)
            Jump();
        else if (jumpButton.tap && isWallRiding) // WALL JUMP
            WallJump();

        // DASH
        if (dashButton.tap && playerColor.GetCurrentColor.GetMain != COLORS.BLUE && dashCD < 0f)
        {
            Debug.Log("TAP DASH");
            
            isDashing = true;
            dashCD = dashCDDuration;

            dashDirection = Vector2.zero;

            if (input.Direction.x > 0.5f)
                dashDirection.x = 1f;
            else if (input.Direction.x < -0.5f)
                dashDirection.x = -1f;

            if (input.Direction.y > 0.5f)
                dashDirection.y = 1f;
            else if (input.Direction.y < -0.5f)
                dashDirection.y = -1f;

            if (dashDirection == Vector2.zero)
                dashDirection.x = facingDirection;

            rb.velocity = Vector2.zero;
            rb.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);

            currentLerpTime = 0f;
        }

        // DEBUG CODE
        Debug.DrawRay(transform.position, new Vector2(facingDirection, 0) * (collider.bounds.extents.x + distanceToWall), Color.red);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + 0.1f); // THIS HAVE PROBLEMS MIGHT NEED FIXING SOMEDAY

        if (isGrounded)
            Move();
        else
            AirMove();

        if (!isGrounded)
            isWallRiding = CastRayInDirection(facingDirection);
    }

    private void Move()
    {
        if (controlCD > 0) return;

        Vector2 targetVel = rb.velocity;

        targetVel.x += xInput * moveSpeed;
        targetVel.x *= 1f - dampForce;

        if (controlCD < 0) // Stop all controls if controlCD is up
            rb.velocity = targetVel;

        // Direction
        if (xInput > 0)
            facingDirection = 1;
        else if (xInput < 0)
            facingDirection = -1;
    }

    private void AirMove()
    {
        if (controlCD > 0) return;

        Vector2 targetVel = rb.velocity;

        targetVel.x += xInput * airSpeed; // with no damping
        targetVel.x = Mathf.Clamp(targetVel.x, -xVelocityMax, xVelocityMax); // now clamp velocity

        rb.velocity = targetVel;
    }

    private void Jump()
    {
        rb.velocity += new Vector2(rb.velocity.x, jumpForce);
    }

    private void WallJump()
    {
        rb.AddForce(new Vector2(-facingDirection * wallJumpForce, wallJumpForce), ForceMode2D.Impulse);
        controlCD = controlCDDuration;
    }

    private bool CastRayInDirection(int direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, new Vector2(direction, 0), collider.bounds.extents.x + distanceToWall, wallLayer);

        if (hit.collider != null)
            return true;

        return false;
    }

    public void ResetDash()
    {
        isDashing = false;
        dashCD = 0f;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}