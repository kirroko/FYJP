using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public Level CurrentLevel { get { return currentLevel; } }
    public int CurrentLevelIndex { get { return currentLevelIndex; } }

    [SerializeField] private PlayerGhost ghost = null;

    public static LevelManager instance = null;

    private List<Level> levels = new List<Level>();
    private Level currentLevel = null;
    private int currentLevelIndex = 0;

    private float elapsedTime = 0f;
    private GameObject player = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        //Get all the levels from resources
        object[] levelList = Resources.LoadAll("Levels", typeof(Level));

        for(int i = 0; i < levelList.Length; ++i)
        {
            Level level = levelList[i] as Level;

            Collectable[] collectables = level.layout.transform.GetComponentsInChildren<Collectable>();
            level.numToCollect = collectables.Length;

            //Check if the file data has be created before if not create it else load it
            LevelData levelData = SaveSystem.LoadLevel(level.name);
            if (levelData == null)
            {
                level.data = new LevelData();
                SaveSystem.SaveLevel(level);
            }
            else
            {
                UpdateLevelData(level, levelData);
            }
            levels.Add(level);
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
            Debug.Log("Loading");
            yield return null;
        }

        Debug.Log("Lvl Loaded");
        //currentLevel.Print();
        GameObject layout = Instantiate(currentLevel.layout);
        //Spawn ghost if it exist
        //if(currentLevel.ghostPos.Count > 0)
        //{
        //    PlayerGhost tempGhost = Instantiate(ghost, currentLevel.ghostPos[0], Quaternion.identity);
        //    tempGhost.Init(currentLevel.ghostPos);
        //}
    }

    public void EndLevel()
    {
        int stars = 0;

        //Updated Fastest Time & ghost
        if (elapsedTime < currentLevel.data.fastestTime)
        {
            currentLevel.data.fastestTime = elapsedTime;
            //Update GhostPos
            player = GameObject.FindGameObjectWithTag("Player");
            currentLevel.ghostPos = player.GetComponent<GhostManager>().RecordedPos;
            //Updated ghost data to be serialized
            foreach (Vector3 pos in currentLevel.ghostPos)
            {
                currentLevel.data.ghostPosSerialized.Add(Vec3Serializable.ToVec3Serializable(pos));
            }
        }

        //Collected all the collectables
        if (currentLevel.collectablesCount == currentLevel.numToCollect)
            ++stars;
        //Finished level within time limit
        if (elapsedTime <= currentLevel.starTime)
            ++stars;
        //Died less thn maxDeath
        if (currentLevel.deathCount <= currentLevel.maxDeath)
            ++stars;

        //Update NumStars
        currentLevel.data.numStars = stars;


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

    private IEnumerator ReloadLevel()
    {
        elapsedTime = 0f;
        SceneTransition.instance.LoadScene("Level");

        yield return new WaitForSeconds(0.5f);

        //currentLevel.Print();
        GameObject layout = Instantiate(currentLevel.layout);
        //Spawn ghost if it exist
        //if(currentLevel.ghostPos.Count > 0)
        //{
        //    PlayerGhost tempGhost = Instantiate(ghost, currentLevel.ghostPos[0], Quaternion.identity);
        //    tempGhost.Init(currentLevel.ghostPos);
        //}
    }

    public void RestartLevel()
    {
        StartCoroutine(ReloadLevel());
    }
}
