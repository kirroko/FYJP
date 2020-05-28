using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

/**
 * This class manages everything level related.
 * It is only created once in Mainmenu and is set to not destory on load.
 * 
 * Method to load a level.
 * 
 * Create a Prefab of the layout of the level.
 * 
 * Layout shld include: 
 * 
 * Camera, layout, player, background.
 */
public class LevelManager : MonoBehaviour
{
    public Level CurrentLevel { get { return currentLevel; } }
    public int CurrentLevelIndex { get { return currentLevelIndex; } }
    public bool GodModeOn { get { return godMode; } }

    /// Allows checkpoint to set the spawnpoint of the current level.
    /// It also updates the number of item collected, enemies killed and time taken upon reaching the checkpoint.
    public Vector3 SpawnPoint
    {
        get { return spawnPoint; }
        set
        {
            spawnPoint = value;
            numCollectedAtCP = currentLevel.numCollected;
            numKilledAtCP = currentLevel.enemiesKilled;
            EventManager.instance.TriggerUpdateRespawnStatusEvent();
            CPTime = elapsedTime;
        }
    }

    public static LevelManager instance = null;

    private List<Level> levels = new List<Level>();
    private Level currentLevel = null;
    private int currentLevelIndex = 0;

    private float elapsedTime = 0f;
    private float CPTime = 0f;///< Time passed upon reaching checkpoint
    private bool start = false;
    private GameObject player = null;

    private Vector3 spawnPoint = new Vector3(0f, 0f, Mathf.Infinity);
    private Vector3 initalPos = Vector3.zero;
    private int numCollectedAtCP = 0;///< Number of Item Collected upon reaching checkpoint
    private int numKilledAtCP = 0;///< Number of Enemies Killed upon reaching checkpoint

    private bool godMode = false;

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
    
    /**
     * This function is called once during Awake and when all data is cleared.
     * 
     * It will initalise all the data to do with the level.
     */
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

            int numToCollect = 0;

            foreach(Collectable collectable in collectables)
            {
                if (!collectable.IsExtra) ++numToCollect;
            }
            level.numToCollect = Mathf.RoundToInt(numToCollect * level.percentToCollect);

            //Get Number of enemies player has to kill each level to get a star
            AI[] enemies = level.layout.transform.GetComponentsInChildren<AI>();
            level.numToKill = Mathf.RoundToInt(enemies.Length * level.percentToKill);
            if (enemies.Length > 0 && level.numToKill == 0)
            {
                Debug.Log(level.numToKill);
                level.numToKill = 1;
            }

            //Check if the file data has be created before if not create it else load it
            LevelData levelData = SaveSystem.LoadLevel("Level " + level.levelNum);
            if (levelData == null)
            {
                level.data = new LevelData();
                if (level.levelNum == 1) level.data.unlocked = true;
                level.data.unlocked = true;
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
    }

    /**
     * What the function will do:
     * 
     * Reset Time, item collect, enemies killed of that level.
     * 
     * Load Level Scene.
     * 
     * Layout prefab is Instantiated once scene is loaded.
     * 
     * Color that player can used is updated.
     * 
     * Spawnpoint is Set.
     */
    private IEnumerator LoadLevel(int index)
    {
        //Update HUD
        EventManager.instance.TriggerUpdateHUDEvent(true);

        //Reset some variables
        elapsedTime = 0f;
        currentLevelIndex = index;
        currentLevel = levels[index];
        currentLevel.numCollected = 0;
        numCollectedAtCP = 0;
        currentLevel.enemiesKilled = 0;
        ObjectReferences.instance.itemCount.text = "0/" + currentLevel.numToCollect;
        ObjectReferences.instance.numKilled.text = "0/" + currentLevel.numToKill;

        SceneTransition.instance.LoadSceneInBG("Level");

        while (!SceneManager.GetSceneByName("Level").isLoaded)
        {
            yield return null;
        }

        GameObject layout = Instantiate(currentLevel.layout);

        yield return null;

        AudioManager.StopBGM("Level");
        switch(index)
        {
            case 0:
                AudioManager.PlayBGM("Level 1", true);
                break;
            case 1:
                AudioManager.PlayBGM("Level 2", true);
                break;
            case 2:
                AudioManager.PlayBGM("Level 3", true);
                break;
            default:
                break;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = player.transform.position;
        initalPos = player.transform.position;

        if (godMode)
            player.GetComponent<PlayerInfo>().GainHeart(9999);

        if (currentLevel.colorList != null)
            PlayerManager.instance.UpdateColorList(currentLevel.colorList);
        else
            Debug.LogWarning("Level" + index + " Color list not set");
    }

    /**
     * Function that is called by when player reaches endpoint of the level.
     * 
     * What the function will do:
     * 
     * Calculates the number of stars obtained.
     * 
     * Changes Scene to ResultScreen scene.
     * 
     */
    public void EndLevel(bool completed)
    {
        start = false;
        EventManager.instance.TriggerUpdateHUDEvent(false);

        if (!completed)
        {
            elapsedTime = 0f;
            SceneTransition.instance.LoadSceneInBG("LevelSelection");
            return;
        }

        int stars = 0;

        //Updated Fastest Time
        if (elapsedTime < currentLevel.data.fastestTime)
        {
            currentLevel.data.fastestTime = elapsedTime;
        }

        //Collected all the collectables
        if (currentLevel.numCollected >= currentLevel.numToCollect)
            ++stars;
        //Finished level within time limit
        if (elapsedTime <= currentLevel.starTime)
            ++stars;
        //Hit kill count
        if (currentLevel.enemiesKilled >= currentLevel.numToKill)
            ++stars;

        //Update NumStars
        currentLevel.currentRunStar = stars;
        if (currentLevel.currentRunStar > currentLevel.data.numStars)
            currentLevel.data.numStars = currentLevel.currentRunStar;


        //Update current level data
        SaveSystem.SaveLevel(currentLevel);

        //Current level completed is not the last level
        if (currentLevelIndex != levels.Count - 1)
        {
            //Unlock next level and update the data
            levels[currentLevelIndex + 1].data.unlocked = true;
            SaveSystem.SaveLevel(currentLevel);
            SaveSystem.SaveLevel(levels[currentLevelIndex + 1]);
        }
        elapsedTime = 0f;
        CPTime = 0f;

        switch(currentLevelIndex)
        {
            case 0:
                AudioManager.StopBGM("Level 1");
                break;
            case 1:
                AudioManager.StopBGM("Level 2");
                break;
            case 2:
                AudioManager.StopBGM("Level 3");
                break;
            default:
                Debug.LogWarning("LEVEL MANAGER.CS ENDLEVEL FUNCTION SWITCH CASE DEFAULT HAS BEEN TRIGGERED");
                break;
        }

        switch(stars)
        {
            case 1:
                AudioManager.PlaySFX("Star 1", false);
                break;
            case 2:
                AudioManager.PlaySFX("Star 2", false);
                break;
            case 3:
                AudioManager.PlaySFX("Star 3", false);
                break;
            default:
                break;
        }

        AudioManager.PlayBGM("Result", true);
        
        SceneTransition.instance.LoadSceneInBG("ResultScreen");
    }


    /**
     * What the function will do:
     * 
     * Set time taken to time when player reached checkpoint.
     * 
     * Set num item collected to num collected when player reached checkpoint.
     * 
     * Set enemies killed to num killed when player reached checkpoint.
     * 
     * Set player's position to checkpoint's position or spawnpoint.
     * 
     * Respawn all items and enemies killed that is before hitting a checkpoint.
     */
    private IEnumerator ReloadLevel(Vector3 posToSpawn, bool respawnItems)
    {
        if(respawnItems)
        {
            currentLevel.numCollected = 0;
            currentLevel.enemiesKilled = 0;
            numCollectedAtCP = 0;
            numKilledAtCP = 0;
            EventManager.instance.TriggerRespawnAllEvent();
            EventManager.instance.TriggerCheckpointEvent(null);
        }


        //Reset some variable
        elapsedTime = CPTime;
        currentLevel.numCollected = numCollectedAtCP;
        currentLevel.enemiesKilled = numKilledAtCP;
        ObjectReferences.instance.itemCount.text = currentLevel.numCollected + "/" + currentLevel.numToCollect;
        ObjectReferences.instance.numKilled.text = currentLevel.enemiesKilled + "/" + currentLevel.numToKill;

        //Set player's pos to checkpoint
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        player.transform.position = posToSpawn - new Vector3(0f, playerCollider.bounds.extents.y, 0f);
        Camera.main.transform.position = player.transform.position;

        //Respawn Respawnable objects
        EventManager.instance.TriggerRespawnObjectsEvent();

        //Reset Platforms
        EventManager.instance.TriggerResetPlatforms();

        if (godMode)
            player.GetComponent<PlayerInfo>().GainHeart(9999);

        yield return null;

        start = true;
    }

    public void StartLevel(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    /// Called if player dies and respawn
    public void RestartFromCheckpoint()
    {
        StartCoroutine(ReloadLevel(spawnPoint, false));
    }

    /// Called if player press restart from pause menu
    public void RestartFromBeginning()
    {
        StartCoroutine(ReloadLevel(initalPos, true));
    }

    public void ClearSavedData()
    {
        SaveSystem.DeleteAllSaveData();
        InitLevelData();
        PlayerManager.instance.InitData();

        EventManager.instance.TriggerUpdatePaintingBorderEvent();
    }

    /// Called in result screen scene
    public void ResetLevelVariables()
    {
        start = false;
        currentLevel.numCollected = 0;
        elapsedTime = 0f;
        currentLevel.enemiesKilled = 0;
    }

    public void StartLevel()
    {
        start = true;
    }

    public void ToggleGodMode()
    {
        godMode = !godMode;
    }
}
