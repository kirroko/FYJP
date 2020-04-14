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
            LevelManager.instance.RestartLevel();
        }
        Destroy(gameObject);
    }
}
