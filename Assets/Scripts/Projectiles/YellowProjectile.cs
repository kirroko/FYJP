using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowProjectile : Projectile
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        AI enemy = collision.gameObject.GetComponent<AI>();

        if (enemy != null)
        {
            enemy.IsStunned = true;
        }
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
        speed = 0f;
        StartCoroutine(DelayDestroy(0.3f));
    }
}
