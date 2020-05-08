using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointPatrol : AI
{
    [SerializeField] private Transform[] moveSpots = null;
    [SerializeField] private float startWaitTime = 1f;

    private int randomSpots = 0;
    private float waitTime = 0f;

    protected override void Start()
    {
        base.Start();
        waitTime = startWaitTime;
        randomSpots = Random.Range(0, moveSpots.Length);
    }

    protected override void Update()
    {
        base.Update();

        if (stun || dead) return;

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
}
