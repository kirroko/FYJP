﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is the projectile that the yellow color shoots
 */
public class YellowProjectile : Projectile
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerInfo>() != null)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }

        AI enemy = collision.gameObject.GetComponent<AI>();

        if (enemy != null)
        {
            enemy.IsStunned = true;
        }
        AudioManager.PlaySFX("Hit", false);
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
        speed = 0f;
        StartCoroutine(DelayDestroy(0.3f));
    }
}
