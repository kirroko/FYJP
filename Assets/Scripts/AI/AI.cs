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
                stunCD = stunDuration;
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
                taggedCD = taggedDuration;
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

    protected virtual void Start()
    {
        speed = moveSpeed;
    }

    protected virtual void Update()
    {
        stunCD -= Time.deltaTime;
        taggedCD -= Time.deltaTime;
        frozenCD -= Time.deltaTime;

        if(stunCD <= 0f)
            stun = false;

        if (taggedCD <= 0f)
            tagged = false;

        if (frozenCD <= 0f)
        {
            frozen = false;
            speed = moveSpeed;
        }
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
                    LevelManager.instance.RestartLevel();
                else if (!movement.StillDashing)//Purple but not dashing
                    LevelManager.instance.RestartLevel();
            }
            else
            {
                if (color.GetCurrentColor.GetMain != COLORS.RED)//Not red
                    LevelManager.instance.RestartLevel();
                else if (!movement.StillDashing)//Red but not dashing
                    LevelManager.instance.RestartLevel();
            }
        }
    }
}
