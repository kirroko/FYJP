using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallingSpikes : MonoBehaviour
{
    [SerializeField] private float waitTime = 3f;

    private bool start = false;
    private float time = 0f;

    private Rigidbody2D rb = null;

    private void Start()
    {
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
        ContactPoint2D contact = collision.GetContact(0);
        if(contact.normal.y > 0f && rb.gravityScale > 0f)
            Destroy(gameObject);
    }
}
