using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public Vector2 springForce = new Vector2();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if(rb != null)
        {
            rb.AddForce(springForce, ForceMode2D.Impulse);
        }
    }
}
