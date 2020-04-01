using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] platformGroups = null;
    [SerializeField] private float flipInterval = 3f;

    private int currentIndex = 0;
    private float flipTime = 0f;

    private void Start()
    {
        DisablePlatforms();
        platformGroups[currentIndex].SetActive(true);
        flipTime = flipInterval;
    }

    private void Update()
    {
        flipTime -= Time.deltaTime;

        if(flipTime <= 0f)
        {
            flipTime = flipInterval;
            ++currentIndex;

            if (currentIndex >= platformGroups.Length)
                currentIndex = 0;

            DisablePlatforms();
            platformGroups[currentIndex].SetActive(true);
        }
    }

    private void DisablePlatforms()
    {
        foreach (GameObject platformGroup in platformGroups)
        {
            platformGroup.SetActive(false);
        }
    }

}
