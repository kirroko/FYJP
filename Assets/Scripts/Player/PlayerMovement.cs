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
    [SerializeField] private float dampForce = 0.05f;
    [SerializeField] private float jumpForce = 5f;

    [Header("General")]
    [SerializeField] private float minJumpDrag = 10f;

    private Rigidbody2D rb = null;
    private Rigidbody rb3D = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb3D = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Vector3 targetPos = transform.position;

        //targetPos.x += input.Horizontal * moveSpeed * Time.deltaTime;
        ////For 3D
        //targetPos.z += input.Vertical * moveSpeed * Time.deltaTime;

        //if (Gesture.lastSwipe.y > minJumpDrag)
        //{
        //    rb3D.velocity = new Vector2(rb3D.velocity.x, jumpForce);
        //}

        //if (input.Horizontal < 0f)
        //    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        //else if( input.Horizontal > 0f)
        //    transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //transform.position = targetPos;
        if (Gesture.lastSwipe.y > minJumpDrag)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(input.Horizontal * moveSpeed, rb.velocity.y);
        Vector3 temp = Vector3.zero;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref temp, dampForce);


    }
}
