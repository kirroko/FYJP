﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private PlayerGhost ghost = null;

    private List<Vector3> recordedPos = new List<Vector3>();

    private Vector2 prevVel = Vector2.zero;

    private PlayerMovement movement = null;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (movement.GetJumpButton.tap && movement.OnGround ||//Player Press Jump and is on ground
                        !movement.OnGround)
        {
            recordedPos.Add(transform.position);
        }

        prevVel = GetComponent<Rigidbody2D>().velocity;
    }

    public void ShowGhost()
    {
        //spawn ghost
        PlayerGhost tempGhost = Instantiate(ghost, recordedPos[0], Quaternion.identity);
        tempGhost.Init(recordedPos);
    }
}