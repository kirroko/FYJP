using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishBlocks : MonoBehaviour
{
    [SerializeField] private float squishAmt = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
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
