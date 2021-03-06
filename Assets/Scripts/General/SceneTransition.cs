﻿using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

/**
 * This class handles the changing of scenes in the game.
 */

public class SceneTransition : MonoBehaviour
{
    public Scene PrevScene { get { return prevScene; } }
    public Scene CurrentScene { get { return currentScene; } }
    public bool ChangedScene { get { return changedScene; } }

    public static SceneTransition instance = null;

    //[Header("Reference")]
    //[SerializeField] private GameObject loadingScreen = null;

    private Scene prevScene;
    private Scene currentScene;
    private bool changedScene = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(this);
    }

    public void LoadSceneInBG(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(ILoadScene(sceneName));
    }

    private IEnumerator ILoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        changedScene = true;
        EventManager.instance.TriggerResetJoystickEvent();
        yield return null;
        changedScene = false;
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        EventManager.instance.TriggerSceneTransitionEvent();
        prevScene = SceneManager.GetActiveScene();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while(!operation.isDone)
        {
            yield return null;
        }

        EventManager.instance.TriggerSceneTransitionOffEvent();

        currentScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(currentScene);
        SceneManager.UnloadSceneAsync(prevScene);

        changedScene = true;
        EventManager.instance.TriggerResetJoystickEvent();
        yield return null;
        changedScene = false;
    }
}
