using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level", order = 5)]

/*
 * Create this Object for every level.
 */
public class Level : ScriptableObject
{   
    public GameObject layout = null;
    public int levelNum = 0;
    public LevelData data = null;//Updated in EndLevel() of lvlManager ///< this is data that will be serialized by the save system
    public ColorManager colorList = null;///< The Colors the player can use
    [HideInInspector] public int numCollected = 0;
    [HideInInspector] public int enemiesKilled = 0;

    [Header("Criteria For Star")]
    public float starTime = 0f;///< Time player has to complete by to get a star
    [Range(0f, 1f)] public float percentToCollect = 0.8f;
    [Range(0f, 1f)] public float percentToKill = 0.5f;
    [HideInInspector] public int numToCollect = 0; ///< Calculated in LevelManager's InitLevelData()
    [HideInInspector] public int numToKill = 0;///< Calculated in LevelManager's InitLevelData()

    /**
     * Text that will be shown when the level start.
     * 
     * write num if you want to write the number in that pos
     * 
     * E.g: 
     * 
     * Text that i want to show: Collect 50 items.
     * 
     * mission1[0] = Collect .
     * 
     * mission1[1] = num.
     * 
     * mission1[2] =  items.
     */
    [SerializeField] private List<string> mission1 = new List<string>();///< Number of items player needs to collect for a Star
    [SerializeField] private List<string> mission2 = new List<string>();///< How fast should the player complete for a Star
    [SerializeField] private List<string> mission3 = new List<string>();///< Number of enemies player should kill for a Star

    public List<List<string>> missions = new List<List<string>>();
    [Tooltip("The word that will be substituted as a number")]
    public string subWord = "num";

    [HideInInspector] public int currentRunStar = 0;
    [HideInInspector] public Sprite fullSprite = null;

    public void Init()
    {
        missions.Clear();
        missions.Add(mission1);
        missions.Add(mission2);
        missions.Add(mission3);
    }

    public void Print()
    {
        Debug.Log(name);
        Debug.Log("Fastest Time: " + data.fastestTime);
        Debug.Log("Stars Collected: " + data.numStars);
    }
}
