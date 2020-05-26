using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    private bool gone = false;
    protected bool willRespawn = true;


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

    protected virtual void TriggerRespawnEvent()
    {
        if (!willRespawn) return;

        gameObject.SetActive(true);
    }

    protected virtual void TriggerRespawnStatusEvent()
    {
        if (gone) willRespawn = false;
    }

    protected virtual void TriggerRespawnAllEvent()
    {
        willRespawn = true;
        gone = false;

        gameObject.SetActive(true);
    }

    protected void Gone()
    {
        gone = true;
        gameObject.SetActive(false);
    }
}
