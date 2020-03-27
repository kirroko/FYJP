using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            if (player.IsDashing() && collision.relativeVelocity.magnitude > 10f && 
                player.GetComponent<PlayerColor>().GetCurrentColor.GetMain == COLORS.PURPLE)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                player.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;
                player.ResetDash();
                Destroy(gameObject);
            }
        }
    }
}
