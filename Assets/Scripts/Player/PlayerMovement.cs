using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Joystick input = null;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("General")]
    [SerializeField] private float minJumpDrag = 10f;

    private Rigidbody2D rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 targetPos = transform.position;

        targetPos.x += input.Horizontal * moveSpeed * Time.deltaTime;

        if (Gesture.lastSwipe.y > minJumpDrag)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (input.Horizontal < 0f)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if( input.Horizontal > 0f)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        transform.position = targetPos;
    }
}
