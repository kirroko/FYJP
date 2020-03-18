using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    public enum FollowStyle
    {
        SNAP_FOLLOW,
        SMOOTH_FOLLOW
    }

    [SerializeField]
    private Camera followCamera;
    [SerializeField]
    public Rigidbody2D target;

    public float startDistance;
    public float endDistance;
    public Vector3 startOffset;
    public Vector3 endOffset;
    public Vector3 rotation;
    public FollowStyle style;


    public float duration { private get; set; }

    public float et
    {
        set
        {
            if (value < 0.0f)
            {
                offset = startOffset;
                distance = startDistance;
            }
            else if (value < duration)
            {
                float m = value / duration;
                offset = startOffset + (endOffset - startOffset) * m;
                distance = startDistance + (endDistance - startDistance) * m;
            }
            else
            {
                offset = endOffset;
                distance = endDistance;
            }
        }
    }

    private Vector3 offset;
    //private float magnitude;
    [SerializeField]
    private float distance;

    [SerializeField]
    private float followMultipler = 1.0f;
    private float followTimer = 1.0f;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        if (!followCamera)
        {
            followCamera = GetComponent<Camera>();
            startPos = followCamera.transform.position;
        }
        //magnitude = 1.0f / (startDistance - endDistance);
    }

    void Update()
    {
        if (!target)
            return;

        MoveCamera();
    }

    void FixedUpdate()
    {
        if (!target)
            return;

        MoveCamera();
    }

    void MoveCamera()
    {
        transform.localEulerAngles = rotation;

        switch (style)
        {
            case FollowStyle.SNAP_FOLLOW:
                followCamera.transform.position = new Vector3(target.position.x,target.position.y,0) + offset + Quaternion.Euler(rotation) * Vector3.back * distance;
                break;
            case FollowStyle.SMOOTH_FOLLOW:
                if (target.velocity.magnitude > 0)
                {
                    startPos = followCamera.transform.position;
                    followTimer = Mathf.Clamp01(Time.deltaTime * followMultipler);
                }
                else
                {
                    followTimer = Mathf.Clamp01(followTimer + Time.deltaTime * followMultipler);
                }
                followCamera.transform.position = Vector3.Lerp(startPos, new Vector3(target.position.x,target.position.y,0) + offset + Quaternion.Euler(rotation) * Vector3.back * distance, followTimer);
                break;
        }
    }
}