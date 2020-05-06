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
    [SerializeField] private GameObject canvas = null;

    private bool isInvincible = false;

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
            isInvincible = false;
    }

    public void TakeDamage(int damage, float knockbackForce)
    {
        if (isInvincible) return;

        heart -= damage;
        isInvincible = true;
        invincibleInterval = 1.0f;

        rb.AddForce(new Vector2(-movement.GetLastXDir, 1f) * knockbackForce, ForceMode2D.Impulse);

        sr.material = whiteMat;

        if (heart <= 0)
        {
            GetComponent<Animator>().SetTrigger("Death");
            Invoke("ResetMaterial", .15f);
            canvas.SetActive(false);
            StartCoroutine(DelayRestartLevel(1.0f));
        }
        else
        {
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
        LevelManager.instance.RestartLevel();
    }

    public void GainHeart(int amt)
    {
        heart += amt;
    }
}
