using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementParry : MonoBehaviour
{
    public Vector2 FacingDirection
    {
        get { return facingDirection; }
    }

    [Header("References")]
    [SerializeField] private Joystick input = null;

    [SerializeField] private Rigidbody2D rigi2d = null;

    [Header("Attributes")]
    public float rollVelocity = 100f;

    public float turnSpeed = 50f;

    // private variable in class
    private bool hasFaced = false;
    private Vector2 facingDirection = new Vector2();
    private void Awake()
    {
        rigi2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 lastInput = input.Direction;

        if (lastInput.x != 0f || lastInput.y != 0f)
        {
            hasFaced = true;

            // Rotate character
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * lastInput;

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        if (input.Released && hasFaced)
        {
            hasFaced = false;

            // Do roll
            Debug.Log("LastDirection " + input.LastInput);
            rigi2d.velocity = input.LastInput * rollVelocity;
        }
    }

    private void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        Gesture.lastSwipe = Vector2.zero;
    }

    //[Header("References")]
    //[SerializeField] private Joystick input = null;

    //[Header("Stats")]
    //[SerializeField] private float moveSpeed = 5f;
    //[SerializeField] private float jumpForce = 5f;

    //private Rigidbody2D rb = null;

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //}

    //private void Update()
    //{
    //    Vector3 targetPos = transform.position;

    //    targetPos.x += input.Horizontal * moveSpeed * Time.deltaTime;

    //    if (Gesture.lastSwipe.y > 0f)
    //    {
    //        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    //    }

    //    transform.position = targetPos;
    //}

    //private void LateUpdate()
    //{
    //    Gesture.lastSwipe = Vector2.zero;
    //}
}