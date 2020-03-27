using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlueColor", menuName = "Colors/Blue", order = 5)]
public class BlueColor : WhiteColor
{
    [Header("Ability Related")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private int maxDash = 3;
    [SerializeField] private float waitTimeDuration = 1f;
    [SerializeField] private float dashCDDuration = 1f;

    private float waitTime = 0f;
    private float dashCD = 0f;

    private int dashCount = 0;
    private bool hasDashed = false;

    private HoldButton dashButton = null;
    private Joystick input = null;
    private Rigidbody2D playerRB = null;

    public override void UpdateAbility(GameObject player)
    {
        waitTime -= Time.deltaTime;
        dashCD -= Time.deltaTime;

        if (!once)
            DoOnce(player);
        
        //Activate Dash
        if(dashButton.tap)
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

    private void DoOnce(GameObject player)
    {
        once = true;
        dashButton = player.GetComponent<PlayerMovement>().GetDashButton;
        input = player.GetComponent<PlayerMovement>().GetInput;
        playerRB = player.GetComponent<Rigidbody2D>();
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
        if (input.Direction.x > 0.5f)
            direction.x = 1f;
        else if (input.Direction.x < -0.5f)
            direction.x = -1f;

        if (input.Direction.y > 0.5f)
            direction.y = 1f;
        else if (input.Direction.y < -0.5f)
            direction.y = -1f;

        playerRB.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
    }

    private void ResetDash()
    {
        hasDashed = false;
        dashCD = dashCDDuration;
        dashCount = 0;
    }
}
