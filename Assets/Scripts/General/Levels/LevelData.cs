using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/**
 * This class is to be serialized by SaveSystem
 * 
 * and is updated when the player completes a level.
 */
public class LevelData
{
    [HideInInspector] public bool unlocked = false;
    [HideInInspector] public int numStars = 0;//Updated each time player collects it 
    [HideInInspector] public float fastestTime = Mathf.Infinity;//Updated in EndLevel() of lvlmanager
}
