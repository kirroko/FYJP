using UnityEngine;

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

    [Header("Bouncing")]
    [SerializeField] private float maxBounce = 4f;
    [SerializeField] private float addBounce = 0.5f;

    [Header("Gliding")]
    [SerializeField] private float glideDrag = 3f;

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

    //Bouncing
    private GameObject bounceObject = null;
    private float bounceBoost = 1f;

    //Smash
    private HoldButton smashButton = null;
    private bool smash = false;
    private float doubleTapCount = 0.2f;
    private int tapNum = 0;

    private Vector2 test;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        smashButton = jumpButton;
    }

    private void Update()
    {
        //xInput = input.Horizontal;
        //xInput = Input.GetAxisRaw("Horizontal");
        test = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (test.x != 0f)
            direction.x = test.x;
        //if (!isGrounded)
        //    xInput *= 0.5f;

        //if (xInput != 0f)
        //    direction.x = xInput;

        ////Jumping
        //if(jumpButton.tap && isGrounded)
        //{
        //    rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        //}
        //if (jumpButton.pressed && !isGrounded && rb.velocity.y < 0f && !smash)
        //    rb.drag = glideDrag;
        //else if (!jumpButton.pressed)
        //    rb.drag = 0f;

        ////Dash
        //if (dashButton.tap)
        //    Dash();

        //if(smashButton.tap)
        //{
        //    ++tapNum;
        //}
        //if (tapNum > 0)
        //    doubleTapCount -= Time.deltaTime;

        //if(doubleTapCount <= 0f)
        //{
        //    ResetSmash();
        //}
        //else if(tapNum >= 2)
        //{
        //    smash = true;
        //    rb.drag = 0f;
        //    rb.gravityScale = 10f;
        //    ResetSmash();
        //}

        //PC
        //if (Input.GetButtonDown("Jump"))
        //    Jump();
        //if (Input.GetButtonUp("Jump"))
        //    rb.drag = 0f;
        if (Input.GetKeyDown(KeyCode.E))
            Dash();

        //if(Input.GetKeyDown(KeyCode.P))
        //{
        //    smash = true;
        //}

        //if (smash)
        //{
        //    Vector2 center = collider.bounds.center - new Vector3(0f, collider.bounds.extents.y, 0f);
        //    Collider2D[] results =  Physics2D.OverlapBoxAll(center, smashBox, 0f, enemiesLayer);
        //    foreach (Collider2D result in results)
        //        Destroy(result.gameObject);

        //    results =  Physics2D.OverlapBoxAll(center, stunBox, 0f, enemiesLayer);
        //    foreach (Collider2D result in results)
        //    {
        //        if (result.GetComponent<AI>() != null)
        //            result.GetComponent<AI>().stun = true;
        //    }
        //}
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(collider.bounds.center, Vector2.down, collider.bounds.extents.y + 0.1f, groundLayer);

        Vector2 targetVelocity = rb.velocity + new Vector2(test.x * moveSpeed, rb.velocity.y);
        Vector2 temp = Vector2.zero;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref temp, dampForce);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    public void Dash()
    {
        Vector2 direction = Vector3.zero;

        ////Move Right
        //if (input.Direction.x > 0.5f)
        //    direction.x = 1f;
        //else if (input.Direction.x < -0.5f)
        //    direction.x = -1f;

        //if (input.Direction.y > 0.5f)
        //    direction.y = 1f;
        //else if (input.Direction.y < -0.5f)
        //    direction.y = -1f;
        //Move Right
        if (test.x > 0.5f)
            direction.x = 1f;
        else if (test.x < -0.5f)
            direction.x = -1f;

        if (test.y > 0.5f)
            direction.y = 1f;
        else if (test.y < -0.5f)
            direction.y = -1f;

        rb.AddForce(direction * dashSpeed, ForceMode2D.Impulse);

        //PC
        //rb.AddForce(new Vector2(dashSpeed * direction.x, 0f), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        Bounce bounce = collision.gameObject.GetComponent<Bounce>();

        if(bounce != null)
        {
            //Once Player Jump on same object the bounce boost chain is broken
            //Check if collided object is below you && not bouncing on the same object twice
            //if (contact.normal.y > 0f && bounceObject != collision.gameObject)
            //{
            //    bounceObject = collision.gameObject;
            //    bounceBoost = Mathf.Min(bounceBoost + addBounce, maxBounce);

            //    rb.AddForce(Vector2.up * bounceBoost, ForceMode2D.Impulse);
            //}
            //else
            //{
            //  bounceBoost = 0f;
            //}

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
    
    private void ResetSmash()
    {
        doubleTapCount = 0.2f;
        tapNum = 0;
    }
}
