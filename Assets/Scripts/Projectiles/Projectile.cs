using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is the base class which all projectiles inherit from
 */
public class Projectile : MonoBehaviour
{
    [SerializeField] protected float lifeTime = 5f;

    protected Vector3 dir = Vector3.zero;
    protected float speed = 0f;
    protected float degree = 0f;

    public void Init(Vector2 dir, float speed)
    {
        this.dir = new Vector3(dir.x, dir.y, 0f);
        this.speed = speed;

        if (this.dir.x == 0f && this.dir.y > 0f) // pointing upwards
            degree = 90f;
        else if (this.dir.x == 0f && this.dir.y < 0f) // pointing downwards  
            degree = -90f;
        else if (this.dir.x > 0f && this.dir.y > 0f) // pointing top-right
            degree = 45f;
        else if (this.dir.x < 0f && this.dir.y > 0f) // pointing top-left
            degree = 125f;
        else if (this.dir.x > 0f && this.dir.y < 0f) // pointing btm-right
            degree = -45f;
        else if (this.dir.x < 0f && this.dir.y < 0f) // pointing btm-left
            degree = -125f;
        else if(this.dir.x < 0)
            degree = 180f;
    }

    protected virtual void Start()
    {
        transform.Rotate(new Vector3(0, 0, 1), degree); 
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

    protected IEnumerator DelayDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
