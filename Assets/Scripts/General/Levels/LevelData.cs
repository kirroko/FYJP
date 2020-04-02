using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public bool unlocked = false;
    public int numStars = 0;
    public float fastestTime = Mathf.Infinity;
    public List<Vec3Serializable> ghostPosSerialized = new List<Vec3Serializable>();
}
