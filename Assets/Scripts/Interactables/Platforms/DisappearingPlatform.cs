using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* This is a platform that will disappear after x amt of time once player stand on it
*/
public class DisappearingPlatform : MonoBehaviour
{
    ///Duration to wait before disappearing
    [SerializeField] private float waitDuration = 5f;

    private float waitTime = 0f;
    private Vector3 scale = Vector3.zero;

    private void Start()
    {
        scale = transform.localScale;
    }

    private void Update()
    {
        waitTime -= Time.deltaTime;

        if (waitTime <= 0f)
            transform.localScale = scale;
        else
            transform.localScale = Vector3.zero;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        if (contact.normal.y < 0f && collision.gameObject.GetComponent<PlayerInfo>() != null)
            waitTime = waitDuration;
    }
}
