using System;
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;

    public event Action<COLORS> PlatformColorEvent = null;
    public event Action<Collision2D, GameObject> EnemyCollisionEvent = null;
    public event Action<GameObject> checkpointEvent = null;
    public event Action<BaseColor, Projectile, float, GameObject> shootProjectileEvent = null;
    public event Action resetJoystickEvent = null;
    public event Action respawnObjectsEvent = null;
    public event Action updateRespawnStatusEvent = null;
    public event Action respawnAllEvent = null;
    public event Action startSceneTransition = null;
    public event Action offSceneTransition = null;

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

    public void TriggerCheckpointEvent(GameObject me)
    {
        if (checkpointEvent != null)
            checkpointEvent(me);
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

    public void TriggerResetJoystickEvent()
    {
        if (resetJoystickEvent != null)
            resetJoystickEvent();
    }

    public void TriggerRespawnObjectsEvent()
    {
        if (respawnObjectsEvent != null)
            respawnObjectsEvent();
    }

    public void TriggerUpdateRespawnStatusEvent()
    {
        if (updateRespawnStatusEvent != null)
            updateRespawnStatusEvent();
    }

    public void TriggerRespawnAllEvent()
    {
        if (respawnAllEvent != null)
            respawnAllEvent();
    }

    public void TriggerSceneTransition()
    {
        if (startSceneTransition != null)
            startSceneTransition();
    }

    public void TriggerSceneTransitionOff()
    {
        if (offSceneTransition != null)
            offSceneTransition();
    }
}
