using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformColor : MonoBehaviour
{
    [SerializeField] private COLORS platformColor = COLORS.NONE;

    private SpriteRenderer spriteRenderer = null;
    private List<Collider2D> colliders = new List<Collider2D>();

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

    private void CheckColor(COLORS playerColor)
    {
        if (playerColor == platformColor)
        {
            foreach (Collider2D collider in colliders)
                collider.isTrigger = false;

            Color temp = spriteRenderer.color;
            temp.a = 1f;
            spriteRenderer.color = temp;
        }
        else
        {
            foreach (Collider2D collider in colliders)
                collider.isTrigger = true;

            Color temp = spriteRenderer.color;
            temp.a = 0.25f;
            spriteRenderer.color = temp;
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.PlatformColorEvent -= CheckColor;
    }
}
