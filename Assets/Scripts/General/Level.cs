using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public GameObject layout = null;
    public string name = "";
    public LevelData data = null;
    public List<Vector3> ghostPos = new List<Vector3>();
}
