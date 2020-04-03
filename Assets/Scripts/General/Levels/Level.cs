using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public GameObject layout = null;//Set in awake of lvlManager
    public string name = "";//Set in awake of lvlManager
    public LevelData data = null;//Updated in EndLevel() of lvlManager
    public List<Vector3> ghostPos = new List<Vector3>();//Updated when player finish the level faster than last time

    public void Print()
    {
        Debug.Log(name);
        Debug.Log("Fastest Time: " + data.fastestTime);
        Debug.Log("Stars Collected: " + data.numStars);
        Debug.Log("Num Ghost Pos: " + ghostPos.Count);
    }
}
