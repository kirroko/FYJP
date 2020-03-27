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

    public override void UpdateAbility(GameObject player)
    {
        if (!once)
            DoOnce(player);

        if (Gesture.tap)
        {
            movement.enabled = !movement.enabled;
            player.transform.SetParent(null);
        }

        GameObject collidedPlatform = playerColor.GetCollidedPlatform;
        if (collidedPlatform == null || collidedPlatform.GetComponent<ControllablePlatform>() == null) return;

        if (movement.enabled) return;

        player.transform.SetParent(collidedPlatform.transform);
        collidedPlatform.transform.position += new Vector3(input.Direction.x, input.Direction.y, 0f) * moveSpeed * Time.deltaTime;
    }

    private void DoOnce(GameObject player)
    {
        once = true;
        playerColor = player.GetComponent<PlayerColor>();
        movement = player.GetComponent<PlayerMovement>();
        input = movement.GetInput;
    }
}
