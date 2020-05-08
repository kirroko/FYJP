using UnityEngine;

public class RedProjectile : Projectile
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerInfo>() != null)
        {
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
            return;
        }

        AI enemy = collision.gameObject.GetComponent<AI>();

        if (enemy != null)
        {
            enemy.IsTagged = true;
        }
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
        speed = 0f;
        StartCoroutine(DelayDestroy(0.3f));
    }
}
