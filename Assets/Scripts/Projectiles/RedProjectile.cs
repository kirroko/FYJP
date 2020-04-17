using UnityEngine;

public class RedProjectile : Projectile
{
    private void Awake()
    {
        gameObject.transform.rotation = new Quaternion(0, degree, 0,0);
    }

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
