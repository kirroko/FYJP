﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurpleColor", menuName = "Colors/Purple", order = 6)]
public class PurpleColor : BaseColor
{
    [SerializeField] private float abilityDuration = 3f;
    [SerializeField] private float speedModifier = 2f;

    private PlayerMovement movement = null;
    private float abilityLeft = 0f;
    private bool useBefore = false;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);

        EventManager.instance.EnemyCollisionEvent -= EnemyCollisionEvent;
        EventManager.instance.EnemyCollisionEvent += EnemyCollisionEvent;
        movement = player.GetComponent<PlayerMovement>();
        abilityInput.HandleRange = 0f;
        useBefore = false;
    }

    public override void UpdateAbility(GameObject player)
    {
        abilityCD -= Time.deltaTime;
        abilityLeft -= Time.deltaTime;

        if(abilityInput.IsPressed && abilityCD <= 0f)
        {
            abilityCD = abilityInterval;
            abilityLeft = abilityDuration;
            abilityActivated = true;

            abilityInput.GetComponentInChildren<CooldownIndicator>().StartCooldown(abilityInterval);

            movement.IncreaseSpeed(speedModifier);
            useBefore = true;
        }

        if(abilityLeft <= 0f && abilityActivated)
        {
            abilityActivated = false;
            movement.NormalSpeed();
        }
    }

    public override void ExitAbility(GameObject player)
    {
        base.ExitAbility(player);
        abilityLeft = 0f;

        abilityInput.HandleRange = 1f;

        if(useBefore)
            movement.NormalSpeed();
        useBefore = false;
    }

    public override void OnPlayerDestroyed()
    {
        EventManager.instance.EnemyCollisionEvent -= EnemyCollisionEvent;
    }

    private void EnemyCollisionEvent(Collision2D collision, GameObject player)
    {
        if (player.GetComponent<PlayerColor>().GetCurrentColor.GetMain != mainColor) return;

        AI enemy = collision.gameObject.GetComponent<AI>();

        if (enemy == null) return;

        if (player.GetComponent<PlayerMovement>().StillDashing)
        {
            player.GetComponent<Rigidbody2D>().velocity = -collision.relativeVelocity;//Gives player vel before they collide
            player.GetComponent<PlayerMovement>().ResetDash();

            enemy.Die();
        }
    }
}
