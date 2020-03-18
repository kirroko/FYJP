using UnityEngine;

public class Parry : MonoBehaviour
{
    [Header("Attributes")]
    public float fov = 90f;

    // Private 
    private bool canDeflect = false;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        // Wait for player's input

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 direction = (other.transform.position - transform.position).normalized;
        float angle = Vector3.Dot(direction, transform.right);
        Debug.Log(angle);
        if (angle > fov * 0.5f) 
            return;

        // Do deflecting
        // Start slow-mo and wait for player responds to deflect projectile
        canDeflect = true;
    }
}