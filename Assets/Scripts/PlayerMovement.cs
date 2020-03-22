using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public bool canMove = false;

    private Rigidbody2D rb = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (!canMove) return;

        Vector3 tempPos = transform.position;

        tempPos.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") != 0)
            GetComponent<PlayerInfo>().dir = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x , jumpForce);
        }

        transform.position = tempPos;
    }
}
