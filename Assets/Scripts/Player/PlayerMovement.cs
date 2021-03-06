﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

/**
 * This class handles the player's movement that includes, Jumping, Dashing, Wall jumping.
 * 
 * Anything related to the player's movement can be written here.
 */
public class PlayerMovement : MonoBehaviour
{
    public float GetLastXDir { get { return lastXDir; } }

    public bool OnGround { get { return isGrounded; } }

    public bool StillDashing { get { return stillDashing; } }

    public bool InforceFlip { set { inforceFlip = value; } get { return inforceFlip; } }

    public int FacingDirection { get { return facingDirection; } }

    public float DefaultGravity { get { return defaultGravity; } }

    [Header("References")]
    [SerializeField] private ParticleSystem dust = null; /// < Set for visual effects.
    [SerializeField] private LayerMask wallLayer = 0;
    [SerializeField] private LayerMask floorLayer = 0;

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
    [SerializeField] private float DashFalloffDuration = 0.3f;
    [SerializeField] private float distanceBetweenImages = 0.3f;

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
    private float speedModifier = 1f;
    private bool inforceFlip = false;

    //Jumping
    private bool isGrounded = false;
    private bool isWallRiding = false;
    private float controlCD = 0f; /// < When this value is positive, all control will be stopped. Needed if you want to stop the player from controling uncessary

    //Dashing
    private bool isDashing = false;
    private float dashCD = 0f;
    private Vector2 dashDirection = Vector2.zero;
    private bool stillDashing = false;
    private float dashDuration = 0.7f;
    private float defaultGravity = 0f;
    private float lastImageXPos;
    private float DashFalloff = 0f;

    //misc
    private bool forceGravity = false; /// < Set this to true if you wish to manually set the player's gravity scale

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

        defaultGravity = rb.gravityScale;
    }

    /**
     * A Update function.
     * 
     * Inside this function handles; 
     * All "cooldown" related codes
     * Player's gravity scale
     * Player's animation value
     * Input handling
     * Button handling
     * Wall, Wall jumping and dash
     */
    private void Update()
    {
        // COOLDOWN CODE
        dashCD -= Time.deltaTime;
        DashFalloff -= Time.deltaTime;
        controlCD -= Time.deltaTime;

        if (DashFalloff < 0 && !forceGravity) // reset gravity
            rb.gravityScale = defaultGravity;

        //For Dash To kill and break objects
        if(stillDashing)
        {
            dashDuration -= Time.deltaTime;
            if(dashDuration <= 0f)
            {
                stillDashing = false;
                dashDuration = 0.7f;
            }

            if(Mathf.Abs(transform.position.x - lastImageXPos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXPos = transform.position.x;
            }
        }

        // ANIMATION CODE
        ani.SetFloat("yVel", rb.velocity.y);
        ani.SetFloat("xVel", rb.velocity.x);
        ani.SetBool("IsGrounded", isGrounded);
        ani.SetBool("IsWall", isWallRiding);

        // INPUT
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKey(KeyCode.A))
                xInput = -1f;
            else if (Input.GetKey(KeyCode.D))
                xInput = 1f;
            else
                xInput = 0f;
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            xInput = input.Horizontal;
        }

        // FORCE xInput to normalize at 1 or -1
        if (xInput > 0.25f) xInput = 1f;
        else if (xInput < -0.25f) xInput = -1;
        else ani.SetBool("IsRunning", false);
        if (xInput != 0) lastXDir = xInput;

        // BUTTON INPUT
        if ((jumpButton.tap || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
            Jump();
        else if ((jumpButton.tap || Input.GetKeyDown(KeyCode.Space)) && isWallRiding) // WALL JUMP
            WallJump();

        // DASH
        if ((dashButton.tap || Input.GetKeyDown(KeyCode.LeftShift)) && playerColor.GetCurrentColor.GetMain != COLORS.BLUE && dashCD < 0f)
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

    /** A FixedUpdate function
     * 
     * Inside this functions includes;
     * All Physic related code
     * Ground Checker
     * Player's movement on ground 
     * Player's movement in air
     * Dash
     */
    private void FixedUpdate()
    {
        bool temp = false;
        Vector3 xExtent = new Vector3(collider.bounds.extents.x, 0f, 0f);
        RaycastHit2D hit = Physics2D.Raycast(collider.bounds.center + xExtent, Vector2.down, collider.bounds.extents.y + 0.1f,floorLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(collider.bounds.center - xExtent, Vector2.down, collider.bounds.extents.y + 0.1f,floorLayer);
        if ((hit || hit2))
            temp = true;

        // VISUAL EFFECTS (DUST)
        if (!isGrounded && temp)
        {
            AudioManager.PlaySFX("Land", false);
            CreateDust();
        }
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

    /**
     * This Function handles player's movement on ground
     * 
     * It orientate the player facing direction
     * 
     * It will stop all movement update if controlCD is up
     */
    private static bool right = true; // Helper variable
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

        targetVel.x = Mathf.Clamp(targetVel.x, -dashSpeed, dashSpeed);

        if (controlCD < 0) // Stop all update to rb is controlCD is up
        {
            ani.SetBool("IsRunning", true);
            rb.velocity = targetVel;
        }
    }

    /**
     * This function handles player's movement in air
     * 
     * It will not update sprite direction when wall riding is true
     * 
     * It will stop all movement update if controlCD is up
     */
    private void AirMove()
    {
        // DON'T UPDATE DIRECTION WHEN WALLRIDING
        if(!isWallRiding && controlCD < 0)
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

        targetVel.x = Mathf.Clamp(targetVel.x, -dashSpeed, dashSpeed);

        if (controlCD < 0) // Stop all update to rb is controlCD is up
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
        rb.AddForce(new Vector2(-facingDirection * wallJumpForce, wallJumpForce * 2.0f), ForceMode2D.Impulse);
        controlCD = controlCDDuration;
        ani.SetTrigger("WallJump");
    }

    private void Dash()
    {
        isDashing = false;
        stillDashing = true;
        dashCD = dashCDDuration;
        DashFalloff = DashFalloffDuration;
        // controlCD = controlCDDuration;
        dashDirection = input.Direction;

        dashButton.GetComponentInChildren<CooldownIndicator>().StartCooldown(dashCD);

        if (dashDirection == Vector2.zero)
            dashDirection.x = facingDirection;
        else if (dashDirection.y <= 0f && dashDirection.y > -0.3f)
            dashDirection.y = 0f;

        if (dashDirection.x == 0 && dashDirection.y > 0) // JOYSTICK FACING UPWARDS
            ani.SetTrigger("UpDash");
        else if (dashDirection.x == 0 && dashDirection.y < 0) // JOYSTICK FACING DOWNWARDS
            ani.SetTrigger("DownDash");
        else if (dashDirection.x != 0 && dashDirection.y > 0) // JOYSTICK FACING UPWARDS EITHER LEFT OR RIGHT
            ani.SetTrigger("DiagonalDash");
        else if (dashDirection.x != 0 && dashDirection.y < 0) // JOYSTICK FACING DOWNWARDS EITHER LEFT OF RIGHT
            ani.SetTrigger("BRDiagonalDash");
        else
            ani.SetTrigger("Dash");

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;

        AudioManager.PlaySFX("Dash", false);

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        Vector3 force = dashDirection * dashSpeed;
        force = Vector3.ClampMagnitude(force, dashSpeed);
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    /**
     * This function is a helper function for wallJumping
     * It'll cast two rays depending on the gameobject collider bounds; One center the other at the bottom.
     * 
     */
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

    private void Flip(bool playDust,bool forceFlip)
    {
        if (playDust)
            CreateDust();

        if (forceFlip)
            sr.flipX = !sr.flipX;
        else if(!inforceFlip)
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

    public void ForceGravityToZero()
    {
        rb.gravityScale = 0;
        forceGravity = true;
    }

    public void ResetGravityToDefault()
    {
        rb.gravityScale = defaultGravity;
        forceGravity = false;
    }
}