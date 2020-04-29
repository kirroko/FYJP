using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExtraHeart : MonoBehaviour
{
    [SerializeField] private int heartsToGive = 1;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInfo player = collision.GetComponent<PlayerInfo>();

        if(player != null)
        {
            player.GainHeart(heartsToGive);
            Destroy(gameObject);
        }
    }
}
