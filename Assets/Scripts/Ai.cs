using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ai : MonoBehaviour
{
    public float speed;
    public float distance;
    public float damage = 10f;
    public float health = 50f;
    private float dir = 1f;

    public bool movingRight = true;

    public Transform groundDetection;

    public TextMeshProUGUI healthText = null;
    private void Update()
    {
        transform.Translate(new Vector2(dir, 0f) * speed * Time.deltaTime);


        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                dir = 1f;
                movingRight = false;
            }
            else
            {
                dir = -1f;
                movingRight = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerInfo info = collision.gameObject.GetComponent<PlayerInfo>();

        if(info != null)
        {
            info.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        healthText.text = health.ToString();
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
