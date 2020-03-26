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

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dampForce = 0.05f;
    [SerializeField] private float jumpForce = 5f;

    [Header("Dashing")]
    [SerializeField] private float dashCDDuration = 1f;

    [Header("Bouncing")]
    [SerializeField] private float maxBounce = 4f;
    [SerializeField] private float addBounce = 0.5f;

    [Header("Gliding")]
    [SerializeField] private float glideDrag = 1f;

    [Header("Smash")]
    [SerializeField] private Vector2 smashBox = Vector2.zero;
    [SerializeField] private Vector2 stunBox = Vector2.zero;

    private Rigidbody2D rb = null;
    private Collider2D collider = null;

    //Movement
    private float xInput = 0f;
    private Vector2 direction = Vector2.zero;

    //Jumping
    private bool isGrounded = false;

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
        cooldown.text = "0";
    }

    private void Update()
    {
        dashCD -= Time.deltaTime;
        cooldown.text = dashCD.ToString("F2");

        //Get Input
        xInput = input.Horizontal;

        //Jumping
        if (jumpButton.tap && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        //Dash
        if (dashButton.tap)
            Dash();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + 0.1f, groundLayer);

        Vector2 targetVel = rb.velocity;

        targetVel.x += xInput * moveSpeed;
        targetVel.x *= 1f - dampForce;

        if (targetVel.y > 0f)
            targetVel.y *= 1f - dampForce;

        rb.velocity = targetVel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnDrawGizmosSelected()
    {
        Vector2 center = collider.bounds.center - new Vector3(0f, collider.bounds.extents.y, 0f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, stunBox);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void Dash()
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

        rb.velocity = direction * dashSpeed;
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
