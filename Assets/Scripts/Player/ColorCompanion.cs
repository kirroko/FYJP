using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCompanion : MonoBehaviour
{
    [SerializeField] private Vector2 offset = Vector2.zero;
    [SerializeField] private float bobIntensity = 5f;
    [SerializeField] private float bobSpeed = 5f;
    [SerializeField] private Vector2 followSpeed = new Vector2(5f, 1f);

    private Joystick moveInput = null;

    private float sinValue = 0f;
    private SpriteRenderer sr = null;

    private float facingDir = 1f;
    private float prevDir = 1f;

    private Transform player = null;

    private void Start()
    {
        moveInput = ObjectReferences.instance.movementInput;

        transform.position = transform.parent.position + new Vector3(offset.x, offset.y);

        // REFERENCE
        sr = GetComponent<SpriteRenderer>();
        player = transform.parent;

        transform.SetParent(null);
    }

    private void LateUpdate()
    {
        if (Application.platform == RuntimePlatform.Android)
            UpdateMobileVer();
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            UpdatePCVer();

        float dir = Mathf.Abs(transform.position.x - player.position.x);
        if (dir <= 0.1f) sr.flipX = player.GetComponent<SpriteRenderer>().flipX;

        Vector3 targetPos = player.position + new Vector3(offset.x * facingDir, offset.y);

        Vector3 tempPos = transform.position;

        tempPos.x = Mathf.Lerp(tempPos.x, targetPos.x, followSpeed.x * Time.deltaTime);
        tempPos.y = Mathf.Lerp(tempPos.y, targetPos.y, followSpeed.y * Time.deltaTime);

        transform.position = tempPos;
        prevDir = facingDir;

        if (Time.timeScale != 0f)
        {
            transform.parent = player;
            Vector3 targetLocalPos = transform.localPosition;
            sinValue += Time.deltaTime * bobSpeed;
            targetLocalPos.y += Mathf.Sin(sinValue) * bobIntensity;
            transform.localPosition = targetLocalPos;
            transform.parent = null;
        }
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
    }

    private void UpdateMobileVer()
    {
        if (moveInput.Direction.x != 0) facingDir = Mathf.Sign(moveInput.Direction.x);
    }
}
