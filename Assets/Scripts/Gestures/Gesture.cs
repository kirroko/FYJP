using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gesture : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private float minDrag = 0f;

    public static Vector2 lastSwipe = Vector2.zero;

    public void OnPointerUp(PointerEventData eventData)
    {
        lastSwipe = eventData.position - eventData.pressPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    { 
    }
}
