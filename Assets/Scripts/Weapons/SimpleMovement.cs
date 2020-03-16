using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float moveSpeed = 0f;
    public float dir = 0f;

    private void Update()
    {
        //Vector3 tempPos = transform.position;

        if (Input.GetAxisRaw("Horizontal") != 0)
            dir = Input.GetAxisRaw("Horizontal");

        //tempPos.x += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

        //transform.position = tempPos;
    }
}
