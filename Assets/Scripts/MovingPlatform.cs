using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3[] distToTravel = null;

    public bool autoStart = false;
    public bool loop = false;
    public bool needCharge = false;
    public bool charging = false;
    public float speed = 10f;
    public Transform onPlatform = null;

    private int index = 0;
    private int add = 1;

    void Start()
    {
        for(int i = 0; i < distToTravel.Length; ++i)
        {
            distToTravel[i] += transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(autoStart)
        {
            float dist = (distToTravel[index] - transform.position).magnitude;

            if(dist <= 0.1f)
            {
                index += add;
                if (needCharge && !charging && index < 0)
                {
                    autoStart = false;
                    index = 0;
                    return;
                }

                if (index >= distToTravel.Length && loop)
                    index = 0;
                else if ((index >= distToTravel.Length || index < 0) && !loop) 
                {
                    add = -add;
                    index += add;
                }
            }

            Vector3 dir = (distToTravel[index] - transform.position).normalized;

            transform.position += dir * speed * Time.deltaTime;
            if(onPlatform != null)
                onPlatform.position += dir * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(needCharge && collision.gameObject.GetComponent<PlayerInfo>().colour == PAINT_COLOURS.YELLOW)
        {
            autoStart = true;
            charging = true;
            onPlatform = collision.transform;
        }
        else if(!needCharge)
        {
            autoStart = true;
            onPlatform = collision.transform;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onPlatform = null;
        if (!needCharge) return;

        charging = false;

        if (add > 0)
            add = -add;
    }
}
