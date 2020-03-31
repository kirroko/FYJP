using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : AI
{
    [SerializeField] private float moveSpeed = 5f;

    public Vector2 dir = new Vector2(1f, 0f);
    private new Collider2D collider = null;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (stun) return;
        if (!Physics2D.Raycast(transform.position + new Vector3(collider.bounds.extents.x * dir.x, 0f, 0f), Vector2.down, collider.bounds.extents.y + 0.1f))
        {
            dir.x = -dir.x;
        }

        Vector3 targetPos = transform.position;

        targetPos.x += dir.x * moveSpeed * Time.deltaTime;

        transform.position = targetPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);
        if(contact.normal.x != 0f)
            dir.x = -dir.x;
    }
}
