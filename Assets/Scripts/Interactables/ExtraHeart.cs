using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExtraHeart : Respawnable
{
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
