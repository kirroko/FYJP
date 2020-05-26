using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * This is a Spike Obstacle that deals damage when stepping on it
 */
public class RegularSpikes : MonoBehaviour
{
    ///Amount the player should be knockback by upon stepping on it
    [SerializeField] private float playerKnockbackAmt = 5f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerInfo player = collision.gameObject.GetComponent<PlayerInfo>();
        if (player != null)
            player.TakeDamage(1, playerKnockbackAmt);
    }
}
