using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Add This Script to Any Gameobject that the player can break by dashing into it
 */
public class DashBreak : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            if (player.StillDashing)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());//Ignores collision so player can go thru
                player.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;//Gives player vel before they collide
                Destroy(gameObject);
            }
        }
    }
}
