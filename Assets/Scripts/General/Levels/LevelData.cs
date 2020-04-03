using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public bool unlocked = false;
    [HideInInspector] public int numStars = 0;//Updated each time player collects it 
    [HideInInspector] public float fastestTime = Mathf.Infinity;//Updated in EndLevel() of lvlmanager
    [HideInInspector] public List<Vec3Serializable> ghostPosSerialized = new List<Vec3Serializable>();//Updated when player finish the level faster than last time
}
