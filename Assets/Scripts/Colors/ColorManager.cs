using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorManager", menuName = "Colors/Manager", order = 1)]

/**
 * One of this object is to be created to store the colors that have been unlocked in a level.
 * 
 * E.g:
 * 
 * Level 1 player can only use Red Color if they have not complete it before.
 * 
 * All colors in colorList shld be locked except for white and red.
 * 
 * If Player has unlocked other colors and goes back to level 1, they will be able to use the other colors and not just Red.
 */
public class ColorManager : ScriptableObject
{
    [SerializeField] private List<BaseColor> colors = null;
    public Dictionary<COLORS, BaseColor> colorList = new Dictionary<COLORS, BaseColor>();

    private void OnEnable()
    {
        foreach(BaseColor color in colors)
        {
            colorList.Add(color.GetMain, color);
        }
    }
}
