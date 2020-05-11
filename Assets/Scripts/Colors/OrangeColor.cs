using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrangeColor", menuName = "Colors/Orange", order = 7)]
public class OrangeColor : BaseColor
{
    [SerializeField] private float abilityDuration = 3f;

    private PlayerColor playerColor = null;
    private PlayerInfo playerInfo = null;

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

        if(abilityInput.IsPressed && abilityCD <= 0f)
        {
            abilityCD = abilityInterval;

            abilityActivated = true;
            playerInfo.IsInvincible = true;

            abilityInput.GetComponentInChildren<CooldownIndicator>().StartCooldown(abilityInterval);
        }

        if (abilityActivated && !playerInfo.IsInvincible)
        {
            abilityActivated = false;
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

        GameObject collidedPlatform = playerColor.GetCollidedPlatform;

        if (collidedPlatform != null && collidedPlatform.GetComponent<DamagingPlatform>() != null)
        {
            collidedPlatform.GetComponent<DamagingPlatform>().IsDamaging = false;
        }
    }
}
