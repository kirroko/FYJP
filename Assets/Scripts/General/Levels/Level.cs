using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 5)]
public class Level : ScriptableObject
{   
    public GameObject layout = null;
    public int levelNum = 0;
    public LevelData data = null;//Updated in EndLevel() of lvlManager
    public ColorManager colorList = null;
    [HideInInspector] public List<Vector3> ghostPos = new List<Vector3>();//Updated when player finish the level faster than last time
    [HideInInspector] public int numCollected = 0;
    [HideInInspector] public int deathCount = 0;

    [Header("Criteria For Star")]
    [HideInInspector] public int numToCollect = 0;
    public float starTime = 0f;
    public int maxDeath = 0;

    [SerializeField] private List<string> mission1 = new List<string>();
    [SerializeField] private List<string> mission2 = new List<string>();
    [SerializeField] private List<string> mission3 = new List<string>();

    public List<List<string>> missions = new List<List<string>>();
    [Tooltip("The word that will be substituted as a number")]
    public string subWord = "num";

    public void Init()
    {
        missions.Add(mission1);
        missions.Add(mission2);
        missions.Add(mission3);
    }

    public void Print()
    {
        Debug.Log(name);
        Debug.Log("Fastest Time: " + data.fastestTime);
        Debug.Log("Stars Collected: " + data.numStars);
        Debug.Log("Num Ghost Pos: " + ghostPos.Count);
    }
}
