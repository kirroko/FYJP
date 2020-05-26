using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This Class will handle whether or not the object should respawn in the game
 * 
 * Any Objects that should respawn when the player respawn should inherit from this class
 */

public class Respawnable : MonoBehaviour
{
    private bool gone = false;
    protected bool willRespawn = true;///< Used to determine whether the object should respawn


    protected virtual void Start()
    {
        EventManager.instance.respawnObjectsEvent -= TriggerRespawnEvent;
        EventManager.instance.respawnObjectsEvent += TriggerRespawnEvent;

        EventManager.instance.updateRespawnStatusEvent -= TriggerRespawnStatusEvent;
        EventManager.instance.updateRespawnStatusEvent += TriggerRespawnStatusEvent;

        EventManager.instance.respawnAllEvent -= TriggerRespawnAllEvent;
        EventManager.instance.respawnAllEvent += TriggerRespawnAllEvent;
    }

    protected virtual void OnDestroy()
    {
        EventManager.instance.respawnObjectsEvent -= TriggerRespawnEvent;
        EventManager.instance.updateRespawnStatusEvent -= TriggerRespawnStatusEvent;
        EventManager.instance.respawnAllEvent -= TriggerRespawnAllEvent;
    }

    /**
     * This function is called when the player respawn.
     * 
     * It will check if the object needs to be respawned and act accordingly
     * 
     * Override this function as needed
     */
    protected virtual void TriggerRespawnEvent()
    {
        if (!willRespawn) return;

        gameObject.SetActive(true);
    }

    /**
     * This function updates whether or not the object should respawn.
     * 
     * Override it as needed
     */
    protected virtual void TriggerRespawnStatusEvent()
    {
        if (gone) willRespawn = false;
    }

    /**
    * This function is called when the player presses the restart button
    * 
    * It will respawn the object regardless of the status
    * 
    * Override it as needed
    */
    protected virtual void TriggerRespawnAllEvent()
    {
        willRespawn = true;
        gone = false;

        gameObject.SetActive(true);
    }

    /**
     * This function should be called when an object can respawn but should no longer respawn
     */
    protected void Gone()
    {
        gone = true;
        gameObject.SetActive(false);
    }
}
