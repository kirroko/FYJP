﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBreak : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            if (collision.relativeVelocity.magnitude > 10f)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());//Ignores collision so player can go thru
                player.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;//Gives player vel before they collide
                Destroy(gameObject);
                if(player.GetComponent<PlayerColor>().GetCurrentColor.GetMain == COLORS.PURPLE)
                    player.ResetDash();
            }
        }
    }
}
