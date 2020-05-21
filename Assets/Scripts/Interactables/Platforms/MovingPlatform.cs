using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool Charging
    {
        get
        {
            return isCharging;
        }

        set
        {
            if (needCharge)
                isCharging = value;
        }
    }

    [Header("Movement")]
    [SerializeField] private Vector3[] distToTravel = null;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Settings")]
    [SerializeField] private bool loop = false;
    [SerializeField] private bool needCharge = false;

    private bool isCharging = false;
    private Transform onPlatform = null;

    private int index = 0;
    private int add = 1;

    // REFERENCES
    private SpriteRenderer sr;
    private Animator ani;

    private void Start()
    {
        for (int i = 0; i < distToTravel.Length; ++i)
        {
            distToTravel[i] += transform.position;
        }

        if (!needCharge)
            isCharging = true;

        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        if (ani == null) return;
        ani.SetBool("Charge", needCharge);
    }

    private void Update()
    {
        if (isCharging)
        {
            float dist = (distToTravel[index] - transform.position).magnitude;

            if (dist <= 0.1f)
            {
                index += add;

                //Reached end of path
                if (index >= distToTravel.Length)
                {
                    if (loop)
                    {
                        index = 0;
                    }
                    else
                    {
                        add = -add;
                        index += add;
                    }
                }
                else if (index < 0 && !loop)//Reached Start Path
                {
                    add = -add;
                    index += add;
                }
            }

            Vector3 dir = (distToTravel[index] - transform.position).normalized;

            if(sr != null)
            {
                if (dir.x > 0)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }

            transform.position += dir * moveSpeed * Time.deltaTime;
            if (onPlatform != null)
                onPlatform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        //Check if collided object is below 
        if (contact.normal.y < 0f && collision.gameObject.GetComponent<PlayerInfo>())
        {
            onPlatform = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlatform = null;
    }
}
