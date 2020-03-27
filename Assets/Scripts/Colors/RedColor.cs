using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RedColor", menuName = "Colors/Red", order = 4)]
public class RedColor : WhiteColor
{
    [Header("Ability Related")]
    [SerializeField] private Projectile projectile = null;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float shootInterval = 2f;

    private float shootCD = 0f;


    public override void UpdateAbility(GameObject player)
    {
        shootCD -= Time.deltaTime;
        
        if (Gesture.tap && shootCD <= 0f)
        {
            shootCD = shootInterval;

            Bounds playerColliderBounds = player.GetComponent<Collider2D>().bounds;
            Vector3 firePoint = playerColliderBounds.center + new Vector3(playerColliderBounds.extents.x, 0f, 0f);

            Projectile temp = Instantiate(projectile, firePoint, Quaternion.identity);
            temp.Init(Vector2.right);
        }
    }
}
