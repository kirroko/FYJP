using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gesture : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static Vector2 lastSwipe = Vector2.zero;
    public static bool pressed = false;
    public static bool tap = false;

    private float cooldown = 0.03f;

    private void Update()
    {
        if(tap)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0f)
                tap = false;
        }
    }

    private void LateUpdate()
    {
        lastSwipe = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lastSwipe = eventData.position - eventData.pressPosition;
        pressed = false;
        tap = false;
        cooldown = 0.03f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        tap = true;
    }
}
