using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * This object is created when the player starts a level.
 * 
 * It shows the missions that they have to complete to gain stars.
 */ 
public class MissionOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName = null;
    [SerializeField] private TextMeshProUGUI[] missions = new TextMeshProUGUI[3];

    private void Start()
    {
        levelName.text = "Level " + LevelManager.instance.CurrentLevelIndex + 1;

        Level currentLevel = LevelManager.instance.CurrentLevel;

        UpdateMissionText(0, currentLevel.numToCollect);
        UpdateMissionText(1, currentLevel.starTime);
        UpdateMissionText(2, currentLevel.numToKill);

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

    private void UpdateMissionText(int index, float numToSub)
    {
        Level currentLevel = LevelManager.instance.CurrentLevel;
        missions[index].text = "";

        foreach (string phrase in currentLevel.missions[index])
        {
            if (phrase.Contains(currentLevel.subWord))
                missions[index].text += numToSub;
            else
                missions[index].text += phrase;

            if (phrase != currentLevel.missions[index][currentLevel.missions.Count - 1])
                missions[index].text += " ";
        }
    }
}
