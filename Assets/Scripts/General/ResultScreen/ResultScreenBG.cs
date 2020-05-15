using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * This class will set the border of the frame and stars in the result screen.
 * 
 * The border of the frame will change based on how many stars is obtained in that run.
 * 
 * If 1 Star is obtained, frame will be Bronze in color.
 * 
 * If 2 Star is obtained, frame will be Silver in color.
 * 
 * If 3 Star is obtained, frame will be Gold in color.
 */
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
        for (int i = 0; i < lvlManager.CurrentLevel.currentRunStar; i++)
        {
            stars.gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        border.sprite = borderImages[Mathf.Clamp(lvlManager.CurrentLevel.currentRunStar - 1, 0, 2)];
    }
}
