using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numCollected = null;
    [SerializeField] private TextMeshProUGUI timeTaken = null;
    [SerializeField] private TextMeshProUGUI enemiesKilled = null;

    private LevelManager lvlManager = null;

    private void Awake()
    {
        lvlManager = LevelManager.instance;
        ObjectReferences.instance.colorIndicator.gameObject.SetActive(false);
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
        lvlManager.StartLevel(lvlManager.CurrentLevelIndex);
    }

    public void Next()
    {
        SceneTransition.instance.LoadSceneInBG("LevelSelection");
    }
}
