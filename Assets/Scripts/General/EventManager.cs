using System;
using System.Collections;
using UnityEngine;

/**
 * This class manages all the events that is created by the developer
 * 
 * How to use:
 * 
 * Templete: public event Action<T> testEvent = null;
 * 
 * if no variables needs to be passed in to the function attached, use Action instead of Action<T>
 * 
 * The gameobject that needs to be linked to that event add this:
 * 
 * EventManager.instance.testEvent -= functionToLink;
 * EventManager.instance.testEvent += functionToLink;
 * 
 * Always minus before adding to make sure that the manager does not have duplicates of the function
 * 
 * Remember to Minus in the OnDestroy Function of the gameobject to make sure that once the object is destroyed, the link is removed
 * 
 * Create a TriggerTestEvent Function in this class to be called whenever the event needs to be triggered
 */

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
    public event Action startSceneTransitionEvent = null;
    public event Action offSceneTransitionEvent = null;
    public event Action<bool> updateHUDEvent = null;
    public event Action<GameObject, COLORS> setPlatformEvent = null;
    public event Action resetPlatformsEvent = null;
    public event Action updatePaintingBorderEvent = null;

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

    public void TriggerSceneTransitionEvent()
    {
        if (startSceneTransitionEvent != null)
            startSceneTransitionEvent();
    }

    public void TriggerSceneTransitionOffEvent()
    {
        if (offSceneTransitionEvent != null)
            offSceneTransitionEvent();
    }

    public void TriggerUpdateHUDEvent(bool state)
    {
        if (updateHUDEvent != null)
            updateHUDEvent(state);
    }

    public void TriggerSetPlatform(GameObject gameObject, COLORS platformColor)
    {
        if (setPlatformEvent != null)
            setPlatformEvent(gameObject, platformColor);
    }

    public void TriggerResetPlatforms()
    {
        if (resetPlatformsEvent != null)
            resetPlatformsEvent();
    }

    public void TriggerUpdatePaintingBorderEvent()
    {
        if (updatePaintingBorderEvent != null)
            updatePaintingBorderEvent();
    }
}
