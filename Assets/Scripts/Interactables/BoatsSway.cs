using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoatsSway : MonoBehaviour
{
    //[SerializeField] private float maxTilt = 25f;

    private float startX = 0f;
    private Vector2 startCenterMass = Vector2.zero;
    private Rigidbody2D rb = null;

    private void Start()
    {
        startX = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        startCenterMass = rb.centerOfMass;
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

    //private void LateUpdate()
    //{
    //    Vector3 rotation = transform.localEulerAngles;

    //    if (rotation.z > 300f)
    //        rotation.z = Mathf.Clamp(rotation.z, 360f - maxTilt, 360f);
    //    else if (rotation.z < 100f)
    //        rotation.z = Mathf.Clamp(rotation.z, 0f, maxTilt);

    //    transform.localRotation = Quaternion.Euler(rotation);
    //}
}

