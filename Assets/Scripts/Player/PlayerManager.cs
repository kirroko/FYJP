using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class will get the loaded player data from the files and 
 * update it in game
 * 
 * Loaded / Saved Data:
 * Player's Color Info
 * 
 * Anything related to the player's loading and saving can be written here
 */

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;
    public ColorManager GetColorManager { get{ return instance.colorManager; } }

    [SerializeField] private ColorManager colorManager = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
        //InitData();
    }


    /**
     * This functions loads all the data from the save files and place updates the game accordingly
     * 
     * Anything player data that is loaded and to be updated can be found here
     * 
     * This function is also called when the player clear all data
     */
    public void InitData()
    {
        //Init Color data
        foreach(COLORS color in colorManager.colorList.Keys)
        {
            ColorData colorData = SaveSystem.LoadColor(color.ToString());
            if(colorData == null)
            {
                SaveSystem.SaveColor(colorManager.colorList[color]);
            }
            else
            {
                colorManager.colorList[color].IsLocked = colorData.locked;
            }
        }
    }

    /**
     * This Function is called whenever a player enters a level.
     * 
     * It will check if there is a new color unlocked for the player to use 
     * If there is it will update the data and save it to the file
     */
    public void UpdateColorList(ColorManager newColorList)
    {
        Dictionary<COLORS, bool> colorToUpdate = new Dictionary<COLORS, bool>();

        Debug.Log(newColorList.name);
        foreach(COLORS color in colorManager.colorList.Keys)
        {
            //current data is locked but new data say not locked
            if(colorManager.colorList[color].IsLocked && !newColorList.colorList[color].IsLocked)
            {
                colorToUpdate.Add(color, newColorList.colorList[color].IsLocked);
            }
        }

        foreach(COLORS color in colorToUpdate.Keys)
        {
            colorManager.colorList[color].IsLocked = colorToUpdate[color];
            //SaveSystem.SaveColor(colorManager.colorList[color]);
        }
    }
}
