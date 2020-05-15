using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool tap = false;
    public bool pressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(Tap());
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        tap = false;
        pressed = false;
    }

    private IEnumerator Tap()
    {
        tap = true;
        yield return null;
        tap = false;
    }
}
