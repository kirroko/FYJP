using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawnable : MonoBehaviour
{
    protected bool gone = false;
    private bool willRespawn = true;

    private Vector3 scale = Vector3.zero;

    protected virtual void Start()
    {
        scale = transform.localScale;
        EventManager.instance.respawnObjectsEvent -= TriggerRespawnEvent;
        EventManager.instance.respawnObjectsEvent += TriggerRespawnEvent;

        EventManager.instance.updateRespawnStatusEvent -= TriggerRespawnStatusEvent;
        EventManager.instance.updateRespawnStatusEvent += TriggerRespawnStatusEvent;
    }

    protected virtual void OnDestroy()
    {
        EventManager.instance.respawnObjectsEvent -= TriggerRespawnEvent;
        EventManager.instance.updateRespawnStatusEvent -= TriggerRespawnStatusEvent;
    }

    private void TriggerRespawnEvent()
    {
        if (!willRespawn) return;

        transform.localScale = scale;
    }

    private void TriggerRespawnStatusEvent()
    {
        if (gone) willRespawn = false;
    }

    protected void Gone()
    {
        gone = true;
        transform.localScale = Vector3.zero;
    }
}
