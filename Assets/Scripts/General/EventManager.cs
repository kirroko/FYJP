using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;

    public event Action<COLORS> PlatformColorEvent = null;
    public event Action<Collision2D, GameObject> EnemyCollisionEvent = null;
    public event Action checkpointEvent = null;
    public event Action<BaseColor, Projectile, float, GameObject> shootProjectileEvent = null;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(instance);
    }

    public void TriggerPlatformColorEvent(COLORS playerColor)
    {
        if (PlatformColorEvent != null)
            PlatformColorEvent(playerColor);
    }

    public void TriggerEnemyCollisionEvent(Collision2D collision, GameObject player)
    {
        if (EnemyCollisionEvent != null)
            EnemyCollisionEvent(collision, player);
    }

    public void TriggerCheckpointEvent()
    {
        if (checkpointEvent != null)
            checkpointEvent();
    }

    public void TriggerShootProjectileEvent(BaseColor me, Projectile projectile, float projectileSpeed, GameObject player)
    {
        StartCoroutine(ITriggerShootProjectileEvent(me, projectile, projectileSpeed, player));
    }

    private IEnumerator ITriggerShootProjectileEvent(BaseColor me, Projectile projectile, float projectileSpeed, GameObject player)
    {
        yield return new WaitForSeconds(0.4f);
        if (shootProjectileEvent != null)
        {
            shootProjectileEvent(me, projectile, projectileSpeed, player);
        }
    }
}
