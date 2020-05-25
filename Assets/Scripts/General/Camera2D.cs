using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D: MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Transform target = null;
    [SerializeField] private float distance = -10f;//z distance from player
    [SerializeField] private Vector2 followSpeed = new Vector2(3f, 3f);
    //[SerializeField] private Vector2 startOffset = Vector2.zero;

    [Header("Dead Zone")]
    [SerializeField] private float deadZoneOffsetX = 0f;
    [SerializeField] private float deadZoneX = 0f;

    [Header("Lookahead")]
    [SerializeField] private float lookaheadAmt = 20f;

    [Header("Follow Y")]
    [SerializeField] private float yMoveTop = 0.2f;//Percent from top
    [SerializeField] private float yMoveBottom = 0.2f;
    [SerializeField] private float moveAmt = 0.5f;//Percentage of the screen height

    [Header("Bounderies")]
    [SerializeField] private Transform boundsLeft = null;
    [SerializeField] private Transform boundsRight = null;
    [SerializeField] private Transform boundsTop= null;
    [SerializeField] private Transform boundsBot = null;

    private float halfDeadZoneX = 0f;

    private bool deadX = false;

    //Target
    private Joystick moveJoysick = null;

    //Camera
    private Camera cam = null;
    private float currentY = 0f;
    private bool ignoreDead = false;
    private float prevDir = 0f;
    private Vector2 camHalfSize = Vector2.zero;

    [HideInInspector] public bool isControlled = false;

    private float moveDir = 0f;
    private float prevX = 0f;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
        moveJoysick = ObjectReferences.instance.movementInput;

        halfDeadZoneX = deadZoneX * 0.5f;
        camHalfSize = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);

        Init();
    }

    private void Update()
    {
        if(target == null)
        {
            GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");
            if (tempPlayer != null)
            {
                target = tempPlayer.transform;
                Init();
                return;
            }
        }

    }

    private void LateUpdate()
    {
        if (target == null || isControlled) return;

        float tempDist = target.position.x - prevX;
        moveDir = Sign(tempDist);
        if (Mathf.Approximately(target.position.x, prevX) || Mathf.Abs(tempDist) < 0.01f)
            moveDir = 0f;

        //Determine if camera should smoothly follow, snap or do nothing
        if (ignoreDead)
        {
            if (prevDir != Sign(moveJoysick.Direction.x))
            {
                ignoreDead = false;
            }
        }

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


        Vector3 targetPos = target.position;
        targetPos.z = distance;

        #region UpdateXPos
        prevDir = Sign(moveJoysick.Direction.x);

        if (!deadX)//Lookahead and have camera follow
        {
            targetPos.x += Sign(moveDir) * lookaheadAmt;//For lookahead
            targetPos.x = Mathf.Lerp(transform.position.x, targetPos.x, Time.deltaTime * followSpeed.x);
        }
        else//Dont move the camera in X
        {
            targetPos.x = transform.position.x;
        }

        #endregion

        #region UpdateYPos
        float min = cam.orthographicSize * 2f * yMoveBottom;
        float max = cam.orthographicSize * 2f * yMoveTop;

        float targetBot = target.position.y /*- target.GetComponent<SpriteRenderer>().bounds.extents.y*/;
        float targetTop = target.position.y /*+ target.GetComponent<SpriteRenderer>().bounds.extents.y*/;

        if(targetBot <= currentY - cam.orthographicSize + min)//target is now below the min height
        {
            currentY -= cam.orthographicSize * moveAmt;
        }
        else if(targetTop >= currentY + cam.orthographicSize - max)//target is now above the max height
        {
            currentY += cam.orthographicSize * moveAmt;
        }
        targetPos.y = Mathf.MoveTowards(transform.position.y, currentY, Time.deltaTime * followSpeed.y);
        #endregion

        LimitCameraBounds(ref targetPos);

        transform.position = targetPos;
        prevX = target.position.x;
    }

    private void OnDrawGizmosSelected()
    {
        Camera temp = GetComponent<Camera>();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(deadZoneOffsetX, 0f, 0f), new Vector3(deadZoneX, temp.orthographicSize * 2f, 0f));

        //Border of camera
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(temp.aspect * temp.orthographicSize * 2f, temp.orthographicSize * 2f, 0f));

        float min = temp.orthographicSize * 2f * yMoveBottom;
        float max = temp.orthographicSize * 2f * yMoveTop;
        
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

    private void Init()
    {
        Vector3 tempPos = transform.position;
        tempPos.z = distance;

        transform.position = tempPos;
        currentY = transform.position.y;

        if(target != null)
            prevX = target.position.x;
    }

    public void LimitCameraBounds(ref Vector3 targetPos)
    {
        if (targetPos.x - camHalfSize.x <= boundsLeft.position.x)
            targetPos.x = boundsLeft.position.x + camHalfSize.x;
        if (targetPos.x + camHalfSize.x >= boundsRight.position.x)
            targetPos.x = boundsRight.position.x - camHalfSize.x;

        if (targetPos.y - camHalfSize.y <= boundsBot.position.y)
            targetPos.y = boundsBot.position.y + camHalfSize.y;
        if (targetPos.y + camHalfSize.y >= boundsTop.position.y)
            targetPos.y = boundsTop.position.y - camHalfSize.y;
    }
}
