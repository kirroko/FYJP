using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorManager", menuName = "Colors/Manager", order = 1)]
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
