using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    [Tooltip("Min Swipe needed for swipe to be true")]
    [SerializeField] private float minSwipe = 100f;

    [Tooltip("Max Swipe for other axis swipe")]
    [SerializeField] private float maxSwipe = 100f;

    [SerializeField] private float rotateAmt = 90f;

    private float cameraRotation = 0f;
    private TurnManager TM = null;

    private void Start()
    {
        cameraRotation = transform.rotation.y;
        TM = TurnManager.Instance;
    }


    private void Update()
    {
        float swipeAmt = 0f;
        if (HasSwipe('x', ref swipeAmt) && swipeAmt > 0f)
        {
            cameraRotation += rotateAmt;
            transform.rotation = Quaternion.Euler(0f, cameraRotation, 0f);
        }
        else if (HasSwipe('x', ref swipeAmt) && swipeAmt < 0f)
        {
            cameraRotation -= rotateAmt;
            transform.rotation = Quaternion.Euler(0f, cameraRotation, 0f);
        }
        else if (HasSwipe('y', ref swipeAmt) && swipeAmt > 0f)
        {
            Vector3 targetPos = transform.position;
            targetPos += Camera.main.transform.forward * TM.gridSize;
            transform.position = targetPos;
            TM.newTurn = true;
        }
        else if (HasSwipe('y', ref swipeAmt) && swipeAmt < 0f)
        {
            Vector3 targetPos = transform.position;
            targetPos -= Camera.main.transform.forward * TM.gridSize;
            transform.position = targetPos;
            TM.newTurn = true;
        }

        //PC
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Vector3 targetPos = transform.position;
        //    targetPos += Camera.main.transform.forward * TM.gridSize;
        //    transform.position = targetPos;
        //    TM.newTurn = true;
        //}
        //else if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Vector3 targetPos = transform.position;
        //    targetPos -= Camera.main.transform.forward * TM.gridSize;
        //    transform.position = targetPos;
        //    TM.newTurn = true;
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    cameraRotation += rotateAmt;
        //    transform.rotation = Quaternion.Euler(0f, cameraRotation, 0f);
        //}
        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    cameraRotation -= rotateAmt;
        //    transform.rotation = Quaternion.Euler(0f, cameraRotation, 0f);
        //}
    }

    private bool HasSwipe(char axis, ref float swipeAmt)
    {
        Vector2 lastSwipe = Gesture.lastSwipe;

        if (axis == 'x')
        {
            if((lastSwipe.x > minSwipe || lastSwipe.x < -minSwipe) && lastSwipe.y > -maxSwipe && lastSwipe.y < maxSwipe)
            {
                swipeAmt = lastSwipe.x;
                return true;
            }
        }
        else
        {
            if ((lastSwipe.y > minSwipe || lastSwipe.y < -minSwipe) && lastSwipe.x > -maxSwipe && lastSwipe.x < maxSwipe)
            {
                swipeAmt = lastSwipe.y;
                return true;
            }
        }

        return false;
    }
}
