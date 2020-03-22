using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public bool frozen = false;
    public Vector3[] points = null;

    private int index = 0;
    private float atkCD = 0f;

    private float freezeCD = 5f;

    private void Start()
    {
        for(int i = 0; i <points.Length;++i)
        {
            points[i] += transform.position;
        }
    }

    private void Update()
    {
        if (frozen)
        {
            freezeCD -= Time.deltaTime;

            if(freezeCD <= 0f)
            {
                freezeCD = 5f;
                GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
                frozen = false;
            }
            return;
        }

        if((points[index] - transform.position).magnitude <= 0.1f)
        {
            if (index == 0)
                index = 1;
            else if (index == 1)
                index = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, points[index], moveSpeed * Time.deltaTime);

        atkCD -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<MinionMovement>() != null)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (atkCD > 0f || frozen) return;

        PlayerInfo info = collision.gameObject.GetComponent<PlayerInfo>();
        if (info != null)
        {
            info.TakeDamage(1);
            atkCD = 1f;
        }
    }
}
