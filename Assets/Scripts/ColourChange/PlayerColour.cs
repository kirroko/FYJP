using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PAINT_COLOURS
{
    RED,
    BLUE,
    YELLOW,
    PURPLE,
    GREEN,
    ORANGE,
    BLACK,
    WHITE,

    COLOUR_COUNT,
}

[System.Serializable]
public class Colors : MonoBehaviour
{
    public PAINT_COLOURS color;
    public bool mixed;
    public PAINT_COLOURS baseColor1; 
    public PAINT_COLOURS baseColor2; 
}
