using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a platform where the player has to be the same color as the platform in order to stand on it
 */
[RequireComponent(typeof(Collider2D))]
public class PlatformColor : MonoBehaviour
{
    [SerializeField] private COLORS platformColor = COLORS.NONE;

    private SpriteRenderer spriteRenderer = null;
    private List<Collider2D> colliders = new List<Collider2D>();

    private bool canStand = false;
    private bool isInside = false;///< Used to Check if the player is inside of the object.

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EventManager.instance.PlatformColorEvent -= CheckColor;
        EventManager.instance.PlatformColorEvent += CheckColor;
        CheckColor(COLORS.WHITE);

        GetComponents<Collider2D>(colliders);
        foreach (Collider2D collider in colliders)
            collider.isTrigger = true;
    }

    /**
    * This function checks the color of the player with the color of the platforms changes the standing mode accordingly
    */
    private void CheckColor(COLORS playerColor)
    {
        if (playerColor == platformColor)
        {
            canStand = true;
            if(!isInside)
                MakeStandable();
        }
        else
        {
            canStand = false;
            MakeUnstandable();
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.PlatformColorEvent -= CheckColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInfo>() != null)
            isInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInfo>() == null) return;

        isInside = false;

        if (canStand)
            MakeStandable();
    }

    private void MakeStandable()
    {
        foreach (Collider2D collider in colliders)
            collider.isTrigger = false;

        Color temp = spriteRenderer.color;
        temp.a = 1f;
        spriteRenderer.color = temp;
    }

    private void MakeUnstandable()
    {
        foreach (Collider2D collider in colliders)
            collider.isTrigger = true;

        Color temp = spriteRenderer.color;
        temp.a = 0.25f;
        spriteRenderer.color = temp;
    }
}
