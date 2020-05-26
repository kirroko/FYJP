using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script is used on background gameobject
 * they have to be a child of the camera
 */
public class ParallaxScrolling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cam = null;

    [Header("Settings")]
    [SerializeField] private bool willRepeat = true;
    [Tooltip("The lower the value the Faster it moves")]
    [Range(0f,1f)]
    [SerializeField] private float scrollSpeed = 1f;//Lower the value faster it moves in game 
    [SerializeField] private bool spawnDup = false;///< Set to true if it should spawn duplicates
    [SerializeField] private bool tileVertical = false;///< Set to true if it should also tile vertically

    private Vector3 startPos = Vector3.zero;
    private Vector2 bounds = Vector2.zero;
    private float startY = 0f;

    private void Start()
    {
        //Init Variables
        startPos = transform.position;
        bounds = GetComponent<SpriteRenderer>().bounds.size;
        startY = startPos.y;

        //Spawn Duplicate
        if (!spawnDup) return;

        if (tileVertical)
        {
            Vector3 duplicatePosUp = transform.position;
            duplicatePosUp.y += bounds.y;

            ParallaxScrolling dupUp = Instantiate(this, duplicatePosUp, Quaternion.identity);
            dupUp.spawnDup = false;

            duplicatePosUp.y += bounds.y;
            ParallaxScrolling dupUp2 = Instantiate(this, duplicatePosUp, Quaternion.identity);
            dupUp2.spawnDup = false;

            dupUp.transform.SetParent(transform);
            dupUp2.transform.SetParent(transform);
        }

        //Right side
        Vector3 duplicatePos = transform.position;
        duplicatePos.x += bounds.x;

        ParallaxScrolling dupRight = Instantiate(this, duplicatePos, Quaternion.identity);
        dupRight.spawnDup = false;

        //Left side
        duplicatePos.x -= bounds.x * 2f;
        ParallaxScrolling dupLeft = Instantiate(dupRight, duplicatePos, Quaternion.identity);
        dupLeft.spawnDup = false;

        dupLeft.transform.SetParent(transform);
        dupRight.transform.SetParent(transform);
    }

    private void Update()
    {
        Vector3 targetPos = startPos;

        float distToMove = cam.position.x * scrollSpeed;
        targetPos.x += distToMove;

        if(willRepeat)
        {
            //Tilling
            float dist = cam.position.x - targetPos.x;
            if (dist > bounds.x * 1.5f)
                startPos.x += bounds.x * 3f;
            else if (dist < bounds.x * -1.5f)
                startPos.x -= bounds.x * 3f;
        }


        if(tileVertical && willRepeat)
        {
            //Tilling
            float distY = cam.position.y - targetPos.y;
            if (distY > bounds.y * 1.5f)
                startPos.y += bounds.y * 3f;
            else if (distY < bounds.y * -1.5f)
                startPos.y -= bounds.y * 3f;

            startPos.y = Mathf.Max(startPos.y, startY);
        }

        transform.position = targetPos;
    }
}
