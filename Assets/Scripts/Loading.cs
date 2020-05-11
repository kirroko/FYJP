using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        EventManager.instance.startSceneTransition -= TriggerTransition;
        EventManager.instance.startSceneTransition += TriggerTransition;

        EventManager.instance.offSceneTransition -= TriggerTransitionOff;
        EventManager.instance.offSceneTransition += TriggerTransitionOff;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.instance.startSceneTransition -= TriggerTransition;
        EventManager.instance.offSceneTransition -= TriggerTransitionOff;
    }

    private void TriggerTransition()
    {
        this.gameObject.SetActive(true);
    }

    private void TriggerTransitionOff()
    {
        this.gameObject.SetActive(false);
    }    
}
