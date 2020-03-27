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

    private float tapCooldown = 0.03f;
    private float holdCooldown = 0.1f;
    private bool startPress = false;

    private int touchIndex = -1;

    private void Update()
    {
        if(tap)
        {
            tapCooldown -= Time.unscaledDeltaTime;
            if (tapCooldown <= 0f)
                tap = false;
        }

        if(startPress)
        {
            holdCooldown -= Time.unscaledDeltaTime;
            if (holdCooldown <= 0f)
            {
                heldDown = true;
            }
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
        startPress = false;
        tap = false;
        heldDown = false;
        tapCooldown = 0.03f;
        holdCooldown = 0.1f;
        deltaPos = Vector2.zero;
        touchIndex = -1;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPress = true;
        tap = true;
        pressPos = eventData.pressPosition;
        touchIndex = Input.touchCount - 1;
    }
}
