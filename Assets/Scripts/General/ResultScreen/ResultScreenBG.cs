using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreenBG : MonoBehaviour
{
    [SerializeField] private Image stars = null;
    [SerializeField] private Image border = null;

    [SerializeField] private Sprite[] starsImages = null;
    [SerializeField] private Sprite[] borderImages = null;

    private LevelManager lvlManager = null;

    private void Awake()
    {
        lvlManager = LevelManager.instance;

        stars.sprite = starsImages[lvlManager.CurrentLevel.currentRunStar];
        border.sprite = borderImages[Mathf.Clamp(lvlManager.CurrentLevel.currentRunStar - 1, 0, 2)];
    }
}
