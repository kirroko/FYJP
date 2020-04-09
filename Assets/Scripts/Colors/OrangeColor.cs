using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrangeColor", menuName = "Colors/Orange", order = 7)]
public class OrangeColor : WhiteColor
{
    [SerializeField] private float abilityDuration = 3f;

    private PlayerColor playerColor = null;
    private PlayerInfo playerInfo = null;

    private float abilityLeft = 0f;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);

        abilityInput.HandleRange = 0f;
        playerColor = player.GetComponent<PlayerColor>();
        playerInfo = player.GetComponent<PlayerInfo>();
    }

    public override void UpdateAbility(GameObject player)
    {
        abilityCD -= Time.deltaTime;
        abilityLeft -= Time.deltaTime;

        if(abilityInput.IsPressed && abilityCD <= 0f)
        {
            abilityCD = abilityInterval;
            abilityLeft = abilityDuration;

            abilityActivated = true;
            playerInfo.isInvincible = true;
        }

        if(abilityActivated && abilityLeft <= 0f)
        {
            abilityActivated = false;
            playerInfo.isInvincible = false;
        }



        //Player Collided with a Damaging platform
        GameObject collidedPlatform = playerColor.GetCollidedPlatform;

        if (collidedPlatform != null && collidedPlatform.GetComponent<DamagingPlatform>() != null)
        {
            collidedPlatform.GetComponent<DamagingPlatform>().IsDamaging = true;
        }
    }

    public override void ExitAbility(GameObject player)
    {
        base.ExitAbility(player);

        abilityInput.HandleRange = 1f;
        abilityLeft = 0f;
        playerInfo.isInvincible = false;

        GameObject collidedPlatform = playerColor.GetCollidedPlatform;

        if (collidedPlatform != null && collidedPlatform.GetComponent<DamagingPlatform>() != null)
        {
            collidedPlatform.GetComponent<DamagingPlatform>().IsDamaging = false;
        }
    }
}
