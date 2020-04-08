using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointPatrol : MonoBehaviour
{
    public float speed;
    private float waitTime;
    public float startWaitTime;

    public Transform[] moveSpots;
    private int randomSpots;
    //public float minX;
    //public float minY;
    //public float maxX;
    //public float maxY;

    void Start()
    {
        waitTime = startWaitTime;
        randomSpots = Random.Range(0, moveSpots.Length);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpots].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpots[randomSpots].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpots = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
                waitTime -= Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(collision.collider.gameObject);
    }
}
