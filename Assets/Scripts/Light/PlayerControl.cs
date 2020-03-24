using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Joystick input = null;
    [SerializeField] private Orb orb = null;

    [Header("Attributes")]
    public float speed = 5f;
    public float turnSpeed = 1.0f;
    public float jumpForce = 5f;
    public float radius = 5f;
    public LayerMask enemyLayer;

    [Header("Properties")]
    public bool grounded;
    public LayerMask groundLayer;
    public float characterOriginOffset = -0.5f;
    public float groundDistance = 0.3f;
    private Vector3 forward;
    private Vector3 groundNormal;
    private RaycastHit groundHit;

    // Private variable
    private Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 dir = input.Direction;
        Move(dir.x, dir.y);

        //if (Gesture.OnPressed && grounded) // jump
        //{
        //    Jump();
        //}
        if(Input.GetKeyDown(KeyCode.Space))
        {
            orb.ReleaseCharge();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    public void Jump()
    {
        if(grounded)
            rig.velocity = new Vector3(rig.velocity.x, jumpForce, rig.velocity.z);
    }

    public void Attack()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radius / 2, enemyLayer);
        foreach(Collider collider in col)
        {
            collider.gameObject.GetComponent<Rigidbody>().AddForce()
        }
    }

    private void Move(float x, float y)
    {
        Vector3 move = new Vector3(x, 0, y);
        Vector3 moveDir = transform.InverseTransformDirection(move);
        moveDir = Vector3.ProjectOnPlane(moveDir, groundNormal);
        float rotationAmount = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        float force = move.magnitude * speed;

        float rAngle = rig.transform.localEulerAngles.y + rotationAmount;

        rig.AddForce(forward * force);
        rig.rotation = Quaternion.RotateTowards(rig.rotation, Quaternion.Euler(0, rAngle, 0), turnSpeed);
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, characterOriginOffset, 0), -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, groundDistance))
        {
            grounded = true;
            groundNormal = hit.normal;
            forward = Vector3.Cross(transform.right, groundNormal);
        }
        else
        {
            grounded = false;
            forward = transform.forward;
        }
        Debug.DrawRay(transform.position + new Vector3(0, characterOriginOffset, 0), -transform.up * groundDistance, Color.red);
    }

    public Transform NearbyEnemy()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        if (col.Length != 0)
            return col[0].transform;
        else
            return GameObject.FindGameObjectWithTag("Enemy").transform;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, radius / 2);
    }
}