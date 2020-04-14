using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "YellowColor", menuName = "Colors/Yellow", order = 3)]
public class YellowColor : WhiteColor
{
    [SerializeField] private YellowProjectile projectile = null;
    [SerializeField] private float projectileSpeed = 5f;
    
    private PlayerColor playerColor = null;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);
        playerColor = player.GetComponent<PlayerColor>();
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
