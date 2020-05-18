using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAI : AI
{
    [SerializeField] private float jumpInterval = 3f;

    private Rigidbody2D rb = null;
    private float jumpTime = 0f;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        jumpTime = jumpInterval;
    }

    protected override void Update()
    {
        base.Update();

        jumpTime -= Time.deltaTime;

        if(jumpTime <= 0f)
        {
            rb.AddForce(Vector2.up * moveSpeed, ForceMode2D.Impulse);
            jumpTime = jumpInterval;
        }
    }
}
