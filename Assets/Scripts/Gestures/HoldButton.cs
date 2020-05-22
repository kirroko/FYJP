using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool tap = false;
    public bool pressed = false;

    private void Start()
    {
        EventManager.instance.resetJoystickEvent -= TriggerResetJoystickEvent;
        EventManager.instance.resetJoystickEvent += TriggerResetJoystickEvent;
    }

    private void OnDestroy()
    {
        EventManager.instance.resetJoystickEvent -= TriggerResetJoystickEvent;
    }

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

    private void TriggerResetJoystickEvent()
    {
        tap = false;
        pressed = false;
    }
}
