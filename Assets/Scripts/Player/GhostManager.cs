using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private PlayerGhost ghost = null;

    private List<List<Vector3>> ghostList = new List<List<Vector3>>();

    private Vector2 prevVel = Vector2.zero;
    private int currentIndex = -1;
    private bool hasJumped = false;

    private PlayerMovement movement = null;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(currentIndex >= 0)
        {
            //if (movement.GetJumpButton.tap && movement.OnGround ||//Player Press Jump and is on ground
            //    !movement.OnGround && prevVel.y > 0f && GetComponent<Rigidbody2D>().velocity.y < 0f ||//Player was going up now falling down
            //     prevVel.y <= 0f && movement.OnGround)
            //{
            //    ghostList[currentIndex].Add(transform.position);
            //}
            if (movement.GetJumpButton.tap && movement.OnGround ||//Player Press Jump and is on ground
                !movement.OnGround)
            {
                ghostList[currentIndex].Add(transform.position);
            }
        }

        prevVel = GetComponent<Rigidbody2D>().velocity;
        if (movement.GetJumpButton.tap && movement.OnGround)
            hasJumped = true;
        if (movement.OnGround)
            hasJumped = false;
    }

    public void NewGhost()
    {
        ++currentIndex;
        List<Vector3> temp = new List<Vector3>();
        temp.Add(transform.position);
        ghostList.Add(temp);
    }

    public void ShowGhost()
    {
        //spawn ghost
        foreach (List<Vector3> pos in ghostList)
        {
            PlayerGhost tempGhost = Instantiate(ghost, pos[0], Quaternion.identity);
            tempGhost.Init(pos);
        }
    }
}
