using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * This class updates the resultscreen with the correct details such as 
 * item collected, time taken and enemies killed
 */

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numCollected = null;
    [SerializeField] private TextMeshProUGUI timeTaken = null;
    [SerializeField] private TextMeshProUGUI enemiesKilled = null;

    private LevelManager lvlManager = null;

    private void Awake()
    {
        lvlManager = LevelManager.instance;
        ObjectReferences.instance.gameObject.SetActive(false);
    }

    private void Start()
    {
        numCollected.text = ObjectReferences.instance.itemCount.text;
        timeTaken.text = ObjectReferences.instance.time.text;
        enemiesKilled.text = ObjectReferences.instance.numKilled.text;

        lvlManager.ResetLevelVariables();
    }

    public void Retry()
    {
        AudioManager.PlaySFX("Click", false);
        switch(lvlManager.CurrentLevelIndex)
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
                Debug.LogWarning("RESULTSCREEN.CS RETRY FUNCTION DEFAULT CASE HAS BEEN TRIGGERED");
                break;
        }

        ObjectReferences.instance.gameObject.SetActive(true);
        lvlManager.StartLevel(lvlManager.CurrentLevelIndex);
    }

    public void Next()
    {
        AudioManager.StopBGM("Result");
        AudioManager.PlaySFX("Click", false);
        AudioManager.PlayBGM("Level", true);
        ObjectReferences.instance.gameObject.SetActive(true);
        SceneTransition.instance.LoadSceneInBG("LevelSelection");
    }
}
