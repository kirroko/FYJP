using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YellowColor", menuName = "Colors/Yellow", order = 3)]
public class YellowColor : BaseColor
{
    [SerializeField] private YellowProjectile projectile = null;
    [SerializeField] private float projectileSpeed = 5f;
    
    private PlayerColor playerColor = null;

    private MovingPlatform collidedPlatform = null;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);
        AddEvent();
        playerColor = player.GetComponent<PlayerColor>();
        collidedPlatform = null;

        EventManager.instance.setPlatformEvent -= SetPlatform;
        EventManager.instance.setPlatformEvent += SetPlatform;
    }

    public override void UpdateAbility(GameObject player)
    {
        base.UpdateAbility(player);

        if (!abilityInput.IsPressed && abilityActivated)
        {
            abilityActivated = false;

            if (dir.Equals(Vector2.zero)) return;

            Shoot(projectile, projectileSpeed, player);
        }

        //UPdate for moving platform
        if (collidedPlatform != null)
            collidedPlatform.GetComponent<MovingPlatform>().Charging = true;
    }

    public override void ExitAbility(GameObject player)
    {
        if (collidedPlatform != null )
            collidedPlatform.Charging = false;

        collidedPlatform = null;
    }

    private void SetPlatform(GameObject platform, COLORS platformColor)
    {
        if (platformColor != mainColor) return;

        if (platform == null)
        {
            if(collidedPlatform != null)
                collidedPlatform.Charging = false;
            collidedPlatform = null;
            return;
        }

        if(platform.GetComponent<MovingPlatform>() != null) collidedPlatform = platform.GetComponent<MovingPlatform>();
    }
}
