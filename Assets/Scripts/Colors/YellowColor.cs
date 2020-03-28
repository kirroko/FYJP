using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YellowColor", menuName = "Colors/Yellow", order = 3)]
public class YellowColor : WhiteColor
{
    private PlayerColor playerColor = null;

    public override void InitAbility(GameObject player)
    {
        playerColor = player.GetComponent<PlayerColor>();
    }

    public override void UpdateAbility(GameObject player)
    {
        GameObject collidedPlatform = playerColor.GetCollidedPlatform;
        if (collidedPlatform != null && collidedPlatform.GetComponent<MovingPlatform>() != null)
            collidedPlatform.GetComponent<MovingPlatform>().Charging = true;
    }

    public override void ExitAbility(GameObject player)
    {
        GameObject collidedPlatform = playerColor.GetCollidedPlatform;
        if (collidedPlatform != null && collidedPlatform.GetComponent<MovingPlatform>() != null)
            collidedPlatform.GetComponent<MovingPlatform>().Charging = false;
    }
}
