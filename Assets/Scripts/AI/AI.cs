using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Assertions.Must;

public class AI : Respawnable
{
    public bool IsStunned
    {
        get
        {
            return stun;
        }

        set
        {
            stun = value;
            if (stun)
            {
                sr.color = Color.yellow;
                stunCD = stunDuration;
            }
        }
    }

    public bool IsTagged
    {
        get
        {
            return tagged;
        }

        set
        {
            tagged = value;
            if (tagged)
            {
                // tint here
                sr.color = Color.red;
                taggedCD = taggedDuration;
            }
        }
    }

    public bool IsFrozen
    {
        get
        {
            return frozen;
        }

        set
        {
            frozen = value;
            if (frozen)
            {
                sr.color = Color.blue;
                speed = moveSpeed * slowAmt;
                frozenCD = frozenDuration;
            }
        }
    }


    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float playerKnockbackAmt = 5f;
    [SerializeField] protected float stunDuration = 2f;
    [SerializeField] protected float taggedDuration = 2f;
    [SerializeField] protected float frozenDuration = 2f;
    [SerializeField] [Range(0f, 1f)] protected float slowAmt = 0.5f;

    protected bool stun = false;
    protected bool tagged = false;
    protected bool frozen = false;
    protected bool dead = false;
    protected float speed = 0f;

    private float stunCD = 0f;
    private float taggedCD = 0f;
    private float frozenCD = 0f;

    private SpriteRenderer sr;

    protected override void Start()
    {
        base.Start();

        speed = moveSpeed;
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        stunCD -= Time.deltaTime;
        taggedCD -= Time.deltaTime;
        frozenCD -= Time.deltaTime;

        if(stunCD <= 0f)
        {
            stun = false;
        }

        if (taggedCD <= 0f)
        {
            tagged = false;
        }

        if (frozenCD <= 0f)
        {
            frozen = false;
            speed = moveSpeed;
        }

        if (stunCD <= 0f && taggedCD <= 0f && frozenCD <= 0f)
            sr.color = Color.white;
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        PlayerInfo player = collision.gameObject.GetComponent<PlayerInfo>();
        if (player != null)
        {
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            PlayerColor color = player.GetComponent<PlayerColor>();

            if(!tagged)
            {
                if (color.GetCurrentColor.GetMain != COLORS.PURPLE)//Not purple
                {
                    player.TakeDamage(damage, playerKnockbackAmt);
                }
                else if (!movement.StillDashing)//Purple but not dashing
                {
                    player.TakeDamage(damage, playerKnockbackAmt);
                }
            }
            else
            {
                if (color.GetCurrentColor.GetMain != COLORS.RED)//Not red
                {
                    player.TakeDamage(damage, playerKnockbackAmt);
                }
                else if (!movement.StillDashing)//Red but not dashing
                {
                    player.TakeDamage(damage, playerKnockbackAmt);
                }
            }
        }

        AI enemy = collision.gameObject.GetComponent<AI>();
        if(enemy != null)
            Physics2D.IgnoreCollision(collision.otherCollider, collision.collider);
    }

    public void Die()
    {
        //Gone();
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Animator>().SetTrigger("Death");
        StartCoroutine(DelayGone());
        
        Level currentLevel = LevelManager.instance.CurrentLevel;
        ++currentLevel.enemiesKilled;
        ObjectReferences.instance.numKilled.text = currentLevel.enemiesKilled.ToString() + "/" + currentLevel.numToKill.ToString();
    }

    private IEnumerator DelayGone()
    {
        yield return new WaitForSeconds(0.5f);
        Gone();
        dead = true;
    }

    protected override void TriggerRespawnEvent()
    {
        base.TriggerRespawnEvent();

        dead = false;
    }

    protected override void TriggerRespawnAllEvent()
    {
        base.TriggerRespawnAllEvent();

        dead = false;
    }
}
