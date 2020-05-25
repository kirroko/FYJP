using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoatsSway : Respawnable
{
    //[SerializeField] private float maxTilt = 25f;
    [SerializeField] private Vector2 overlapSize = Vector2.zero;
    [SerializeField] private Vector2 overlapOffset = Vector2.zero;

    private float startX = 0f;
    private Vector2 startCenterMass = Vector2.zero;
    private Rigidbody2D rb = null;
    private BuoyancyEffector2D effector2D = null;

    private string[] layerNames = new string[3];


    private void Start()
    {
        startX = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        startCenterMass = rb.centerOfMass;
        effector2D = GetComponentInParent<BuoyancyEffector2D>();

        layerNames[0] = "Platforms";
        layerNames[1] = "Enemy";
        layerNames[2] = "Player";
    }

    private void Update()
    {
        Vector3 targetPos = transform.position;
        targetPos.x = startX;
        transform.position = targetPos;

        //rb.rotation = Mathf.Clamp(rb.rotation, -maxTilt, maxTilt);

        float averageX = 0f;
        List<ContactPoint2D> contactPoints = new List<ContactPoint2D>();
        int count = rb.GetContacts(contactPoints);

        if (count == 0)
        {
            rb.centerOfMass = startCenterMass;
            return;
        }

        foreach (ContactPoint2D contactPoint in contactPoints)
        {
            averageX += contactPoint.otherCollider.gameObject.transform.localPosition.x;
        }
        averageX /= -count;

        rb.centerOfMass = new Vector2(averageX, rb.centerOfMass.y);

    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.x += overlapOffset.x;
        pos.y += overlapOffset.y;

        Collider2D[] results = Physics2D.OverlapBoxAll(pos, overlapSize, 0f);

        bool gotPlayer = false;
        foreach (Collider2D result in results)
        {
            if (result.GetComponent<PlayerInfo>() == null) continue;

            gotPlayer = true;
        }

        if(gotPlayer)
        {
            effector2D.colliderMask = LayerMask.GetMask(layerNames);
        }
        else
        {
            string[] masks = new string[2];
            masks[0] = layerNames[0];
            masks[1] = layerNames[1];
            effector2D.colliderMask = LayerMask.GetMask(masks);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 pos = transform.position;
        pos.x += overlapOffset.x;
        pos.y += overlapOffset.y;

        Gizmos.DrawWireCube(pos, overlapSize);
    }

    //private void LateUpdate()
    //{
    //    Vector3 rotation = transform.localEulerAngles;

    //    if (rotation.z > 300f)
    //        rotation.z = Mathf.Clamp(rotation.z, 360f - maxTilt, 360f);
    //    else if (rotation.z < 100f)
    //        rotation.z = Mathf.Clamp(rotation.z, 0f, maxTilt);

    //    transform.localRotation = Quaternion.Euler(rotation);
    //}

    protected override void TriggerRespawnAllEvent()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    protected override void TriggerRespawnEvent()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}

