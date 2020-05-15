using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAI : AI
{
    [SerializeField] private Vector2 direction = Vector2.up;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpInterval = 3f;

    private float jumpTime = 0f;
    private Vector3 startPos = Vector3.zero;
    private Vector3 targetPos = Vector3.zero;

    private bool jump = false;

    protected override void Start()
    {
        base.Start();

        jumpTime = jumpInterval;
        direction.Normalize();

        startPos = transform.position;

        Vector3 temp = direction * jumpForce;
        targetPos = startPos + temp;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //Fish is at start Pos
        float dist = Mathf.Abs((transform.position - startPos).magnitude);
        if (dist <= 0.1f)
            jumpTime -= Time.deltaTime;

        //Fish is jumping out
        if(jumpTime <= 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

            dist = Mathf.Abs((transform.position - targetPos).magnitude);

            if (dist <= 0.1f)
                jumpTime = jumpInterval;
        }
        else//Fish falling back down
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, startPos, speed * Time.deltaTime);
        }
    }
}
