using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveColour : ChangeColour
{
    private void Start()
    {
        colour = PAINT_COLOURS.WHITE;
        ownColor = new Color(1f, 1f, 1f, 1f);
    }

}
