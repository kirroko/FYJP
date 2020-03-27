using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorManager", menuName = "Colors/Manager", order = 1)]
public class ColorManager : ScriptableObject
{
    [SerializeField] private List<WhiteColor> colors = null;
    public Dictionary<COLORS, WhiteColor> colorList = new Dictionary<COLORS, WhiteColor>();

    private void OnEnable()
    {
        foreach(WhiteColor color in colors)
        {
            colorList.Add(color.GetMain, color);
        }
    }
}
