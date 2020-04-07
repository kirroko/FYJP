using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformColor : MonoBehaviour
{
    [SerializeField] private COLORS platformColor = COLORS.NONE;

    private SpriteRenderer spriteRenderer = null;
    private new Collider2D collider = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
        EventManager.instance.PlatformColorEvent += CheckColor;
        CheckColor(COLORS.WHITE);
    }

    private void CheckColor(COLORS playerColor)
    {
        if (playerColor == platformColor)
        {
            collider.isTrigger = false;
            Color temp = spriteRenderer.color;
            temp.a = 1f;
            spriteRenderer.color = temp;
        }
        else
        {
            collider.isTrigger = true;
            Color temp = spriteRenderer.color;
            temp.a = 0.25f;
            spriteRenderer.color = temp;
        }
    }
}
