using UnityEngine;

public class DamagingPlatform : MonoBehaviour
{
    public bool IsDamaging {
        get
        {
            return damageOthers;
        }
        set
        {
            if (value)
                damageOthers = value;
            else
                dieDown = true;
        }
    }

    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private float dieDownDuration = 3f;

    private bool damageOthers = false;
    private bool dieDown = false;

    private float damageCD = 0f;
    private float dieDownCD = 0f;

    private void FixedUpdate()
    {
        damageCD -= Time.fixedDeltaTime;

        if(dieDown)
        {
            dieDownCD -= Time.fixedDeltaTime;
        }

        if (dieDownCD <= 0f)
        {
            damageOthers = false;
            dieDownCD = dieDownDuration;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!damageOthers) return;

        if(damageCD <= 0f)
        {
            Debug.Log("Dealing Damage");
            damageCD = damageInterval;
        }
    }
}
