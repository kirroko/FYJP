﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class should be on objects that upon colliding should cause the player to respawn regardless of their health
 */

[RequireComponent(typeof(BoxCollider2D))]
public class Respawn : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerInfo>() != null)
            collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(10000, 0f);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerInfo>() != null)
            collision.gameObject.GetComponent<PlayerInfo>().TakeDamage(10000, 0f);
    }
}
