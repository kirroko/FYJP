﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This platform will flip can stand on and cannot stand on
 * 
 * E.g 
 * 
 * Every 2 sec the platform group's standing status will change
 * 
 * t = 0s, platform 1 can stand, platform 2 cannot stand on
 * 
 * t = 2s, platform 1 cannot stand, platform 2 can stand
 * 
 * t = 4s, platform 1 can stand, platform 2 cannot stand on
 */
public class FlippingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] platformGroups = null;///< The Different Groups
    [SerializeField] private float flipInterval = 3f;///< How often does it flips

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
            EnablePlatforms();
        }
    }

    /**
    * This function will called when the platform cannot be stand on
    */
    private void DisablePlatforms()
    {
        foreach (GameObject platformGroup in platformGroups)
        {
            Collider2D[] colliders = platformGroup.GetComponentsInChildren<Collider2D>();

            if (colliders.Length != platformGroup.transform.childCount)
                Debug.LogWarning("Some of the children of " + platformGroup.name + " does not have a collider");

            foreach (Collider2D collider in colliders)
            {
                if (!collider.usedByEffector)
                    collider.isTrigger = true;


                if (collider.GetComponent<SpriteRenderer>() != null)
                {
                    Color tempColor = collider.GetComponent<SpriteRenderer>().color;
                    tempColor.a = 0.5f;
                    collider.GetComponent<SpriteRenderer>().color = tempColor;
                }
            }
        }
    }

    /**
    * This function will called when the platform can be stand on
    */
    private void EnablePlatforms()
    {
        Collider2D[] colliders = platformGroups[currentIndex].GetComponentsInChildren<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            if (!collider.usedByEffector)
                collider.isTrigger = false;

            if (collider.GetComponent<SpriteRenderer>() != null)
            {
                Color tempColor = collider.GetComponent<SpriteRenderer>().color;
                tempColor.a = 1f;
                collider.GetComponent<SpriteRenderer>().color = tempColor;
            }
        }
    }

}
