using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointPatrol : AI
{
    [SerializeField] private Transform[] moveSpots = null;
    [SerializeField] private float startWaitTime = 1f;

    private int currentIndex = 0;
    private float waitTime = 0f;

    protected override void Start()
    {
        base.Start();
        waitTime = startWaitTime;
    }

    protected override void Update()
    {
        base.Update();

        if (stun || dead) return;

        Vector3 targetPos = transform.position;
        targetPos.x = Mathf.MoveTowards(transform.position.x, moveSpots[currentIndex].position.x, speed * Time.deltaTime);
        // Debug.Log("Current: " + transform.position + " | Target: " + targetPos);
        transform.position = targetPos;
        // Debug.Log("Distance: " + Vector2.Distance(transform.position, moveSpots[currentIndex].position));
        Debug.Log("Abs Distance: " + Mathf.Abs(transform.position.x - moveSpots[currentIndex].position.x));
        if(Mathf.Abs(transform.position.x - moveSpots[currentIndex].position.x) < 0.2f)
        {
            if (waitTime <= 0)
            {
                ++currentIndex;
                if (currentIndex >= moveSpots.Length)
                    currentIndex = 0;
                waitTime = startWaitTime;
            }
            else
                waitTime -= Time.deltaTime;
        }
        //if (Vector2.Distance(transform.position, moveSpots[currentIndex].position) < 0.2f)
        //{
        //    if (waitTime <= 0)
        //    {
        //        ++currentIndex;
        //        if (currentIndex >= moveSpots.Length)
        //            currentIndex = 0;
        //        waitTime = startWaitTime;
        //    }
        //    else
        //        waitTime -= Time.deltaTime;
        //}
    }
}
