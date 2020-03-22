using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : MonoBehaviour
{
    [SerializeField] protected PAINT_COLOURS colour = PAINT_COLOURS.WHITE;
    [SerializeField] protected Color ownColor = new Color();
    [SerializeField] protected Color mixColor1 = new Color();
    [SerializeField] protected Color mixColor2 = new Color();


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInfo info = collision.gameObject.GetComponent<PlayerInfo>();

        if (info != null)
        {
            info.colour = colour;
            info.GetComponent<SpriteRenderer>().color = ownColor;
            info.GetComponent<PlatformVisibility>().UpdateVisibility();
        }


    }
}
