using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShard : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 dir = new Vector2();

    private void Start()
    {
        Destroy(gameObject, 8f);
    }

    public void Init(Vector2 dir)
    {
        this.dir = dir;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(dir.x, dir.y), moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MinionMovement minion = collision.gameObject.GetComponent<MinionMovement>();

        if(minion != null)
        {
            minion.frozen = true;
            minion.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f, 1f);
            Destroy(gameObject);
        }
    }
}
