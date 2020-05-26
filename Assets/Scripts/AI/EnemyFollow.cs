using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : AI
{
    [SerializeField] private float detectionDistance = 5f;

    private Transform target = null;

    protected override void Start()
    {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();

        if (stun || dead) return;

        if (target == null)
        {
            Debug.Log("Target player Found");
            target = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }

        // Flip animation
        Vector2 temp = target.position - transform.position;
        temp.Normalize();

        if (temp.x > 0)
            sr.flipX = true;
        else
            sr.flipX = false;

        if (Vector2.Distance(transform.position, target.position) < detectionDistance)
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        else if (Vector2.Distance(transform.position, target.position) < 10)
            transform.Translate(-Vector2.right * speed * Time.deltaTime);

    }
}

