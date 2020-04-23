using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RedColor", menuName = "Colors/Red", order = 4)]
public class RedColor : BaseColor
{
    [SerializeField] private RedProjectile projectile = null;
    [SerializeField] private float projectileSpeed = 5f;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);

        EventManager.instance.EnemyCollisionEvent -= EnemyCollisionEvent;
        EventManager.instance.EnemyCollisionEvent += EnemyCollisionEvent;

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
    }

    public override void OnPlayerDestroyed()
    {
        EventManager.instance.EnemyCollisionEvent -= EnemyCollisionEvent;
    }

    private void EnemyCollisionEvent(Collision2D collision, GameObject player)
    {
        if (player.GetComponent<PlayerColor>().GetCurrentColor.GetMain != mainColor) return;

        AI enemy = collision.gameObject.GetComponent<AI>();

        if (enemy == null) return;

        if (player.GetComponent<PlayerMovement>().StillDashing && enemy.IsTagged)
        {
            Physics2D.IgnoreCollision(collision.collider, player.GetComponent<Collider2D>());//Ignores collision so player can go thru
            player.GetComponent<Rigidbody2D>().velocity = -collision.relativeVelocity;//Gives player vel before they collide
            player.GetComponent<PlayerMovement>().ResetDash();

            Destroy(collision.gameObject);
        }
    }
}
