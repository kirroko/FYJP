using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeYellow : ChangeColour
{
    private void Start()
    {
        colour = PAINT_COLOURS.YELLOW;
        ownColor = new Color(1f, 1f, 0f, 1f);
        mixColor1 = new Color(0f, 1f, 0f, 1f);//Green
        mixColor2 = new Color(1f, 0.65f, 0f, 1f);//Orange
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInfo info = collision.gameObject.GetComponent<PlayerInfo>();

        if (info == null) return;

        if(info.colour == PAINT_COLOURS.WHITE)
        {
            info.colour = colour;
            info.GetComponent<SpriteRenderer>().color = ownColor;
        }
        else if(info.colour == PAINT_COLOURS.RED)
        {
            info.colour = PAINT_COLOURS.ORANGE;
            info.GetComponent<SpriteRenderer>().color = mixColor2;
        }
        else if (info.colour == PAINT_COLOURS.BLUE)
        {
            info.colour = PAINT_COLOURS.GREEN;
            info.GetComponent<SpriteRenderer>().color = mixColor1;
        }
        else if (info.colour != colour && info.colour != PAINT_COLOURS.ORANGE && info.colour != PAINT_COLOURS.GREEN)
        {
            info.colour = PAINT_COLOURS.BLACK;
            info.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f);
        }

        info.GetComponent<PlatformVisibility>().UpdateVisibility();
    }
}
