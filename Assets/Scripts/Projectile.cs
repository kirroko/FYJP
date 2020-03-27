using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float speed = 5f;

    private Vector3 dir = Vector3.zero;

    public void Init(Vector2 dir)
    {
        this.dir = new Vector3(dir.x, dir.y, 0f);

        if (dir == Vector2.zero)
            Destroy(gameObject);
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
            Destroy(gameObject);

        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
