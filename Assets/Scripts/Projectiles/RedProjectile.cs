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
        gameObject.GetComponent<Animator>().SetTrigger("Destroy");
        speed = 0f;
        StartCoroutine(DelayDestroy(0.3f));
    }
}
