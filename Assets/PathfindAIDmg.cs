using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindAIDmg : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(collision.collider.gameObject);
    }
}
