using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gesture : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static Vector2 lastSwipe = Vector2.zero;
    public static bool pressed = false;

    private float cooldown = 0f;

    private void Update()
    {
        cooldown -= Time.deltaTime;
        //if (cooldown <= 0f)
        //    lastSwipe = Vector2.zero;
    }
    private void LateUpdate()
    {
        lastSwipe = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lastSwipe = eventData.position - eventData.pressPosition;
        Debug.Log("last Swipe: " + lastSwipe);
        pressed = false;
        cooldown = 0.05f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }
}
