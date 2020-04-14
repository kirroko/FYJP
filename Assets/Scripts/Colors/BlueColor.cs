using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlueColor", menuName = "Colors/Blue", order = 5)]
public class BlueColor : WhiteColor
{
    [SerializeField] private BlueProjectile projectile = null;
    [SerializeField] private float projectileSpeed = 5f;

    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private int maxDash = 3;
    [SerializeField] private float waitTimeDuration = 1f;
    [SerializeField] private float dashCDDuration = 1f;

    private float waitTime = 0f;
    private float dashCD = 0f;

    private int dashCount = 0;
    private bool hasDashed = false;

    private HoldButton dashButton = null;
    private Joystick movementInput = null;
    private Rigidbody2D playerRB = null;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);
        dashButton = ObjectReferences.instance.dashButton;
        movementInput = ObjectReferences.instance.movementInput;
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    public override void UpdateAbility(GameObject player)
    {
        base.UpdateAbility(player);

        waitTime -= Time.deltaTime;
        dashCD -= Time.deltaTime;

        //Shoot projectiles
        if (!abilityInput.IsPressed && abilityActivated)
        {
            abilityActivated = false;

            if (dir.Equals(Vector2.zero)) return;

            Shoot(projectile, projectileSpeed, player);
        }

        //Activate Dash
        if (dashButton.tap)
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

        Vector2 direction = Vector3.zero;

        //Move Right
        if (movementInput.Direction.x > 0.5f)
            direction.x = 1f;
        else if (movementInput.Direction.x < -0.5f)
            direction.x = -1f;

        if (movementInput.Direction.y > 0.5f)
            direction.y = 1f;
        else if (movementInput.Direction.y < -0.5f)
            direction.y = -1f;

        playerRB.velocity += direction * dashSpeed;
        //playerRB.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
    }

    private void ResetDash()
    {
        hasDashed = false;
        dashCD = dashCDDuration;
        dashCount = 0;
    }
}
