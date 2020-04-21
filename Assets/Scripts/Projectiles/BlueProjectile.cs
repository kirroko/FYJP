using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueProjectile : Projectile
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
            enemy.IsFrozen = true;
        }
        Destroy(gameObject);
    }
}
