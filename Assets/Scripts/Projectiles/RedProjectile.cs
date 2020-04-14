using UnityEngine;

public class RedProjectile : Projectile
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        AI enemy = collision.gameObject.GetComponent<AI>();

        if (enemy != null)
        {
            enemy.IsTagged = true;
        }
        Destroy(gameObject);
    }
}
