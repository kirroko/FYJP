   using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : AI
{
    [Header("BUllets")]
    [SerializeField] private Projectile bullet = null;
    [SerializeField] private float bulletSpeed = 5f;
    [Header("Settings")]
    [SerializeField] private bool shootable = false;
    [SerializeField] private float nextShot = 2f;

    private float shootingInterval = 0f;

    private Vector2 dir = new Vector2(1f, 0f);
    private new Collider2D collider = null;

    protected override void Start()
    {
        base.Start();
        collider = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();
        shootingInterval -= Time.deltaTime;

        if (stun) return;

        if (!Physics2D.Raycast(transform.position + new Vector3(collider.bounds.extents.x * dir.x, 0f, 0f), Vector2.down, collider.bounds.extents.y + 0.1f))
        {
            dir.x = -dir.x;
        }

        Vector3 targetPos = transform.position;

        targetPos.x += dir.x * speed * Time.deltaTime;

        transform.position = targetPos;

        if(shootable)
        {
            if (nextShot >= shootingInterval)
            {
                Vector3 firePoint = collider.bounds.center + new Vector3(collider.bounds.extents.x, 0f, 0f) * dir.x * 1.25f;

                Projectile temp = Instantiate(bullet, firePoint, Quaternion.identity);
                temp.Init(new Vector2(dir.x, 0f), bulletSpeed);
                shootingInterval = 1.5f;
            }
        }
       
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.GetComponent<Projectile>() != null) return;

        ContactPoint2D contact = collision.GetContact(0);
        if(contact.normal.x != 0f)
            dir.x = -dir.x;
    }
}
