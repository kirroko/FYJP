using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public Level CurrentLevel { get { return currentLevel; } }

    [SerializeField] private PlayerGhost ghost = null;

    public static LevelManager instance = null;

    private List<Level> levels = new List<Level>();
    private Level currentLevel = null;
    private int currentLevelIndex = 0;

    private float elapsedTime = 0f;

    private void Awake()
    { 
        if (!instance)
            instance = this;
        DontDestroyOnLoad(this);

        //Get all the level layout from resources
        object[] levelLayouts = Resources.LoadAll("Levels", typeof(GameObject));
        
        for(int i = 0; i < levelLayouts.Length; ++i)
        {
            Level temp = new Level();

            temp.layout = levelLayouts[i] as GameObject;
            temp.name = "Level" + (i + 1);

            //Check if the file data has be created before if not create it else load it
            LevelData levelData = SaveSystem.LoadLevel(temp.name);
            if (levelData == null)
            {
                temp.data = new LevelData();
                if (i == 0)
                    temp.data.unlocked = true;
                SaveSystem.SaveLevel(temp);
            }
            else
            {
                UpdateLevelData(temp, levelData);
            }
            levels.Add(temp);
        }
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    private void UpdateLevelData(Level level, LevelData levelData)
    {
        level.data = levelData;
        
        foreach(Vec3Serializable pos in levelData.ghostPosSerialized)
        {
            level.ghostPos.Add(new Vector3(pos.x, pos.y, pos.z));
        }
    }

    public IEnumerator LoadLevel(int index)
    {
        elapsedTime = 0f;
        currentLevelIndex = index;
        currentLevel = levels[index];
        SceneTransition.instance.LoadSceneInBG("Level");

        while (!SceneManager.GetSceneByName("Level").isLoaded)
        {
            yield return null;
        }
        currentLevel.Print();
        GameObject layout = Instantiate(currentLevel.layout);
        //Spawn ghost if it exist
        if(currentLevel.ghostPos.Count > 0)
        {
            PlayerGhost tempGhost = Instantiate(ghost, currentLevel.ghostPos[0], Quaternion.identity);
            tempGhost.Init(currentLevel.ghostPos);
        }
    }

    public void EndLevel()
    {
        //Updated Fastest Time & ghost
        if (elapsedTime < currentLevel.data.fastestTime)
        {
            currentLevel.data.fastestTime = elapsedTime;
            //Update GhostPos
            currentLevel.ghostPos = ObjectReferences.instance.player.GetComponent<GhostManager>().RecordedPos;
            //Updated ghost data to be serialized
            foreach (Vector3 pos in currentLevel.ghostPos)
            {
                currentLevel.data.ghostPosSerialized.Add(Vec3Serializable.ToVec3Serializable(pos));
            }
        }
        //Update current level data
        SaveSystem.SaveLevel(currentLevel);

        //Current level completed is not the last level
        if (currentLevelIndex != levels.Count - 1)
        {
            //Unlock next level and update the data
            levels[currentLevelIndex + 1].data.unlocked = true;
            SaveSystem.SaveLevel(currentLevel);
        }
        elapsedTime = 0f;

        SceneTransition.instance.LoadSceneInBG("LevelSelection");
    }
}
