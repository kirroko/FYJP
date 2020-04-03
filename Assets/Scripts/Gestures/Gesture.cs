using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gesture : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static Vector2 lastSwipe = Vector2.zero;
    public static Vector2 deltaPos = Vector2.zero;
    public static Vector2 pressPos = Vector2.zero;
    public static Vector2 currentPos = Vector2.zero;
    public static bool heldDown = false;
    public static bool tap = false;

    private int touchIndex = -1;

    private void Update()
    {
        if (tap)
        {
            tap = false;
        }

        if(heldDown)
        {
            currentPos = Input.GetTouch(touchIndex).position;
            deltaPos = Input.GetTouch(touchIndex).deltaPosition;
        }
    }

    private void LateUpdate()
    {
        lastSwipe = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lastSwipe = eventData.position - eventData.pressPosition;
        tap = false;
        heldDown = false;
        deltaPos = Vector2.zero;
        touchIndex = -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        heldDown = true;
        tap = true;
        pressPos = eventData.pressPosition;
        touchIndex = Input.touchCount - 1;
    }

    private void OnDestroy()
    {
        lastSwipe = Vector2.zero;
        deltaPos = Vector2.zero;
        pressPos = Vector2.zero;
        currentPos = Vector2.zero;
        heldDown = false;
        tap = false;
    }
}
