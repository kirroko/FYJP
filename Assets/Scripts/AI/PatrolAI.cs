using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : AI
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask groundMask;

    public Vector2 dir = new Vector2(1f, 0f);
    private Collider2D collider = null;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!Physics2D.Raycast(transform.position + new Vector3(collider.bounds.extents.x * dir.x, 0f, 0f), Vector2.down, collider.bounds.extents.y + 0.1f, groundMask))
        {
            dir.x = -dir.x;
        }

        Vector3 targetPos = transform.position;

        targetPos.x += dir.x * moveSpeed * Time.deltaTime;

        transform.position = targetPos;
    }
}
