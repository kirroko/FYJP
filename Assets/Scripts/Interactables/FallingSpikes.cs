using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is an obstacle that will drop and deal damage when the player walks below it
 */
[RequireComponent(typeof(Rigidbody2D))]
public class FallingSpikes : Respawnable
{
    ///Duration to wait before dropping
    [SerializeField] private float waitTime = 3f;

    ///Amount to knock player back when in contact
    [SerializeField] private float playerKnockbackAmt = 1f;

    private bool start = false;
    private float time = 0f;

    private Rigidbody2D rb = null;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        time = waitTime;
    }

    private void Update()
    {
        if (!start) return;

        time -= Time.deltaTime;

        if (time <= 0f)
            rb.gravityScale = 1f;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerInfo>() != null)
        {
            time = waitTime;
            start = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInfo player = collision.gameObject.GetComponent<PlayerInfo>();
        if (player != null)
            player.TakeDamage(1, playerKnockbackAmt);

        ContactPoint2D contact = collision.GetContact(0);
        if (contact.normal.y > 0f && rb.gravityScale > 0f)
            Gone();
    }
}
