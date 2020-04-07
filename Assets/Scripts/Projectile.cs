using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    private Vector3 dir = Vector3.zero;
    private float speed = 0f;

    public void Init(Vector2 dir, float speed)
    {
        this.dir = new Vector3(dir.x, dir.y, 0f);
        this.speed = speed;
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
