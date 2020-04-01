using UnityEngine;

public class WindDraft : MonoBehaviour
{
    [SerializeField] private Vector2 direction = new Vector2(1f, 0f);
    [SerializeField] private float force = 10f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

        if (rb == null) return;

        rb.velocity += direction * force;
    }
}
