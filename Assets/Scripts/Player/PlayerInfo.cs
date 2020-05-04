using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public bool IsInvincible
    {
        get { return isInvincible; }
        set
        {
            if (value)
            {
                isInvincible = true;
                invincibleInterval = invincibleDuration;
            }
        }
    }

    [SerializeField] private int heart = 1;
    [SerializeField] private float invincibleDuration = 3f;

    private bool isInvincible = false;

    private PlayerMovement movement = null;
    private Rigidbody2D rb = null;

    private float invincibleInterval = 0f;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        invincibleInterval -= Time.deltaTime;

        if (invincibleInterval <= 0f)
            isInvincible = false;
    }

    public void TakeDamage(int damage, float knockbackForce)
    {
        if (isInvincible) return;

        heart -= damage;
        isInvincible = true;
        invincibleInterval = 1f;

        rb.AddForce(new Vector2(-movement.GetLastXDir, 1f) * knockbackForce, ForceMode2D.Impulse);


        if (heart <= 0)
        {
            GetComponent<Animator>().SetTrigger("Death");
            // LevelManager.instance.RestartLevel();
        }
        else
            GetComponent<Animator>().SetTrigger("Hurt");
    }

    public void GainHeart(int amt)
    {
        heart += amt;
    }
}
