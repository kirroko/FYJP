using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a spring platform to be used in the game
 * 
 * Any Object with rigidbody2d when landing on the spring will be bounced in the direction set
 */
public class Spring : MonoBehaviour
{
    ///Direction where the object should bounce
    [SerializeField] private Vector2 direction = Vector2.zero;

    ///Force to be applied in the stated direction
    [SerializeField] private float force = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        ContactPoint2D contact = collision.GetContact(0);

        if(contact.normal.y < 0f)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }
}
