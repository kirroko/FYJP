using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        EventManager.instance.startSceneTransitionEvent -= TriggerTransition;
        EventManager.instance.startSceneTransitionEvent += TriggerTransition;

        EventManager.instance.offSceneTransitionEvent -= TriggerTransitionOff;
        EventManager.instance.offSceneTransitionEvent += TriggerTransitionOff;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.instance.startSceneTransitionEvent -= TriggerTransition;
        EventManager.instance.offSceneTransitionEvent -= TriggerTransitionOff;
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
