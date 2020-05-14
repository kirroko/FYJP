using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishjumping : AI
{

    [SerializeField] private float thrust = 1.0f;
    [SerializeField] private float timer = 3.0f;

    public Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0.0f)
            rb.AddForce(transform.up * thrust);
        if (timer <= -2.0f)
            timer = 2.0f;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

    }
}
