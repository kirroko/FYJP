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

        stars.sprite = starsImages[lvlManager.CurrentLevel.data.numStars];
        border.sprite = borderImages[Mathf.Clamp(lvlManager.CurrentLevel.data.numStars - 1, 0, 2)];
    }
}
