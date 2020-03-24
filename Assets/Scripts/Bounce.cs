using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private float bounceForce = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < 0f)//Check if collided object is above you
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
