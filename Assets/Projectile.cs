using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public bool Released
    {
        get { return released; }
        set { released = value; }
    }

    public Transform TargetLocation
    {
        get { return targetLocation; }
        set { targetLocation = value; }
    }

    [Header("Attributes")]
    public float speed = 5f;
    public float radius = 2f;
    public LayerMask effectedLayer;

    // private
    private Rigidbody rig;
    private Transform targetLocation;
    private bool released = false;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(released)
        {
            Vector3 dir = targetLocation.position - transform.position;
            dir.Normalize();
            rig.velocity = dir * speed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); // destroy projectile
        Collider[] collider = Physics.OverlapSphere(transform.position, radius, effectedLayer);
        foreach(Collider col in collider)
        {
            Destroy(col.gameObject);
        }
    }
}
