using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public GameObject Source
    {
        get { return source; }
        set { source = value; }
    }

    [Header("Attributes")]
    public float speed = 5f;

    public float lifeTime = 10f; // 5 sec

    [SerializeField] private Vector2 direction = new Vector2();

    private Rigidbody2D rigi2d;
    private GameObject source;

    private void Awake()
    {
        rigi2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, lifeTime);

        ChangeRotation(direction);

        rigi2d.velocity = direction * speed;
    }

    // Update is called once per frame
    private void Update()
    {
        //direction.Normalize();
        //rigi2d.velocity = direction * speed;
    }

    public void FlipVelocity()
    {
        rigi2d.velocity = -rigi2d.velocity;
        Vector2 dir = rigi2d.velocity.normalized;
        ChangeRotation(dir);
    }

    public void ChangeVelocity(Vector2 dir)
    {
        // what is the direction and is the velocity correct
        rigi2d.velocity = Vector2.zero;
        rigi2d.velocity = dir * speed;
        ChangeRotation(dir);
    }

    public void ChangeVelocity(Vector2 dir, float value)
    {
        rigi2d.velocity = Vector2.zero;
        rigi2d.velocity = direction * value;
        ChangeRotation(dir);
    }

    private void ChangeRotation(Vector2 dir)
    {
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * dir;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        // Debug.Log(gameObject + " Hit " + collider2D.gameObject);
        if (collider2D.gameObject.tag == "System")
        {
            Physics2D.IgnoreCollision(collider2D, GetComponent<Collider2D>());
        }
        if (collider2D.gameObject.CompareTag("Player") || collider2D.gameObject.CompareTag("Enemy"))
        {
            // destroy player, destroy bullet
            // Destroy(collider2D.gameObject);
            Destroy(gameObject);
        }
    }
}