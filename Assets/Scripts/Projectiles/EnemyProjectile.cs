using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInfo player = collision.gameObject.GetComponent<PlayerInfo>();

        if (player != null)
        {
            player.TakeDamage(1, 5f);
        }

        AI aI= collision.gameObject.GetComponent<AI>();

        if (aI != null)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
        speed = 0f;
        StartCoroutine(DelayDestroy(0.25f));
    }
}
