using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName = null;
    [SerializeField] private TextMeshProUGUI[] missions = new TextMeshProUGUI[3];

    private void Start()
    {
        levelName.text = "Level " + LevelManager.instance.CurrentLevelIndex + 1;
        missions[0].text = LevelManager.instance.CurrentLevel.missions[0];
        missions[1].text = LevelManager.instance.CurrentLevel.missions[1];
        missions[2].text = LevelManager.instance.CurrentLevel.missions[2];

        ObjectReferences.instance.gameObject.SetActive(false);
    }

    public void StartLevel()
    {
        //Enable Canvas so player can move
        ObjectReferences.instance.gameObject.SetActive(true);
        //Disable MissionOverlay
        gameObject.SetActive(false);
        //Start Level
        LevelManager.instance.StartLevel();
    }
}
