using UnityEngine;

public class RedProjectile : Projectile
{
    private void Start()
    {
        Debug.Log("Red projectile degree: " + degree);
        transform.Rotate(new Vector3(0, 0, 1), degree);
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
