using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* TThis Platform is used by the orange color
* 
* If the player is orange, the platform will damage enemies on it
* else it acts as a normal platform
*/
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        //Check if collided object is below 
        if (contact.normal.y < 0f && collision.gameObject.GetComponent<PlayerInfo>())
        {
            EventManager.instance.TriggerSetPlatform(gameObject, COLORS.ORANGE);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        EventManager.instance.TriggerSetPlatform(null, COLORS.ORANGE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        collision.GetContacts(contacts);

        foreach (ContactPoint2D contact in contacts)
        {
            //Check if collided object is below && is player
            if (contact.normal.y > 0f && collision.gameObject.GetComponent<PlayerInfo>())
            {
                EventManager.instance.TriggerSetPlatform(gameObject, COLORS.ORANGE);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.instance.TriggerSetPlatform(null, COLORS.ORANGE);
    }
}
