using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

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
        //this.gameObject.SetActive(true);
        if (GameObject.FindGameObjectWithTag("Effect"))
            GameObject.FindGameObjectWithTag("Effect").SetActive(false);
    }

    private void TriggerTransitionOff()
    {
        //this.gameObject.SetActive(false);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }    
}
