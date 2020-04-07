using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;

    public event Action<COLORS> PlatformColorEvent = null;

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

}
