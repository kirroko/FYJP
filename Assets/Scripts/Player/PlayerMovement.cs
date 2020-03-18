using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Joystick input = null;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody2D rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 targetPos = transform.position;

        targetPos.x += input.Horizontal * moveSpeed * Time.deltaTime;

        if (Gesture.lastSwipe.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        transform.position = targetPos;
    }

    private void LateUpdate()
    {
        Gesture.lastSwipe = Vector2.zero;
    }
}
