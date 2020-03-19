using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPickUp : MonoBehaviour
{
    private GameObject pickedUpObj = null;
    private float cooldown = 0f;

    private Vector2 swipe = Vector2.zero;
    private bool pressed = false;

    private void Update()
    {
        swipe = Gesture.lastSwipe;
        pressed = Gesture.tap;

        if(pickedUpObj != null && Gesture.tap)
        {
            pickedUpObj.transform.SetParent(null);
            pickedUpObj = null;
            cooldown = 0.1f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        cooldown -= Time.deltaTime;

        if (cooldown > 0f || !collision.CompareTag("Moveable") || swipe != Vector2.zero) return;

        if (pressed && pickedUpObj == null)
        {
            pickedUpObj = collision.gameObject;
            collision.transform.position = transform.position;
            collision.transform.SetParent(transform);
            cooldown = 0.1f;
        }

    }
}
