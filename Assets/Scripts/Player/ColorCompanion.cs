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
    private float facingDir = 1f;

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

        float offsetDist = Mathf.Abs(transform.position.x - transform.parent.position.x);

        //Companion is further thn offset so bring it closer to player
        if (offsetDist > Mathf.Abs(offset.x))
        {
            stopMove = false;
            Vector3 tempPos = transform.position;

            tempPos.x = Mathf.Lerp(transform.position.x, transform.parent.position.x + offset.x * facingDir, Time.deltaTime);
            transform.position = tempPos;
        }


        if (stopMove)
        {
            //Stop the companion from moving
            Vector3 tempPos = transform.position;
            tempPos.x = prevX;
            transform.position = tempPos;

            float dist = transform.position.x - transform.parent.position.x;
            if (Mathf.Abs(dist) <= 0.1f)//Player is very near companion so can say companion shld flip
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

        if (dir != 0f) facingDir = Mathf.Sign(dir);

        if (facingDir == Mathf.Sign(transform.position.x - transform.parent.position.x))//Player is facing companion
        {
            stopMove = true;
        }

        //Player was moving towards companion but changes dir and moves away from companion before going pass companion
        if (stopMove &&
            facingDir != Mathf.Sign(transform.position.x - transform.parent.position.x) &&   //Player not facing companion
            !offsetBool)    //So tat Companion will not start following player once companion is almost same pos as player
        {
            stopMove = false;
        }
    }

    private void UpdateMobileVer()
    {
        if (moveInput.Direction.x != 0) facingDir = Mathf.Sign(moveInput.Direction.x);

        if (facingDir == Mathf.Sign(transform.position.x - transform.parent.position.x))//Player is facing companion
        {
            stopMove = true;
        }

        //Player was moving towards companion but changes dir and moves away from companion before going pass companion
        if (stopMove &&
            facingDir != Mathf.Sign(transform.position.x - transform.parent.position.x) &&//Player not facing companion
            !offsetBool)    //So tat Companion will not start following player once companion is almost same pos as player
        {
            stopMove = false;
        }
    }
}
