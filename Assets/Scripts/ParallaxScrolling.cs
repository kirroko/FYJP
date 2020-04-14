using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cam = null;

    [Header("Settings")]
    [Tooltip("The lower the value the Faster it moves")]
    [Range(0f,1f)]
    [SerializeField] private float scrollSpeed = 1f;//Lower the value faster it moves in game 
    [SerializeField] private bool spawnDup = false;

    private Vector3 startPos = Vector3.zero;
    private float boundX = 0f;


    private void Start()
    {
        //Init Variables
        startPos = transform.position;
        boundX = GetComponent<SpriteRenderer>().bounds.size.x;

        //Spawn Duplicate
        if (!spawnDup) return;

        //Right side
        Vector3 duplicatePos = transform.position;
        duplicatePos.x += boundX;

        ParallaxScrolling dupRight = Instantiate(this, duplicatePos, Quaternion.identity);
        dupRight.spawnDup = false;

        //Left side
        duplicatePos.x -= boundX * 2f;
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

        float dist = cam.position.x - targetPos.x;
        if (dist > boundX * 1.5f)
            startPos.x += boundX * 3f;
        else if (dist < boundX * -1.5f)
            startPos.x -= boundX * 3f;

        transform.position = targetPos;
    }
}
