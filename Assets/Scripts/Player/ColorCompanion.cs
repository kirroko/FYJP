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
    private bool tempBool = false;
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
        if(Time.timeScale != 0f)
        {
            Vector3 targetLocalPos = transform.localPosition;
            sinValue += Time.deltaTime * bobSpeed;
            targetLocalPos.y += Mathf.Sin(sinValue) * bobIntensity;
            transform.localPosition = targetLocalPos;
        }


        //Player is moving towards companion

        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            UpdatePCVer();   
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            UpdateMobileVer();
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

            if (Mathf.Abs(dist) >= Mathf.Abs(offset.x) && offsetBool)
            {
                stopMove = false;
                offsetBool = false;
                sr.flipX = transform.parent.GetComponent<SpriteRenderer>().flipX;
            }
        }

        prevX = transform.position.x;
    }


    private void UpdatePCVer()
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

        if (stopMove && dir != 0 &&
            Mathf.Sign(dir) != Mathf.Sign(transform.position.x - transform.parent.position.x) &&
            !offsetBool)
        {
            stopMove = false;
        }

        float offsetDist = Mathf.Abs(transform.position.x - transform.parent.position.x);

        if (offsetDist > Mathf.Abs(offset.x))
        {
            Vector3 tempPos = transform.position;
            tempPos.x = Mathf.Lerp(transform.position.x, transform.parent.position.x + offset.x * Mathf.Sign(dir), Time.deltaTime);
            transform.position = tempPos;
        }
    }

    private void UpdateMobileVer()
    {
        if (moveInput.Direction.x != 0 &&
            Mathf.Sign(moveInput.Direction.x) == Mathf.Sign(transform.position.x - transform.parent.position.x))
        {
            stopMove = true;
        }

        if (stopMove && moveInput.Direction.x != 0 &&
            Mathf.Sign(moveInput.Direction.x) != Mathf.Sign(transform.position.x - transform.parent.position.x) &&
            !offsetBool)
        {
            stopMove = false;
        }

        float offsetDist = Mathf.Abs(transform.position.x - transform.parent.position.x);

        if (stopMove && offsetDist > Mathf.Abs(offset.x))
        {
            Vector3 tempPos = transform.position;
            tempPos.x = transform.parent.position.x + offset.x * -Mathf.Sign(moveInput.Direction.x);
            transform.position = tempPos;
        }
    }
}
