using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

/**
 * This class handles all loading transition
 * 
 * it's reliant to eventmanager class.
 * 
 * Anything related to loading can be written here with callback.
 */
public class Loading : MonoBehaviour
{
    private void Start()
    {
        EventManager.instance.startSceneTransitionEvent -= TriggerTransition;
        EventManager.instance.startSceneTransitionEvent += TriggerTransition;

        EventManager.instance.offSceneTransitionEvent -= TriggerTransitionOff;
        EventManager.instance.offSceneTransitionEvent += TriggerTransitionOff;

        //gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.instance.startSceneTransitionEvent -= TriggerTransition;
        EventManager.instance.offSceneTransitionEvent -= TriggerTransitionOff;
    }

    private void TriggerTransition()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        if (GameObject.FindGameObjectWithTag("Effect"))
            GameObject.FindGameObjectWithTag("Effect").SetActive(false);

        AudioManager.StopBGM("MainMenu");
    }

    private void TriggerTransitionOff()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }    
}
