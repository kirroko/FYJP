using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RedColor", menuName = "Colors/Red", order = 4)]
public class RedColor : WhiteColor
{
    [Header("Ability Related")]
    [SerializeField] private Projectile projectile = null;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float offsetPos = 2f;

    private float shootCD = 0f;

    private Joystick input = null;

    public override void InitAbility(GameObject player)
    {
        input = ObjectReferences.instance.joystick;
    }

    public override void UpdateAbility(GameObject player)
    {
        shootCD -= Time.deltaTime;

        if (Gesture.tap && shootCD <= 0f)
        {
            shootCD = shootInterval;

            Vector3 direction = Vector3.zero;

            if (input.Direction.x > 0.5f)
                direction.x = 1f;
            else if (input.Direction.x < -0.5f)
                direction.x = -1f;

            if (input.Direction.y > 0.5f)
                direction.y = 1f;
            else if (input.Direction.y < -0.5f)
                direction.y = -1f;

            if (direction == Vector3.zero)
            {
                direction.x = player.GetComponent<PlayerMovement>().GetLastXDir;
                if (direction.x == 0)
                    direction.x = 1f;
            }

            Bounds playerColliderBounds = player.GetComponent<Collider2D>().bounds;
            Vector3 firePoint = playerColliderBounds.center + new Vector3(playerColliderBounds.extents.x * direction.x, playerColliderBounds.extents.y * direction.y, 0f) * offsetPos;

            Projectile temp = Instantiate(projectile, firePoint, Quaternion.identity);
            temp.Init(direction, projectileSpeed);
        }
    }

}
