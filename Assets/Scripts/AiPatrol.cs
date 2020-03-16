using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrol : MonoBehaviour
{
    public float speed;
    public float distance;

    public float nextShot;
    public float shootingInterval;

    public bool movingRight = true;

    public Transform groundDetection;
    public Transform gunPlaceHolder;

    public GameObject bullet;
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        shootingInterval -= Time.deltaTime;

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
        if(nextShot >= shootingInterval)
        {
            GameObject temp = Instantiate(bullet, gunPlaceHolder.transform.position , transform.rotation);
            Destroy(temp, 3);
            Debug.Log(shootingInterval);
            shootingInterval = 1.5f;
        }
    }
}

