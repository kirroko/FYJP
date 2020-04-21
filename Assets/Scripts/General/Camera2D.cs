using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D: MonoBehaviour
{
    [SerializeField] private Transform target = null;

    [SerializeField] private float distance = -10f;//z distance from player

    [SerializeField] private float followSpeed = 3f;

    [SerializeField] private float deadZoneOffsetX = 0f;
    [SerializeField] private float deadZoneX = 0f;
    [SerializeField] private float snapZoneOffsetX = 0f;
    [SerializeField] private float snapZoneX = 0f;

    public float tempTop = 0.1f;
    public float tempBot = 0.15f;


    private float halfDeadZoneX = 0f;
    private float halfSnapZoneX = 0f;

    private bool deadX = false;
    private bool snapX = false;

    //Target
    private Vector2 prevPos = Vector2.zero;

    //Player
    private PlayerMovement player = null;
    private bool wasGrounded = true;

    //Camera
    private Camera cam = null;
    private float prevY = 0f;
    private float currentY = 0f;
    
    private void Start()
    {
        cam = GetComponent<Camera>();

        Vector3 tempPos = transform.position;
        tempPos.z = distance;
        tempPos.y = currentY;
        transform.position = tempPos;

        halfDeadZoneX = deadZoneX * 0.5f;
        halfSnapZoneX = snapZoneX * 0.5f;
        if(target != null)
        {
            prevPos = target.position;

            if (target.GetComponent<PlayerMovement>() != null)
                player = target.GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
        if(target == null)
        {
            GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
            if (tempPlayer != null)
            {
                target = tempPlayer.transform;
                prevPos = target.position;
                player = target.GetComponent<PlayerMovement>();
            }
            return;
        }

        //Determine if camera should smoothly follow, snap or do nothing
        if (target.position.x < transform.position.x + deadZoneOffsetX + halfDeadZoneX && 
            target.position.x > transform.position.x + deadZoneOffsetX - halfDeadZoneX)
        {
            deadX = true;
            snapX = false;
        }
        else if (target.position.x > transform.position.x + snapZoneOffsetX + halfSnapZoneX || 
            target.position.x < transform.position.x + snapZoneOffsetX - halfSnapZoneX)
        {
            snapX = true;
            deadX = false;
        }
        else
        {
            snapX = false;
            deadX = false;
        }

    }

    private void LateUpdate()
    {
        if (target == null) return;

        //Calc Dist covered and Update prevPos
        Vector2 temp = target.position;
        Vector2 distCovered = temp - prevPos;
        prevPos = temp;

        Vector3 targetPos = target.position;
        targetPos.z = distance;

        #region UpdateXPos
        if (!deadX && !snapX)//Smoothing in X
        {
            targetPos.x = Mathf.MoveTowards(transform.position.x, targetPos.x, Time.deltaTime * followSpeed);
        }
        else if (deadX)//Dont move the camera in X
        {
            targetPos.x = transform.position.x;
        }
        else if(snapX)//No more smoothing just snap to player in X
        {
            float dir = target.position.x - transform.position.x;
            if (dir > 0f)
                dir = 1f;
            else
                dir = -1f;

            targetPos.x = transform.position.x + distCovered.x;
        }
        #endregion

        #region UpdateYPos
        float min = cam.orthographicSize * 2f * tempBot;
        float max = cam.orthographicSize * 2f * tempTop;

        float targetBot = target.position.y - target.GetComponent<SpriteRenderer>().bounds.extents.y;
        float targetTop = target.position.y + target.GetComponent<SpriteRenderer>().bounds.extents.y;

        if(targetBot <= currentY - cam.orthographicSize + min)//target is now below the min height
        {
            currentY -= cam.orthographicSize * 0.75f;
        }
        else if(targetTop >= currentY + cam.orthographicSize - max)//target is now above the max height
        {
            currentY += cam.orthographicSize * 0.75f;
        }
        targetPos.y = Mathf.MoveTowards(transform.position.y, currentY, Time.deltaTime * followSpeed);



        #endregion

        transform.position = targetPos;
    }

    private void OnDrawGizmosSelected()
    {
        Camera temp = GetComponent<Camera>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(deadZoneOffsetX, 0f, 0f), new Vector3(deadZoneX, temp.orthographicSize * 2f, 0f));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(snapZoneOffsetX, 0f, 0f), new Vector3(snapZoneX, temp.orthographicSize * 2f, 0f));

        //Border of camera
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(temp.aspect * temp.orthographicSize * 2f, temp.orthographicSize * 2f, 0f));

        float min = temp.orthographicSize * 2f * tempBot;
        float max = temp.orthographicSize * 2f * tempTop;
        
        //Top y 
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, temp.orthographicSize - max, 0f), 
            new Vector3(temp.aspect * temp.orthographicSize * 2f, 0f, 0f));

        //Bottom Y
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + new Vector3(0f, -temp.orthographicSize + min, 0f),
            new Vector3(temp.aspect * temp.orthographicSize * 2f, 0f, 0f));


    }
}
