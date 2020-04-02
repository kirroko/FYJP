using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    private List<Level> levels = new List<Level>();
    private int numLevel = 0;

    private void Awake()
    {
        if (!instance)
            instance = this;
        DontDestroyOnLoad(this);

        object[] levelLayouts = Resources.LoadAll("Levels", typeof(GameObject));

        numLevel = levelLayouts.Length;
        
        for(int i = 0; i < numLevel; ++i)
        {
            Level temp = new Level();

            temp.layout = levelLayouts[i] as GameObject;
            temp.name = "Level" + (i + 1);

            //Check if the file data has be created before if not create it else load it
            LevelData levelData = SaveSystem.LoadLevel(temp.name);
            if (levelData == null)
            {
                temp.data = new LevelData();
                SaveSystem.SaveLevel(temp);
            }
            else
            {
                UpdateLevelData(temp, levelData);
            }
            levels.Add(temp);
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

    public IEnumerator LoadLevel(int index)
    {
        SceneTransition.instance.LoadSceneInBG("Level");

        while (!SceneManager.GetSceneByName("Level").isLoaded)
        {
            yield return null;
        }

        GameObject layout = Instantiate(levels[index - 1].layout);
    }
}
