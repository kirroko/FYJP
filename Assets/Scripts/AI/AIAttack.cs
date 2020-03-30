using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    [SerializeField] private PatrolAI parent = null;

    private float cooldown = 0f;
    private float damage = 10f;
    private float dir = 1f;

    private new Collider2D collider = null;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if(dir != parent.dir.x)
        {
            collider.offset = new Vector2(-collider.offset.x, collider.offset.y);
            dir = parent.dir.x;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        cooldown -= Time.deltaTime;

        if (cooldown > 0f) return;

        PlayerInfo info = collision.GetComponent<PlayerInfo>();

        if(info != null)
        {
            info.TakeDamage(damage);
            cooldown = 1f;
        }
    }
}
