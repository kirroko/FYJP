using UnityEngine;

public class Parry : MonoBehaviour
{
    [Header("Attributes")]
    public float angleThreshold = 45f;
    [SerializeField] private float triggerRadius = 1.5f;

    // Private 
    private bool canDeflect = false;
    private GameObject theProjectile;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // Wait for player's input
        if(Gesture.OnPressed && canDeflect)
        {
            Debug.Log("Deflecting bullet");
            canDeflect = false;

            // Do deflecting
            // Start slow-mo and wait for player responds to deflect projectile
            //Time.timeScale = 0.5f;
            if (theProjectile.GetComponent<Bullet>().Source != null)
            {
	            Debug.Log(theProjectile.GetComponent<Bullet>().Source.transform.position + " - " + theProjectile.transform.position);
	            Vector2 dir = (theProjectile.GetComponent<Bullet>().Source.transform.position - theProjectile.transform.position).normalized;
	            Debug.Log(dir);
	            theProjectile.GetComponent<Bullet>().ChangeVelocity(dir); // Possible to speed up the bullet to the source instead?
            }
            else
            {
                theProjectile.GetComponent<Bullet>().FlipVelocity();
            }

            theProjectile = null; // clear theProjectile in case of access violation
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 direction = (other.transform.position - transform.position).normalized;
        float angle = Mathf.Acos(Vector3.Dot(direction, transform.right)) * Mathf.Rad2Deg;
        Debug.Log("angle " + angle);

        if(angle > angleThreshold)
        {
            Debug.Log("Return from triggerEnter");
            return;
        }

        //Vector2 direction = (other.transform.position - transform.position).normalized;
        //// Debug.Log(other.transform.position + " - " + transform.position);
        //Debug.Log(direction + " | " + transform.right);
        //float angle = Vector3.Dot(direction, transform.right);
        //Debug.Log(angle);
        ////Debug.Log("angle : " + angle);
        ////Debug.Log("angle " + angle + " > " + " Fov " + fov);
        //if (angle > fov)
        //{
        //    Debug.Log("Return from triggerEnter");
        //    return;
        //}

        Debug.Log("deflect : true");
        canDeflect = true;
        theProjectile = other.gameObject;
    }
}