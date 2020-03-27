using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YellowColor", menuName = "Colors/Yellow", order = 3)]
public class YellowColor : WhiteColor
{
    private PlayerColor playerColor = null;

    public override void UpdateAbility(GameObject player)
    {
        if (!once)
            DoOnce(player);

        GameObject collidedPlatform = playerColor.GetCollidedPlatform;
        if (collidedPlatform != null && collidedPlatform.GetComponent<MovingPlatform>() != null)
            collidedPlatform.GetComponent<MovingPlatform>().Charging = true;
    }

    private void DoOnce(GameObject player)
    {
        once = true;
        playerColor = player.GetComponent<PlayerColor>();
    }
}
