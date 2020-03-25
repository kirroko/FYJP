using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
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
    [SerializeField] private float slowCDDuration = 1f;

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
    private bool isSlowedDown = false;
    private float slowDownCD = 0.5f;
    private float timeAfterDash = 0f;

    //Bouncing
    private GameObject bounceObject = null;
    private float bounceBoost = 1f;

    //Gliding
    private bool isGliding = false;
    private float glideMultiplier = 1f;

    //Smash
    private HoldButton smashButton = null;
    private bool smash = false;
    private float doubleTapCount = 0.2f;
    private int tapNum = 0;

    [Header("TBR")]
    public TextMeshProUGUI cooldown = null;
    private Vector2 test;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        smashButton = jumpButton;
        cooldown.text = "0";
        slowDownCD = slowCDDuration;
    }

    private void Update()
    {
        dashCD -= Time.deltaTime;
        cooldown.text = dashCD.ToString("F2");

        //Get Input
        xInput = input.Horizontal;
        //xInput = Input.GetAxisRaw("Horizontal");
        //test = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Jumping
        if (jumpButton.tap && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        if (jumpButton.pressed && !isGrounded && rb.velocity.y < 0f && !smash)
        {
            glideMultiplier = glideDrag;
            isGliding = true;
        }
        else if (!jumpButton.pressed)
        {
            ResetGlide();
        }

        //Dash
        if (dashButton.tap)
            Dash();

        if (smashButton.tap)
        {
            ++tapNum;
        }
        if (tapNum > 0)
            doubleTapCount -= Time.deltaTime;

        if (doubleTapCount <= 0f)
        {
            ResetSmash();
        }
        else if (tapNum >= 2)
        {
            smash = true;
            ResetGlide();
            rb.gravityScale = 10f;
            ResetSmash();
        }

        //PC
        //if (Input.GetButtonDown("Jump"))
        //    Jump();
        //if (Input.GetButtonUp("Jump"))
        //    rb.drag = 0f;
        //if (Input.GetKeyDown(KeyCode.E))
        //    Dash();

        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    smash = true;
        //}

        if (smash)
        {
            Vector2 center = collider.bounds.center - new Vector3(0f, collider.bounds.extents.y, 0f);
            Collider2D[] results = Physics2D.OverlapBoxAll(center, smashBox, 0f, enemiesLayer);
            foreach (Collider2D result in results)
                Destroy(result.gameObject);

            results = Physics2D.OverlapBoxAll(center, stunBox, 0f, enemiesLayer);
            foreach (Collider2D result in results)
            {
                if (result.GetComponent<AI>() != null)
                    result.GetComponent<AI>().stun = true;
            }
        }

        if (isSlowedDown)
            slowDownCD -= Time.unscaledDeltaTime;
        if (slowDownCD <= 0f)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime *= 2f;
            slowDownCD = slowCDDuration;
            isSlowedDown = false;
            ResetDash();
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + 0.1f, groundLayer);

        Vector2 targetVel = rb.velocity;

        targetVel.x += xInput * moveSpeed;
        //targetVel.x += test.x * moveSpeed;
        targetVel.x *= 1f - dampForce;
        if (targetVel.y > 0f)
            targetVel.y *= 1f - dampForce;
        if (targetVel.y < 0f && isGliding)
            targetVel.y *= 1f - glideDrag;

        if (isDashing /*&& targetVel.magnitude <= 5f*/)
        {
            isSlowedDown = true;
            if(Time.timeScale != 0.5f)
                Time.fixedDeltaTime *= 0.5f;
            Time.timeScale = 0.5f;
        }

        rb.velocity = targetVel;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        Bounce bounce = collision.gameObject.GetComponent<Bounce>();

        if(bounce != null)
        {
            //Bounce Boost still continues after chain them and they jump on same object 
            //Check if collided object is below you && not bouncing on the same object twice
            if (contact.normal.y > 0f && bounceObject != collision.gameObject)
            {
                bounceObject = collision.gameObject;
                bounceBoost = Mathf.Min(bounceBoost + addBounce, maxBounce);
            }

            rb.AddForce(Vector2.up * bounceBoost, ForceMode2D.Impulse);
        }
        else
        {
            bounceBoost = 0f;
        }

        if(contact.normal.y > 0f)
        {
            smash = false;
            rb.gravityScale = 2f;
        }
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

        //PC
        //Move Right
        //if (test.x > 0.5f)
        //    direction.x = 1f;
        //else if (test.x < -0.5f)
        //    direction.x = -1f;

        //if (test.y > 0.5f)
        //    direction.y = 1f;
        //else if (test.y < -0.5f)
        //    direction.y = -1f;

        rb.velocity = direction * dashSpeed;
    }

    private void ResetSmash()
    {
        doubleTapCount = 0.2f;
        tapNum = 0;
    }

    private void ResetGlide()
    {
        isGliding = false;
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
