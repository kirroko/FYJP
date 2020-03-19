using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gesture : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static bool OnPressed
    {
        get { return onPressed; }
    }

    [SerializeField] private float minDrag = 0f;

    public static Vector2 lastSwipe = Vector2.zero;

    private static bool onPressed = false;

    public void OnPointerUp(PointerEventData eventData)
    {
        lastSwipe = eventData.position - eventData.pressPosition;

        onPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPressed = true;
    }
}
