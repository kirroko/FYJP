using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCompanion : MonoBehaviour
{
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private float bobIntensity = 5f;
    [SerializeField] private float bobSpeed = 5f;

    private Joystick moveInput = null;

    private float sinValue = 0f;
    private float prevX = 0f;
    private bool stopMove = false;
    private SpriteRenderer sr = null;

    private bool offsetBool = false;

    private void Start()
    {
        moveInput = ObjectReferences.instance.movementInput;

        transform.position = transform.parent.position + new Vector3(offset.x, offset.y);

        // REFERENCE
        sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        //Making it Bob
        Vector3 targetLocalPos = transform.localPosition;
        sinValue += Time.deltaTime * bobSpeed;
        targetLocalPos.y += Mathf.Sin(sinValue) * bobIntensity;
        transform.localPosition = targetLocalPos;

        //Player is moving towards companion

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            float dir = 0f;

            if (Input.GetKey(KeyCode.A))
                dir = -1f;
            else if (Input.GetKey(KeyCode.D))
                dir = 1f;
            else
                dir = 0f;

            if (dir != 0 &&
                Mathf.Sign(dir) == Mathf.Sign(transform.position.x - transform.parent.position.x))
            {
                stopMove = true;
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (moveInput.Direction.x != 0 &&
                Mathf.Sign(moveInput.Direction.x) == Mathf.Sign(transform.position.x - transform.parent.position.x))
            {
                stopMove = true;
            }
        }

        if (stopMove)
        {
            //Stop the companion from moving
            Vector3 tempPos = transform.position;
            tempPos.x = prevX;
            transform.position = tempPos;

            float dist = transform.position.x - transform.parent.position.x;
            if (Mathf.Abs(dist) <= 0.1f)
                offsetBool = true;

            if (Mathf.Abs(dist) >= Mathf.Abs(offset.x) && offsetBool == true)
            {
                stopMove = false;
                offsetBool = false;
                sr.flipX = transform.parent.GetComponent<SpriteRenderer>().flipX;
            }
        }

        stopMove = false;
        prevX = transform.position.x;
    }
}
