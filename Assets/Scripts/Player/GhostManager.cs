using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{ 
    public List<Vector3> RecordedPos { get { return recordedPos; } }

    private List<Vector3> recordedPos = new List<Vector3>();

    private Vector2 prevVel = Vector2.zero;

    private PlayerMovement movement = null;
    private HoldButton jumpButton = null;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        jumpButton = ObjectReferences.instance.jumpButton;
    }

    private void Update()
    {
        if (jumpButton.tap && movement.OnGround ||//Player Press Jump and is on ground
                        !movement.OnGround)
        {
            recordedPos.Add(transform.position);
        }

        prevVel = GetComponent<Rigidbody2D>().velocity;
    }
}
