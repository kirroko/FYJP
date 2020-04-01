using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private Vector2 direction = Vector2.zero;
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
