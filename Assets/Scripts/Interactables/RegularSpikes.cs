using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSpikes : MonoBehaviour
{
    [SerializeField] private float playerKnockbackAmt = 1f;

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerInfo player = collision.gameObject.GetComponent<PlayerInfo>();
        if (player != null)
            player.TakeDamage(1, playerKnockbackAmt);
    }
}
