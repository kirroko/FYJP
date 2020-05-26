using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrangeColor", menuName = "Colors/Orange", order = 7)]
public class OrangeColor : BaseColor
{
    private PlayerColor playerColor = null;
    private PlayerInfo playerInfo = null;

    private DamagingPlatform collidedPlatform = null;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);

        abilityInput.HandleRange = 0f;
        playerColor = player.GetComponent<PlayerColor>();
        playerInfo = player.GetComponent<PlayerInfo>();

        collidedPlatform = null;

        EventManager.instance.setPlatformEvent -= SetPlatform;
        EventManager.instance.setPlatformEvent += SetPlatform;
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
        if (collidedPlatform != null)
        {
            collidedPlatform.IsDamaging = true;
        }
    }

    public override void ExitAbility(GameObject player)
    {
        base.ExitAbility(player);

        abilityInput.HandleRange = 1f;

        if (collidedPlatform != null)
        {
            collidedPlatform.IsDamaging = false;
        }

        collidedPlatform = null;
    }

    private void SetPlatform(GameObject platform, COLORS platformColor)
    {
        if (platformColor != mainColor) return;

        if (platform == null)
        {
            if (collidedPlatform != null)
                collidedPlatform.IsDamaging = false;

            collidedPlatform = null;
            return;
        }

        if (platform.GetComponent<DamagingPlatform>() != null) collidedPlatform = platform.GetComponent<DamagingPlatform>();
    }
}
