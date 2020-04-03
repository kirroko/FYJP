using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GreenColor", menuName = "Colors/Green", order = 8)]
public class GreenColor : WhiteColor
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerColor playerColor = null;
    private PlayerMovement movement = null;
    private Joystick input = null;

    public override void InitAbility(GameObject player)
    {
        playerColor = player.GetComponent<PlayerColor>();
        movement = player.GetComponent<PlayerMovement>();
        input = ObjectReferences.instance.joystick;
    }

    public override void UpdateAbility(GameObject player)
    {
        //Check if the player is on a controllable platform
        GameObject collidedPlatform = playerColor.GetCollidedPlatform;
        if (collidedPlatform == null || collidedPlatform.GetComponent<ControllablePlatform>() == null) return;

        //Toggle between moving the platform or player
        if (Gesture.tap)
        {
            movement.enabled = !movement.enabled;
            player.transform.SetParent(null);
        }

        if (movement.enabled) return;

        player.transform.SetParent(collidedPlatform.transform);
        collidedPlatform.transform.position += new Vector3(input.Direction.x, input.Direction.y, 0f) * moveSpeed * Time.deltaTime;
    }

    public override void ExitAbility(GameObject player)
    {
        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
