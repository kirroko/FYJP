﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float minAtkDrag = 10f;
    [SerializeField] private PlayerInfo player = null;

    private bool kill = false;
    private bool triggered = false;

    private void Update()
    {
        if (triggered && (Gesture.lastSwipe.x >= minAtkDrag || Gesture.lastSwipe.x <= -minAtkDrag) &&
            Gesture.lastSwipe.y > -100f && Gesture.lastSwipe.y < 100f)
        {
            kill = true;
        }
    }

    private void LateUpdate()
    {
        triggered = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        AI enemy = collision.gameObject.GetComponent<AI>();
        triggered = true;
        if(enemy != null)
        {
            if (kill)
            {
                enemy.TakeDamage(player.damage);
                kill = false;
            }
        }
    }
}
