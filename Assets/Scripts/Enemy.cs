using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public GameObject bullet;
    public float shootCooldown = 1.5f;

    [Header("Information")]
    [SerializeField] private GameObject target;

    private float actualShootCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        actualShootCooldown = shootCooldown;
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        actualShootCooldown += Time.deltaTime;

        if(actualShootCooldown > shootCooldown)
        {
            actualShootCooldown = 0;

            // Shoot bullet
            Vector2 directionToTarget = target.transform.position - transform.position;
            directionToTarget.Normalize();
            GameObject tempGO = Instantiate(bullet, transform.position, transform.rotation);
            tempGO.GetComponent<Bullet>().Direction = directionToTarget;
            Debug.Log("Bullet shooted");
        }
    }
}
