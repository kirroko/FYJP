using UnityEngine;
using TMPro;

public class AI : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private TextMeshProUGUI healthText = null;
    public bool stun = false;

    public void TakeDamage(float damage)
    {
        health -= damage;

        healthText.text = health.ToString();

        if (health <= 0f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
