using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool tap = false;
    public bool pressed = false;

    private bool once = false;

    private void LateUpdate()
    {
        if (tap)
        {
            tap = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tap = true;
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tap = false;
        pressed = false;
    }
}
