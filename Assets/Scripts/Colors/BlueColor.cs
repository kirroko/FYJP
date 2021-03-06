﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlueColor", menuName = "Colors/Blue", order = 5)]
public class BlueColor : BaseColor
{
    [SerializeField] private BlueProjectile projectile = null;
    [SerializeField] private float projectileSpeed = 5f;

    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private int maxDash = 3;
    [SerializeField] private float waitTimeDuration = 1f;
    [SerializeField] private float dashCDDuration = 1f;
    [SerializeField] private float dashFalloffDuration = 0.15f;

    private float waitTime = 0f;
    private float dashCD = 0f;

    private int dashCount = 0;
    private bool hasDashed = false;
    private float dashFalloff = 0f;
    private float defaultGravity = 0f;

    private HoldButton dashButton = null;
    private Joystick movementInput = null;
    private Rigidbody2D playerRB = null;
    private PlayerMovement playerMovement = null;
    private Animator playerAnimator = null;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);
        AddEvent();

        dashButton = ObjectReferences.instance.dashButton;
        movementInput = ObjectReferences.instance.movementInput;
        playerRB = player.GetComponent<Rigidbody2D>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAnimator = player.GetComponent<Animator>();

        defaultGravity = playerMovement.DefaultGravity;
    }

    public override void UpdateAbility(GameObject player)
    {
        base.UpdateAbility(player);

        waitTime -= Time.deltaTime;
        dashCD -= Time.deltaTime;
        dashFalloff -= Time.deltaTime;

        if (dashFalloff < 0) // reset gravity
            playerRB.gravityScale = defaultGravity;

        //Shoot projectiles
        if (!abilityInput.IsPressed && abilityActivated)
        {
            abilityActivated = false;

            if (dir.Equals(Vector2.zero)) return;

            Shoot(projectile, projectileSpeed, player);
        }

        //Activate Dash
        if (dashButton.tap || Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        //All dash have been used up so go to cooldown OR
        //Dashed at least once and didnt use dash again within a period of time
        if (dashCount >= maxDash || (hasDashed && waitTime <= 0f))
        {
            ResetDash();
        }

    }

    private void Dash()
    {
        //Dash still on cooldown
        if (dashCD > 0f) return;

        ++dashCount;
        hasDashed = true;
        waitTime = waitTimeDuration;
        dashFalloff = dashFalloffDuration;



        Vector2 direction = movementInput.Direction;

        if (direction == Vector2.zero)
            direction.x = playerMovement.GetLastXDir;

        //if (direction.y != 0 && direction.x != 0)
        //    playerAnimator.SetTrigger("DiagonalDash");
        //else
        //    playerAnimator.SetTrigger("Dash");

        if (direction.x == 0 && direction.y > 0) // JOYSTICK FACING UPWARDS
            playerAnimator.SetTrigger("UpDash");
        else if (direction.x == 0 && direction.y < 0) // JOYSTICK FACING DOWNWARDS
            playerAnimator.SetTrigger("DownDash");
        else if (direction.x != 0 && direction.y > 0) // JOYSTICK FACING UPWARDS EITHER LEFT OR RIGHT
            playerAnimator.SetTrigger("DiagonalDash");
        else if (direction.x != 0 && direction.y < 0) // JOYSTICK FACING DOWNWARDS EITHER LEFT OF RIGHT
            playerAnimator.SetTrigger("BRDiagonalDash");
        else
            playerAnimator.SetTrigger("Dash");

        playerRB.velocity = Vector2.zero;
        playerRB.gravityScale = 0f;
        Vector3 force = direction * dashSpeed;
        force = Vector3.ClampMagnitude(force, dashSpeed);
        playerRB.AddForce(force, ForceMode2D.Impulse);
    }

    private void ResetDash()
    {
        hasDashed = false;
        dashCD = dashCDDuration;
        dashCount = 0;
        dashButton.GetComponentInChildren<CooldownIndicator>().StartCooldown(dashCD);
    }
}
