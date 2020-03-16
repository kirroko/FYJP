using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float speed = 1.5f;
    public float movementSmoothing = .05f;
    public float jumpForce = 100f;
    public float dashSpeedModifier = 3f;
    public float dashCooldown = 1.5f;
    public float characterOriginOffset = -0.5f;
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float groundDistance = 0.3f;

    private Rigidbody2D rigi2d;
    private Collider2D playerCollider;
    public float horizontalMove;

    private bool grounded = false;
    private bool onPlatform = false;
    private Vector3 velocity = Vector3.zero;
    private float dashCooldownActual = 0;

    private void Awake()
    {
        rigi2d = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CircleCollider2D>();
        dashCooldownActual = dashCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        dashCooldownActual += Time.deltaTime;
        horizontalMove = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownActual > dashCooldown)
        {
            Dash();
            dashCooldownActual = 0f;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            DropFromPlatform();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        Movement();
    }

    private void Movement()
    {
        Vector3 targetVelocity = new Vector2(horizontalMove * speed, rigi2d.velocity.y);
        rigi2d.velocity = Vector3.SmoothDamp(rigi2d.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    private void Jump()
    {
        if (grounded)
        {
            rigi2d.AddForce(new Vector2(0, jumpForce));
        }
    }

    private void Dash()
    {
        Debug.Log("Dashing");
        rigi2d.velocity = new Vector2(rigi2d.velocity.x * dashSpeedModifier, rigi2d.velocity.y);
    }

    private void DropFromPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, characterOriginOffset, 0), -Vector2.up, groundDistance, platformLayer);

        if(hit.collider != null)
        {
            playerCollider.isTrigger = true;
            //Invoke("ResetCollider", 0.3f);
        }
    }

    private void ResetCollider()
    {
        playerCollider.isTrigger = false;
    }

    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, characterOriginOffset, 0), -Vector2.up, groundDistance, groundLayer);

        if (hit.collider != null)
        {
            ResetCollider();
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        Debug.DrawRay(transform.position + new Vector3(0, characterOriginOffset, 0), -Vector2.up * groundDistance, Color.red);
    }
}