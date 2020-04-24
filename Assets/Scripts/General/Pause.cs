using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject settings = null;

    private float prevTimeScale = 0f;
    private float prevFixedDelta = 0f;

    [HideInInspector] public bool isPaused = false;

    private void Start()
    {
        if (settings != null) settings.SetActive(false);
    }

    public void Pausing()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            prevTimeScale = Time.timeScale;
            prevFixedDelta = Time.fixedDeltaTime;

            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
            settings.SetActive(true);
        }
        else
        {
            Time.timeScale = prevTimeScale;
            Time.fixedDeltaTime = prevFixedDelta;
            settings.SetActive(false);
        }
    }
}
