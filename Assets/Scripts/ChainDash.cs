using UnityEngine;

public class ChainDash : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if(player != null)
        {
            if(player.IsDashing() && player.GetComponent<Rigidbody2D>().velocity.magnitude > 1f)
            {
                Debug.Log("go thru");
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                player.ResetDash();
            }
        }
    }
}
