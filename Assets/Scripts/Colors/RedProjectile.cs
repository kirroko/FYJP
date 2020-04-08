using UnityEngine;

public class RedProjectile : Projectile
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        AI enemy = collision.gameObject.GetComponent<AI>();

        if (enemy != null)
        {
            if (enemy.IsTagged)//Enemy alrd tagged so kill it
                Destroy(enemy.gameObject);
            else
                enemy.IsTagged = true;
        }
        Destroy(gameObject);
    }
}
