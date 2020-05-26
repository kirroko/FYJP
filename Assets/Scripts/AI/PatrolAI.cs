using System.Collections;
using UnityEngine;

public class PatrolAI : AI
{
    [Header("BUllets")]
    [SerializeField] private Projectile bullet = null;
    [SerializeField] private float bulletSpeed = 5f;
    [Header("Settings")]
    [SerializeField] private bool shootable = false;
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float distanceBeforeRotate = 5f;

    private float shootingInterval = 0f;
    private float defaultSpeed = 0f;
    private Vector3 startPos = new Vector3();

    [SerializeField] private Vector2 dir = new Vector2(1f, 0f);
    private new Collider2D collider = null;

    protected override void Start()
    {
        base.Start();
        collider = GetComponent<Collider2D>();
        defaultSpeed = moveSpeed;
        startPos = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        shootingInterval -= Time.deltaTime;

        if (stun || dead) return;

        Vector3 origin = transform.position - new Vector3(0, collider.bounds.extents.y, 0);
        origin += new Vector3(collider.bounds.extents.x * dir.x, 0);
        Debug.DrawRay(origin, new Vector2(0, -1) * (collider.bounds.extents.y + 0.1f), Color.red);

        // If on platform it'll flip at the edge or reach max distance 
        if (!Physics2D.Raycast(collider.bounds.center + new Vector3(collider.bounds.extents.x * dir.x, 0f, 0f), Vector2.down, collider.bounds.extents.y + 0.1f) ||
            Vector3.Distance(startPos, transform.position) > distanceBeforeRotate)
        {
            dir.x = -dir.x;
            sr.flipX = !sr.flipX;
            if(Vector3.Distance(startPos,transform.position) > distanceBeforeRotate)
            {
                Vector3 temp = transform.position;
                temp.x += dir.x;
                transform.position = temp;
            }
        }

        Vector3 targetPos = transform.position;

        targetPos.x += dir.x * speed * Time.deltaTime;

        transform.position = targetPos;

        if(shootable)
        {
            if (shootingInterval <= 0f)
            {
                moveSpeed = 0f;
                gameObject.GetComponent<Animator>().SetBool("Walking", false);

                Vector3 firePoint = collider.bounds.center + new Vector3(collider.bounds.extents.x, 0f, 0f) * dir.x * 1.25f;

                gameObject.GetComponent<Animator>().SetTrigger("Shoot");
                StartCoroutine(DelayReset(3.15f));
                StartCoroutine(DelayShooting(1.0f, firePoint));
                shootingInterval = shootInterval + 3.15f;
            }
        }
       
    }

    private IEnumerator DelayReset(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.GetComponent<Animator>().SetBool("Walking", true);
        moveSpeed = defaultSpeed;
    }

    private IEnumerator DelayShooting(float time, Vector3 point)
    {
        yield return new WaitForSeconds(time);
        Projectile temp = Instantiate(bullet, point, Quaternion.identity);
        temp.Init(new Vector2(dir.x, 0f), bulletSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null) return;

        ContactPoint2D contact = collision.GetContact(0);
        if(contact.normal.x != 0f)
        {
            dir.x = -dir.x;
            sr.flipX = !sr.flipX;
        }
    }
}
