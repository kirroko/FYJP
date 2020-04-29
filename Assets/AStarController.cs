using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AStarController : AI
{
    private AIPath enemy;

  
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemy = gameObject.GetComponent<AIPath>();

        
    }
    protected override void Update()
    {
        base.Update();

        if (stun)
            enemy.canMove = false;
        else
            enemy.canMove = true;

        if (frozen)
            enemy.maxSpeed = moveSpeed * slowAmt;
        else
            enemy.maxSpeed = moveSpeed;
    }
  
}
