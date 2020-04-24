using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void InitData()
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
            Debug.Log(color + ": " + colorManager.colorList[color].IsLocked);
            //SaveSystem.SaveColor(colorManager.colorList[color]);
        }
    }
}
