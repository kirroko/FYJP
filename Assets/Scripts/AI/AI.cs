using UnityEngine;
using TMPro;

public class AI : MonoBehaviour
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
    [SerializeField] protected float stunDuration = 2f;
    [SerializeField] protected float taggedDuration = 2f;
    [SerializeField] protected float frozenDuration = 2f;
    [SerializeField] [Range(0f, 1f)] protected float slowAmt = 0.5f;

    protected bool stun = false;
    protected bool tagged = false;
    protected bool frozen = false;
    protected float speed = 0f;

    private float stunCD = 0f;
    private float taggedCD = 0f;
    private float frozenCD = 0f;

    private SpriteRenderer sr;

    protected virtual void Start()
    {
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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<PlayerInfo>().isInvincible) return;

            PlayerMovement movement = collision.gameObject.GetComponent<PlayerMovement>();
            PlayerColor color = collision.gameObject.GetComponent<PlayerColor>();

            if(!tagged)
            {
                if (color.GetCurrentColor.GetMain != COLORS.PURPLE)//Not purple
                {
                    LevelManager.instance.RestartLevel();
                    ObjectReferences.instance.debug.text = "Not tagged, not purple " + color.GetCurrentColor.GetMain;
                }
                else if (!movement.StillDashing)//Purple but not dashing
                {
                    LevelManager.instance.RestartLevel();
                    ObjectReferences.instance.debug.text = "Not tagged, purple but not dashing";
                }
            }
            else
            {
                if (color.GetCurrentColor.GetMain != COLORS.RED)//Not red
                {
                    LevelManager.instance.RestartLevel();
                    ObjectReferences.instance.debug.text = "Tagged, not red " + color.GetCurrentColor.GetMain;
                }
                else if (!movement.StillDashing)//Red but not dashing
                {
                    LevelManager.instance.RestartLevel();
                    ObjectReferences.instance.debug.text = "Tagged, red but not dashing";
                }
            }
        }
    }
}
