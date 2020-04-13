using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float GetLastXDir { get { return lastXDir; } }

    public bool OnGround { get { return isGrounded; } }

    public bool StillDashing { get { return stillDashing; } }

    [Header("References")]
    [SerializeField] private ParticleSystem dust = null;
    [SerializeField] private ParticleSystem afterImage = null;
    [SerializeField] private LayerMask wallLayer = 0;

    [Header("Movement")]
    [SerializeField] private float maxSpeed = 6f; // The max velocity
    [SerializeField] private float maxAccel = 20f; // The speed that reach its max velocity

    [Header("Air Control")]
    [SerializeField] private float maxAirSpeed = 4f; // The max velocity
    [SerializeField] private float maxAirAccel = 20f; // The speed that reach its max velocity

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCDDuration = 1f;
    [SerializeField] private float cappedSpeed = 20f;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForce = 2.5f;
    [SerializeField] private float distanceToWall = 0.5f;
    [SerializeField] private float controlCDDuration = 0.1f;

    //Inputs
    private Joystick input = null;
    private HoldButton jumpButton = null;
    private HoldButton dashButton = null;

    //Component References
    private Rigidbody2D rb = null;
    private new Collider2D collider = null;
    private PlayerColor playerColor = null;
    private SpriteRenderer sr = null;
    private Animator ani = null;

    //Movement
    private float xInput = 0f;
    private float lastXDir = 0;
    private int facingDirection = 0;
    private float speedModifier = 0f;

    //Jumping
    private bool isGrounded = false;
    private bool isWallRiding = false;
    private float controlCD = 0f;

    //Dashing
    private bool isDashing = false;
    private float dashCD = 0f;
    private Vector2 dashDirection = Vector2.zero;
    private bool stillDashing = false;
    private float dashDuration = 0.7f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        playerColor = GetComponent<PlayerColor>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        input = ObjectReferences.instance.movementInput;
        jumpButton = ObjectReferences.instance.jumpButton;
        dashButton = ObjectReferences.instance.dashButton;
    }

    private void Update()
    {
        // COOLDOWN CODE
        dashCD -= Time.deltaTime;
        controlCD -= Time.deltaTime;

        //For Dash To kill and break objects
        if(stillDashing)
        {
            dashDuration -= Time.deltaTime;
            if(dashDuration <= 0f)
            {
                stillDashing = false;
                dashDuration = 0.7f;
            }
            if(rb.velocity.magnitude > cappedSpeed)
            {
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, cappedSpeed);
            }
        }

        // ANIMATION CODE
        ani.SetFloat("yVel", rb.velocity.y);
        ani.SetFloat("xVel", rb.velocity.x);
        ani.SetBool("IsGrounded", isGrounded);
        ani.SetBool("IsWall", isWallRiding);

        // INPUT
        xInput = input.Horizontal;
        // FORCE xInput to normalize at 1 or -1
        if (xInput > 0.25f) xInput = 1f;
        else if (xInput < -0.25f) xInput = -1;
        else ani.SetBool("IsRunning", false);
        if (xInput != 0) lastXDir = xInput;

        // BUTTON INPUT
        if (jumpButton.tap && isGrounded)
            Jump();
        else if (jumpButton.tap && isWallRiding) // WALL JUMP
            WallJump();

        // DASH
        if ((dashButton.tap || Input.GetKeyDown(KeyCode.E))&& playerColor.GetCurrentColor.GetMain != COLORS.BLUE && dashCD < 0f)
            isDashing = true;

        // MISC
        if (isGrounded)
        {
            isWallRiding = false;
            ani.SetBool("IsWall", isWallRiding);
        }

        // DEBUG CODE
        Debug.DrawRay(transform.position, new Vector2(facingDirection, 0) * (collider.bounds.extents.x + distanceToWall), Color.red);
        Debug.DrawRay(transform.position - new Vector3(0, collider.bounds.extents.y, 0), new Vector2(facingDirection, 0) * (collider.bounds.extents.x + distanceToWall), Color.red);
    }

    private void FixedUpdate()
    {
        bool temp = false; // THIS HAVE PROBLEMS MIGHT NEED FIXING SOMEDAY
        Vector3 xExtent = new Vector3(collider.bounds.extents.x, 0f, 0f);
        if (Physics2D.Raycast(collider.bounds.center + xExtent, Vector2.down, collider.bounds.extents.y + 0.1f) ||
           Physics2D.Raycast(collider.bounds.center - xExtent, Vector2.down, collider.bounds.extents.y + 0.1f))
            temp = true;

        // VISUAL EFFECTS (DUST)
        if (!isGrounded && temp)
            CreateDust();
        isGrounded = temp;

        if (isGrounded)
            Move();
        else
            AirMove();

        if (isDashing)
            Dash();

        if (!isGrounded)
            isWallRiding = CastRayInDirection(facingDirection);

    }

    private static bool right = true;
    private void Move()
    {
        // UPDATE DIRECTION
        if (xInput > 0) facingDirection = 1;
        else if (xInput < 0) facingDirection = -1;
        // UPDATE CHARACTER FLIP
        if (xInput > 0 && !right)
            Flip(true,false);
        else if (xInput < 0 && right)
            Flip(true,false);
        // UPDATE LAST KNOWN DIRECTION
        if (xInput > 0)
            right = true;
        else if (xInput < 0)
            right = false;

        Vector2 targetVel = rb.velocity;
        float maxSpeedChange = maxAccel * Time.deltaTime;
        targetVel.x = Mathf.MoveTowards(targetVel.x, xInput * maxSpeed, maxSpeedChange);
        Debug.Log("input * speed: " + xInput * maxSpeed);
        Debug.Log("targetVel.x: " + targetVel.x);

        if (controlCD < 0) // Stop all update to rb is controlCD is up
        {
            ani.SetBool("IsRunning", true);
            rb.velocity = targetVel;
        }
    }

    private void AirMove()
    {
        // DON'T UPDATE DIRECTION WHEN WALLRIDING
        if(!isWallRiding)
        {
            // UPDATE DIRECTION
            if (xInput > 0) facingDirection = 1;
            else if (xInput < 0) facingDirection = -1;
            // UPDATE CHARACTER FLIP
            if (xInput > 0 && !right)
                Flip(true, false);
            else if (xInput < 0 && right)
                Flip(true, false);
            // UPDATE LAST KNOWN DIRECTION
            if (xInput > 0)
                right = true;
            else if (xInput < 0)
                right = false;
        }
        else
        {
            if (facingDirection > 0)
                right = false;
            else if (facingDirection < 0)
                right = true;
            Flip();
        }

        Vector2 targetVel = rb.velocity;
        float maxSpeedChange = maxAirAccel * Time.deltaTime;
        targetVel.x = Mathf.MoveTowards(targetVel.x, xInput * maxAirSpeed, maxSpeedChange);

        if(controlCD < 0) // Stop all update to rb is controlCD is up
            rb.velocity = targetVel;

    }

    private void Jump()
    {
        CreateDust();
        rb.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
        ani.SetTrigger("Jump");
    }

    private void WallJump()
    {
        rb.AddForce(new Vector2(-facingDirection * wallJumpForce, wallJumpForce * 1.25f), ForceMode2D.Impulse);
        controlCD = controlCDDuration;
        ani.SetTrigger("WallJump");
    }

    private void Dash()
    {
        isDashing = false;
        stillDashing = true;
        dashCD = dashCDDuration;
        controlCD = controlCDDuration;
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

        ani.SetTrigger("Dash");
        StartCoroutine(PerformDash(controlCD, dashDirection));
    }

    IEnumerator PerformDash(float time, Vector2 dir)
    {
        yield return new WaitForSeconds(time);
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * dashSpeed, ForceMode2D.Impulse);
    }

    private bool CastRayInDirection(int direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center, new Vector2(direction, 0), collider.bounds.extents.x + distanceToWall, wallLayer);
        RaycastHit2D lowHit = Physics2D.Raycast(collider.bounds.center - new Vector3(0, collider.bounds.extents.y, 0), new Vector2(direction, 0), collider.bounds.extents.x + distanceToWall, wallLayer);
        if (hit.collider != null || lowHit.collider != null)
            return true;

        return false;
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void CreateAfterImage()
    {
        // afterImage.gameObject.GetComponent<ParticleSystemRenderer>().sharedMaterial.SetTexture("_MainTex", sr.sprite.texture);
    }

    private void Flip(bool playDust,bool forceFlip)
    {
        if (playDust)
            CreateDust();

        if (forceFlip)
            sr.flipX = !sr.flipX;
        else
        {
            if (facingDirection > 0)
                sr.flipX = false;
            else if (facingDirection < 0)
                sr.flipX = true;
        }
    }

    private void Flip()
    {
        if (right)
            sr.flipX = false;
        else
            sr.flipX = true;
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

    public void IncreaseSpeed(float modifier)
    {
        speedModifier = modifier;

        maxAccel *= modifier;
        maxSpeed *= modifier;
        maxAirAccel *= modifier;
        maxAirSpeed *= modifier;
        dashSpeed *= modifier;
    }

    public void NormalSpeed()
    {
        maxAccel /= speedModifier;
        maxSpeed /= speedModifier;
        maxAirAccel /= speedModifier;
        maxAirSpeed /= speedModifier;
        dashSpeed /= speedModifier;

        speedModifier = 1f;
    }
}