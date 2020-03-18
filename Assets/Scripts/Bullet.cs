using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)),RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    [Header("Attributes")]
    public float speed = 5f;
    public float lifeTime = 3f; // 3 sec

    [SerializeField] private Vector2 direction = new Vector2();

    private Rigidbody2D rigi2d;

    void Awake()
    {
        rigi2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);

        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * direction;

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 250 * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        direction.Normalize();
        rigi2d.velocity = direction * speed;
    }

    //private void OnCollisionEnter2D(Collider2D col)
    //{
    //    Debug.Log(gameObject.name + " Hit " + col.gameObject.name);

    //    if(col.gameObject.CompareTag("Player"))
    //    {
    //        Destroy(col.gameObject);
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        Debug.Log(gameObject + " Hit " + collider2D.gameObject);
        if (collider2D.gameObject.tag == "System")
        {

            Physics2D.IgnoreCollision(collider2D, GetComponent<Collider2D>());
        }
        if (collider2D.gameObject.CompareTag("Player"))
        {
            // destroy player, destroy bullet
            Destroy(collider2D.gameObject);
            Destroy(gameObject);
        }
    }
}
