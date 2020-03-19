using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishBlocks : MonoBehaviour
{
    [SerializeField] private float squishAmt = 1f;
    [SerializeField] private float minSwipeAmt = -275f;

    private bool squish = false;
    private bool triggered = false;

    private void Update()
    {
        if (triggered && Gesture.lastSwipe.y < minSwipeAmt && Gesture.lastSwipe.x < 100f && Gesture.lastSwipe.x > -100f)
            squish = true;
    }

    private void LateUpdate()
    {
        triggered = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
            triggered = true;
            if (!squish) return;

            squish = false;
            ContactPoint2D contact = collision.GetContact(0);
            if(contact.normal.y < 0f)
            {
                Vector3 targetPos = transform.position;
                Vector3 targetScale = transform.localScale;

                targetPos.y -= squishAmt / 7f;
                targetScale.y -= squishAmt;

                transform.position = targetPos;
                transform.localScale = targetScale;
            }
        }
    }
}
