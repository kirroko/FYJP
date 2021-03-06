﻿using UnityEngine;

/**
 * This class is a sub class to AI. It main reason is to allow the gameobject to jump up and down at
 * a set interval deteremind in the inspector
 */

public class JumpingAI : AI
{
    [SerializeField] private float jumpInterval = 3f;

    private Rigidbody2D rb = null;
    private float jumpTime = 0f;
    private bool reduceCooldown = true;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        jumpTime = jumpInterval;
    }

    protected override void Update()
    {
        base.Update();

        if (rb.velocity.y < 0)
            sr.flipY = true;
        else if (rb.velocity.y > 1)
            sr.flipY = false;

        if(reduceCooldown)
            jumpTime -= Time.deltaTime;

        if(jumpTime <= 0f)
        {
            rb.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
            jumpTime = jumpInterval;
            reduceCooldown = false;
        }

        if(rb.velocity.y <= 10f)
        {
            Vector2 tempVel = rb.velocity;
            tempVel.y -= 1f;
            rb.velocity = tempVel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject == transform.parent.gameObject)
            reduceCooldown = true;

        //To jump thru bounds collider
        if (collision.collider.bounds.extents.x * 2f > 50f ||
            collision.collider.bounds.extents.y * 2f > 50f)
        {
            rb.velocity = collision.relativeVelocity;
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        }
    }
}
