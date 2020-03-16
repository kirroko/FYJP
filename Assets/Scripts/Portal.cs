using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    public Vector3 addposition;
    public GameObject exitPortal;
    public float force = 10f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerInfo>() != null)
        {
            Vector3 location = exitPortal.transform.position;
            other.gameObject.transform.position = location;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(other.GetComponent<SimpleMovement>().dir * force, 0f), ForceMode2D.Impulse);
        }
    }
}
