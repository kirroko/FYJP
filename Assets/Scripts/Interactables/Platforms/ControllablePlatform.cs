using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
* This fplatform is used by the green color
* 
* If the player is green and is standing on it, player can move the platform as by using the left joystick but they cannot 
* move the player itself
* 
* If player is anyother color than green, it will act as a normal platform
*/
public class ControllablePlatform : MonoBehaviour
{
    ///The Bounds of where the player can control it 
    [SerializeField] private Vector2 bounds = Vector2.zero;
    [SerializeField] private Vector2 offset = Vector2.zero;

    private Vector2 halfBounds = Vector2.zero;
    private Vector3 startPos = Vector3.zero;
    private Vector2 halfOffset = Vector2.zero;

    private void Start()
    {
        EventManager.instance.resetPlatformsEvent -= ResetPlatformEvent;
        EventManager.instance.resetPlatformsEvent += ResetPlatformEvent;


        startPos = transform.position;
        halfBounds = bounds * 0.5f;
        halfOffset = offset * 0.5f;
    }

    private void Update()
    {
        Vector3 targetPos = transform.position;

        if (targetPos.x > startPos.x + halfBounds.x + halfOffset.x)
            targetPos.x = startPos.x + halfBounds.x + halfOffset.x;
        else if (targetPos.x < startPos.x - halfBounds.x - halfOffset.x)
            targetPos.x = startPos.x - halfBounds.x - halfOffset.x;

        if (targetPos.y > startPos.y + halfBounds.y + halfOffset.y)
            targetPos.y = startPos.y + halfBounds.y + halfOffset.y;
        else if (targetPos.y < startPos.y - halfBounds.y - halfOffset.y)
            targetPos.y = startPos.y - halfBounds.y - halfOffset.y;

        transform.position = targetPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        //Check if collided object is below 
        if (contact.normal.y < 0f && collision.gameObject.GetComponent<PlayerInfo>())
        {
            EventManager.instance.TriggerSetPlatform(gameObject, COLORS.GREEN);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        //Check if collided object is below 
        if (contact.normal.y < 0f && collision.gameObject.GetComponent<PlayerInfo>())
        {
            EventManager.instance.TriggerSetPlatform(gameObject, COLORS.GREEN);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        EventManager.instance.TriggerSetPlatform(null, COLORS.GREEN);
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
                EventManager.instance.TriggerSetPlatform(gameObject, COLORS.GREEN);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        collision.GetContacts(contacts);

        foreach (ContactPoint2D contact in contacts)
        {
            //Check if collided object is below && is player
            if (contact.normal.y > 0f && collision.gameObject.GetComponent<PlayerInfo>())
            {
                EventManager.instance.TriggerSetPlatform(gameObject, COLORS.GREEN);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.instance.TriggerSetPlatform(null, COLORS.GREEN);
    }

    private void OnDestroy()
    {
        EventManager.instance.resetPlatformsEvent -= ResetPlatformEvent;
    }
    private void ResetPlatformEvent()
    {
        transform.position = startPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 pos = transform.position;
        pos.x += offset.x;
        pos.y += offset.y;

        Gizmos.DrawWireCube(pos, new Vector3(bounds.x, bounds.y));
    }
}
