using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public GameObject bullet;

    public float shootCooldown = 1.5f;

    [Header("Information")]
    [SerializeField] private GameObject target;
    [SerializeField] private Transform firePosition;

    private float actualShootCooldown = 0;

    // Start is called before the first frame update
    private void Start()
    {
        actualShootCooldown = shootCooldown;
        target = GameObject.FindGameObjectWithTag("Player");
        firePosition = gameObject.transform.GetChild(0).transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (actualShootCooldown > shootCooldown && target != null)
        {
            actualShootCooldown = 0;

            // Shoot bullet
            Vector2 directionToTarget = target.transform.position - transform.position;
            directionToTarget.Normalize();
            GameObject tempGO = Instantiate(bullet, firePosition.position, transform.rotation);
            tempGO.GetComponent<Bullet>().Direction = directionToTarget;
            tempGO.GetComponent<Bullet>().Source = gameObject;
            Debug.Log("Bullet shot");
        }
        else
            actualShootCooldown += Time.deltaTime;
    }
}