using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    private LevelManager lvlManager = null;

    private void Awake()
    {
        lvlManager = LevelManager.instance;
    }

    public void Retry()
    {
        lvlManager.StartLevel(lvlManager.CurrentLevelIndex);
    }

    public void Next()
    {
        SceneTransition.instance.LoadSceneInBG("LevelSelection");
    }
}
