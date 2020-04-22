﻿using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private int LevelIndex = 0;
    [SerializeField] private float holdDuration = 1f;

    private float holdTime = 0f;
    private bool once = false;

    private Joystick abilityInput = null;

    private void Start()
    {
        holdTime = holdDuration;
        abilityInput = ObjectReferences.instance.abilityInput;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if ((abilityInput.IsPressed || Input.GetKey(KeyCode.E)) && !once)
            {
                holdTime -= Time.deltaTime;

                if(holdTime<= 0f)
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = ObjectReferences.fixedTimeScale;
                    LevelManager.instance.StartLevel(LevelIndex - 1);
                    once = true;
                }
            }
        }
    }
}
