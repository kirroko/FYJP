using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D: MonoBehaviour
{
    [SerializeField] private Transform target = null;

    [SerializeField] private float distance = -10f;//z distance from player

    [SerializeField] private Vector2 followSpeed = new Vector2(3f, 3f);

    [SerializeField] private float deadZoneOffsetX = 0f;
    [SerializeField] private float deadZoneX = 0f;

    [SerializeField] private float lookaheadAmt = 20f;

    public float tempTop = 0.1f;
    public float tempBot = 0.15f;

    private float halfDeadZoneX = 0f;

    private bool deadX = false;

    //Target
    private Vector2 prevPos = Vector2.zero;

    //Camera
    private Camera cam = null;
    private float currentY = 0f;
    private bool ignoreDead = false;
    private float prevDir = 0f;
    
    private void Start()
    {
        cam = GetComponent<Camera>();

        Vector3 tempPos = transform.position;
        tempPos.z = distance;
        tempPos.y = currentY;

        halfDeadZoneX = deadZoneX * 0.5f;
        if(target != null)
        {
            prevPos = target.position;
            tempPos.x = target.position.x;
        }

        transform.position = tempPos;

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
            }
            return;
        }

    }

    private void LateUpdate()
    {
        if (target == null) return;

        //Calc Dist player covered and Update prevPos
        Vector2 temp = target.position;
        Vector2 distCovered = temp - prevPos;
        prevPos = temp;

        //Determine if camera should smoothly follow, snap or do nothing
        if (!ignoreDead && target.position.x < transform.position.x + deadZoneOffsetX + halfDeadZoneX &&
            target.position.x > transform.position.x + deadZoneOffsetX - halfDeadZoneX)
        {
            deadX = true;
        }
        else
        {
            ignoreDead = true;
            deadX = false;
        }

        if (ignoreDead)
        {
            if (prevDir != Sign(distCovered.x))
            {
                ignoreDead = false;
            }
        }


        Vector3 targetPos = target.position;
        targetPos.z = distance;

        #region UpdateXPos

        if (!deadX)//Smoothing in X
        {
            prevDir = Sign(distCovered.x);
            targetPos.x += Sign(distCovered.x) * lookaheadAmt;//For lookahead
            targetPos.x = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * followSpeed.x);
        }
        else//Dont move the camera in X
        {
            targetPos.x = transform.position.x;
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
        targetPos.y = Mathf.MoveTowards(transform.position.y, currentY, Time.deltaTime * followSpeed.y);
        #endregion

        transform.position = targetPos;
    }

    private void OnDrawGizmosSelected()
    {
        Camera temp = GetComponent<Camera>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(deadZoneOffsetX, 0f, 0f), new Vector3(deadZoneX, temp.orthographicSize * 2f, 0f));

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

    private float Sign(float value)
    {
        if (value > 0f)
            return 1f;
        else if (value < 0f)
            return -1f;
        else return 0f;
    }
}
