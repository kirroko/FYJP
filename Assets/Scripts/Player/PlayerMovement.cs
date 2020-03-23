using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Joystick input = null;
    [SerializeField] private GameObject cameraController = null;

    [Header("Stats")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rotateSpeed = 5f;

    [Header("General")]
    [SerializeField] private float minJumpDrag = 10f;

    private Rigidbody rb = null;

    private float rotateY = 90f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 targetPos = transform.position;

        //targetPos.x += input.Horizontal * moveSpeed * Time.deltaTime;
        //targetPos.z += input.Vertical * moveSpeed * Time.deltaTime;

        //if (Gesture.lastSwipe.y > minJumpDrag)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //}

        //if (input.Horizontal < 0f)
        //    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        //else if (input.Horizontal > 0f)
        //    transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        //For PC
        targetPos.z -= Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        targetPos.x += Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetAxisRaw("Horizontal") < 0f)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else if (Input.GetAxisRaw("Horizontal") > 0f)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);


        transform.position = targetPos;

        //Camera Turn Right
        if(Input.GetKey(KeyCode.L))
        {
            rotateY += rotateSpeed * Time.deltaTime;
            cameraController.transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
        }
        else if(Input.GetKeyDown(KeyCode.K))
        {
            rotateY -= rotateSpeed * Time.deltaTime;
            cameraController.transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
        }
    }
}
