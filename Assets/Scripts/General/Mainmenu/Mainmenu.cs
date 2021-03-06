﻿using UnityEngine;

public class Mainmenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject settings = null;

    [Header("Others")]
    [SerializeField] private string sceneName = "";

    private SceneTransition transition = null;

    private void Start()
    {
        transition = SceneTransition.instance;
        AudioManager.PlayBGM("MainMenu", true);
    }

    public void StartGame()
    {
        transition.LoadSceneInBG(sceneName);
        AudioManager.PlaySFX("Click",false);
        // AudioManager.PlayBGM("Level", true);
    }

    public void ToSettings()
    {
        settings.SetActive(true);
        gameObject.SetActive(false);
        AudioManager.PlaySFX("Click", false);
    }
}
