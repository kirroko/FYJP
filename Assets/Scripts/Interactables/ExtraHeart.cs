using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This Object will give the player an extra heart when collecting it
 */
[RequireComponent(typeof(Collider2D))]
public class ExtraHeart : Respawnable
{
    ///Amount of hearts to give
    [SerializeField] private int heartsToGive = 1;

    protected override void Start()
    {
        base.Start();

        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInfo player = collision.GetComponent<PlayerInfo>();

        if(player != null)
        {
            player.GainHeart(heartsToGive);
            Gone();
        }
    }
}
