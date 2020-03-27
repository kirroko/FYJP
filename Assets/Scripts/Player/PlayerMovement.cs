using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Joystick GetInput
    {
        get { return input; }
    }

    public HoldButton GetJumpButton
    {
        get { return jumpButton; }
    }

    public HoldButton GetDashButton
    {
        get { return dashButton; }
    }

    [Header("References")]
    [SerializeField] private Joystick input = null;
    [SerializeField] private HoldButton jumpButton = null;
    [SerializeField] private HoldButton dashButton = null;
    [SerializeField] private LayerMask groundLayer = 0;
    [SerializeField] private LayerMask enemiesLayer = 0;
    [SerializeField] private LayerMask wallLayer = 0;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dampForce = 0.05f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float wallJumpForce = 2.5f;

    [Header("Dashing")]
    [SerializeField] private float dashCDDuration = 1f;

    [Header("Wall Jump")]
    [SerializeField] private float distanceToWall = 0.5f;
    [SerializeField] private float controlCDDuration = 0.1f;
    

    //Component References
    private Rigidbody2D rb = null;
    private Collider2D collider = null;
    private PlayerColor playerColor = null;

    //Movement
    private float xInput = 0f;
    private int direction = 0;

    //Jumping
    private bool isGrounded = false;
    private bool isWallRiding = false;
    private float controlCD = 0f;

    //Dashing
    private bool isDashing = false;
    private float dashCD = 0f;


    [Header("TBR")]
    public TextMeshProUGUI cooldown = null;
    private Vector2 test;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        playerColor = GetComponent<PlayerColor>();
        cooldown.text = "0";
    }

    private void Update()
    {
        dashCD -= Time.deltaTime;
        cooldown.text = dashCD.ToString("F2");
        
        controlCD -= Time.deltaTime;
        //Get Input
        xInput = input.Horizontal;

        //Jumping && Wall jumping
        if (jumpButton.tap && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        else if(jumpButton.tap && isWallRiding)
        {
            rb.AddForce(new Vector2(-direction * wallJumpForce, wallJumpForce), ForceMode2D.Impulse);
            controlCD = controlCDDuration;
        }

        //Dash
        //Use this Dash when player is not blue
        if (dashButton.tap && playerColor.GetCurrentColor.GetMain != COLORS.BLUE)
            Dash();

        Debug.DrawRay(transform.position, new Vector2(direction, 0) * distanceToWall, Color.red);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + 0.1f, groundLayer);

        Move();
        if (!isGrounded)
            isWallRiding = CastRayInDirection(direction);
    }
    private void Move()
    {
        Vector2 targetVel = rb.velocity;

        targetVel.x += xInput * moveSpeed;
        targetVel.x *= 1f - dampForce;

        if (targetVel.y > 0f)
            targetVel.y *= 1f - dampForce;

        if(controlCD < 0)
            rb.velocity = targetVel;

        // Direction
        if (xInput > 0)
            direction = 1;
        else if (xInput < 0)
            direction = -1;
        else
            direction = 0;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Dash()
    {
        if (dashCD > 0f) return;

        isDashing = true;
        dashCD = dashCDDuration;

        Vector2 direction = Vector3.zero;

        //Move Right
        if (input.Direction.x > 0.5f)
            direction.x = 1f;
        else if (input.Direction.x < -0.5f)
            direction.x = -1f;

        if (input.Direction.y > 0.5f)
            direction.y = 1f;
        else if (input.Direction.y < -0.5f)
            direction.y = -1f;

        rb.AddForce(direction * dashSpeed, ForceMode2D.Impulse);
        //rb.velocity = direction * dashSpeed;
    }

    private bool CastRayInDirection(int direction)
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), distanceToWall, wallLayer);


        if(hit.collider != null)
        {
            return true;
        }

        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void ResetDash()
    {
        isDashing = false;
        dashCD = 0f;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
}
