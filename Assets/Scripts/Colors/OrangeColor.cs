using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrangeColor", menuName = "Colors/Orange", order = 7)]
public class OrangeColor : WhiteColor
{
    private PlayerColor playerColor = null;

    public override void UpdateAbility(GameObject player)
    {
        if (!once)
            DoOnce(player);

        //Player Collided with a Damaging platform
        GameObject collidedPlatform = playerColor.GetCollidedPlatform;

        if (collidedPlatform != null && collidedPlatform.GetComponent<DamagingPlatform>() != null)
        {
            collidedPlatform.GetComponent<DamagingPlatform>().IsDamaging = true;
        }
    }

    private void DoOnce(GameObject player)
    {
        once = true;
        playerColor = player.GetComponent<PlayerColor>();
    }
}
