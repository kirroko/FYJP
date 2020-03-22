using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformVisibility : MonoBehaviour
{
    public PlatformInfo[] platforms = null;

    public PlayerInfo playerInfo = null;

    private void Start()
    {
        DisablePlatforms();

        playerInfo = GetComponent<PlayerInfo>();
    }

    public void UpdateVisibility()
    {
        DisablePlatforms();

        foreach (PlatformInfo platform in platforms)
        {
            foreach(PAINT_COLOURS color in platform.visibleColors)
            {
                if(color == playerInfo.colour)
                {
                    EnablePlatforms(platform);
                }
            }
        }

    }

    private void DisablePlatforms()
    {
        foreach (PlatformInfo platform in platforms)
        {
            for (int i = 0; i < platform.platformGroup.transform.childCount; ++i)
            {
                GameObject child = platform.platformGroup.transform.GetChild(i).gameObject;

                if (platform.visibleColors[0] == PAINT_COLOURS.PURPLE)
                {
                    if (child.GetComponent<Collider2D>() != null)
                        child.GetComponent<Collider2D>().isTrigger = false;

                    Color tempColor = child.GetComponent<SpriteRenderer>().color;
                    tempColor.a = 1f;
                    child.GetComponent<SpriteRenderer>().color = tempColor;
                }
                else
                {
                    if (child.GetComponent<Collider2D>() != null)
                        child.GetComponent<Collider2D>().isTrigger = true;

                    Color tempColor = child.GetComponent<SpriteRenderer>().color;
                    tempColor.a = 0.25f;
                    child.GetComponent<SpriteRenderer>().color = tempColor;
                }
            }
        }
    }

    private void EnablePlatforms(PlatformInfo platform)
    {
        for (int i = 0; i < platform.platformGroup.transform.childCount; ++i)
        {
            GameObject child = platform.platformGroup.transform.GetChild(i).gameObject;

            if(platform.visibleColors[0] == PAINT_COLOURS.PURPLE)
            {
                if (child.GetComponent<Collider2D>() != null)
                    child.GetComponent<Collider2D>().isTrigger = true;

                Color tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = 0.25f;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
            else
            {
                if (child.GetComponent<Collider2D>() != null)
                    child.GetComponent<Collider2D>().isTrigger = false;

                Color tempColor = child.GetComponent<SpriteRenderer>().color;
                tempColor.a = 1f;
                child.GetComponent<SpriteRenderer>().color = tempColor;
            }
        }
    }
}
