using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public Level CurrentLevel { get { return currentLevel; } }
    public int CurrentLevelIndex { get { return currentLevelIndex; } }
    public Vector3 SpawnPoint
    {
        get { return spawnPoint; }
        set
        {
            spawnPoint = value;
            numCollectedAtCP = currentLevel.numCollected;
            EventManager.instance.TriggerUpdateRespawnStatusEvent();
            CPTime = elapsedTime;
        }
    }

    [SerializeField] private PlayerGhost ghost = null;

    public static LevelManager instance = null;

    private List<Level> levels = new List<Level>();
    private Level currentLevel = null;
    private int currentLevelIndex = 0;

    private float elapsedTime = 0f;
    private float CPTime = 0f;
    private bool start = false;
    private GameObject player = null;

    private Vector3 spawnPoint = new Vector3(0f, 0f, Mathf.Infinity);
    private Vector3 initalPos = Vector3.zero;
    private int numCollectedAtCP = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        InitLevelData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            EndLevel(true);

        if (!start) return;

        elapsedTime += Time.deltaTime;
        ObjectReferences.instance.time.text = elapsedTime.ToString("F2");
    }
    
    private void InitLevelData()
    {
        SaveSystem.Init();
        //Get all the levels from resources
        object[] levelList = Resources.LoadAll("Levels", typeof(Level));

        for (int i = 0; i < levelList.Length; ++i)
        {
            Level level = levelList[i] as Level;
            level.Init();

            //Get number of item player has to collect each level to get a star
            Collectable[] collectables = level.layout.transform.GetComponentsInChildren<Collectable>();
            level.numToCollect = Mathf.RoundToInt(collectables.Length * 0.8f);

            //Check if the file data has be created before if not create it else load it
            LevelData levelData = SaveSystem.LoadLevel("Level " + level.levelNum);
            if (levelData == null)
            {
                level.data = new LevelData();
                if (level.levelNum == 1) level.data.unlocked = true;
                SaveSystem.SaveLevel(level);
            }
            else
            {
                UpdateLevelData(level, levelData);
            }
            levels.Add(level);
        }
    }

    private void UpdateLevelData(Level level, LevelData levelData)
    {
        level.data = levelData;
        
        foreach(Vec3Serializable pos in levelData.ghostPosSerialized)
        {
            level.ghostPos.Add(new Vector3(pos.x, pos.y, pos.z));
        }
    }

    private IEnumerator LoadLevel(int index)
    {
        //Reset some variables
        elapsedTime = 0f;
        currentLevelIndex = index;
        currentLevel = levels[index];
        currentLevel.numCollected = 0;
        numCollectedAtCP = 0;
        ObjectReferences.instance.itemCount.text = "0/" + currentLevel.numToCollect;

        SceneTransition.instance.LoadSceneInBG("Level");

        while (!SceneManager.GetSceneByName("Level").isLoaded)
        {
            yield return null;
        }

        GameObject layout = Instantiate(currentLevel.layout);

        yield return null;

        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = player.transform.position;
        initalPos = player.transform.position;

        if (currentLevel.colorList != null)
            PlayerManager.instance.UpdateColorList(currentLevel.colorList);
        else
            Debug.LogError("Level" + index + " Color list not set");

        //Spawn ghost if it exist
        //if(currentLevel.ghostPos.Count > 0)
        //{
        //    PlayerGhost tempGhost = Instantiate(ghost, currentLevel.ghostPos[0], Quaternion.identity);
        //    tempGhost.Init(currentLevel.ghostPos);
        //}
    }

    public void EndLevel(bool completed)
    {
        start = false;

        if(!completed)
        {
            elapsedTime = 0f;
            SceneTransition.instance.LoadSceneInBG("LevelSelection");
            return;
        }

        int stars = 0;

        //Updated Fastest Time & ghost
        if (elapsedTime < currentLevel.data.fastestTime)
        {
            currentLevel.data.fastestTime = elapsedTime;
            //Update GhostPos
            currentLevel.ghostPos = player.GetComponent<GhostManager>().RecordedPos;
            //Updated ghost data to be serialized
            foreach (Vector3 pos in currentLevel.ghostPos)
            {
                currentLevel.data.ghostPosSerialized.Add(Vec3Serializable.ToVec3Serializable(pos));
            }
        }

        //Collected all the collectables
        Debug.Log("Num collected: " + currentLevel.numCollected);
        if (currentLevel.numCollected >= currentLevel.numToCollect)
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
        CPTime = 0f;

        SceneTransition.instance.LoadSceneInBG("ResultScreen");
    }

    private IEnumerator ReloadLevel(Vector3 posToSpawn, bool respawnItems)
    {
        if(respawnItems)
        {
            currentLevel.numCollected = 0;
            EventManager.instance.TriggerRespawnAllEvent();
        }


        //Reset some variable
        elapsedTime = CPTime;
        currentLevel.numCollected = numCollectedAtCP;
        ObjectReferences.instance.itemCount.text = currentLevel.numCollected + "/" + currentLevel.numToCollect;

        //Set player's pos to checkpoint 
        player.transform.position = posToSpawn + new Vector3(0f, 5f, 0f);
        Camera.main.transform.position = player.transform.position;

        //Respawn Respawnable objects
        EventManager.instance.TriggerRespawnObjectsEvent();

        yield return null;

        start = true;
        
        //Spawn ghost if it exist
        //if(currentLevel.ghostPos.Count > 0)
        //{
        //    PlayerGhost tempGhost = Instantiate(ghost, currentLevel.ghostPos[0], Quaternion.identity);
        //    tempGhost.Init(currentLevel.ghostPos);
        //}
    }

    public void StartLevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    public void RestartFromCheckpoint()
    {
        StartCoroutine(ReloadLevel(spawnPoint, false));
    }

    public void RestartFromBeginning()
    {
        StartCoroutine(ReloadLevel(initalPos, true));
    }

    public void ClearSavedData()
    {
        SaveSystem.DeleteAllSaveData();
    }

    public void ResetLevelVariables()
    {
        start = false;
        currentLevel.numCollected = 0;
        elapsedTime = 0f;
    }

    public void StartLevel()
    {
        start = true;
    }
}
