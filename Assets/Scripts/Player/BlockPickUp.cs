using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPickUp : MonoBehaviour
{
    private GameObject pickedUpObj = null;
    private float cooldown = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        cooldown -= Time.deltaTime;

        if (cooldown > 0f || !collision.CompareTag("Moveable") || Gesture.lastSwipe != Vector2.zero) return;

        if (Gesture.pressed && pickedUpObj == null)
        {
            pickedUpObj = collision.gameObject;
            collision.transform.position = transform.position;
            collision.transform.SetParent(transform);
            cooldown = 0.1f;
        }
        else if(Gesture.pressed && pickedUpObj != null)
        {
            pickedUpObj.transform.SetParent(null);
            pickedUpObj = null;
            cooldown = 0.1f;
        }
    }
}
