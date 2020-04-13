using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;

    protected Vector3 dir = Vector3.zero;
    protected float speed = 0f;

    public void Init(Vector2 dir, float speed)
    {
        this.dir = new Vector3(dir.x, dir.y, 0f);
        this.speed = speed;
    }

    protected virtual void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
            Destroy(gameObject);

        transform.position += dir * speed * Time.deltaTime;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
