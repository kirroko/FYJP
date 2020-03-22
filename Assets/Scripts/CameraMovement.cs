using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject playerRef = null;

    private float startCD = 1f;
    private bool started = false;
    private bool moveToPlayer = false;
    public float moveSpeed = 4f;

    private Camera cam = null;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!started && !moveToPlayer)
        {
            startCD -= Time.deltaTime;

            if (startCD <= 0f)
            {
                moveToPlayer = true;
            }
            return;
        }

        if (moveToPlayer)
        {
            Vector3 tempPos = playerRef.transform.position;
            tempPos.z = -10f;

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 3.5f, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, tempPos, moveSpeed * Time.deltaTime * 1.5f);

            if ((tempPos - transform.position).magnitude <= 0.1f)
            {
                moveToPlayer = false;
                started = true;
                playerRef.GetComponent<PlayerMovement>().canMove = true;
            }
            return;
        }

        if (started)
        {
            Vector3 tempPos = playerRef.transform.position;
            tempPos.z = -10f;
            transform.position = tempPos;
        }
    }
}
