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

    public bool TookDamage { get { return tookDamage; } }

    [SerializeField] private int heart = 1;
    [SerializeField] private const float invincibleDuration = 3f;
    [SerializeField] private GameObject canvas = null;

    private bool isInvincible = false;
    private bool tookDamage = false;

    private PlayerMovement movement = null;
    private Rigidbody2D rb = null;

    private float invincibleInterval = 0f;

    private Material whiteMat;
    private Material defaultMat;
    private SpriteRenderer sr;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        whiteMat = Resources.Load("Material/Effect/WhiteShade", typeof(Material)) as Material;
        defaultMat = sr.material;
        canvas = ObjectReferences.instance.gameObject;
    }

    private void Update()
    {
        invincibleInterval -= Time.deltaTime;

        if (invincibleInterval <= 0f)
        {
            isInvincible = false;
            tookDamage = false;
        }
    }

    public void TakeDamage(int damage, float knockbackForce)
    {
        if (isInvincible) return;

        heart -= damage;
        isInvincible = true;
        tookDamage = true;
        invincibleInterval = 1.0f;

        rb.AddForce(new Vector2(-movement.GetLastXDir, 1f) * knockbackForce, ForceMode2D.Impulse);

        sr.material = whiteMat;

        if (heart <= 0)
        {
            AudioManager.PlaySFX("Hurt", false);
            AudioManager.PlaySFX("Death",false);
            GetComponent<Animator>().SetTrigger("Death");
            Invoke("ResetMaterial", .15f);
            EventManager.instance.TriggerResetJoystickEvent();
            IsInvincible = true;
            canvas.SetActive(false);
            StartCoroutine(DelayRestartLevel(1.0f));
        }
        else
        {
            AudioManager.PlaySFX("Hurt", false);
            GetComponent<Animator>().SetTrigger("Hurt");
            Invoke("ResetMaterial", .15f);
        }
    }

    private void ResetMaterial()
    {
        sr.material = defaultMat;
    }

    private IEnumerator DelayRestartLevel(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Animator>().SetTrigger("Reset");
        canvas.SetActive(true);
        EventManager.instance.TriggerResetJoystickEvent();
        LevelManager.instance.RestartFromCheckpoint();
        isInvincible = true;
        invincibleInterval = 1.5f;
    }

    public void GainHeart(int amt)
    {
        heart += amt;
    }
}
