using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllablePlatform : MonoBehaviour
{
    [SerializeField] private Vector2 bounds = Vector2.zero;

    private Vector2 halfBounds = Vector2.zero;
    private Vector3 startPos = Vector3.zero;
    private Transform onPlatform = null;

    private void Start()
    {
        startPos = transform.position;
        halfBounds = bounds * 0.5f;
    }

    private void Update()
    {
        Vector3 targetPos = transform.position;

        if (targetPos.x > startPos.x + halfBounds.x)
            targetPos.x = startPos.x + halfBounds.x;
        else if (targetPos.x < startPos.x - halfBounds.x)
            targetPos.x = startPos.x - halfBounds.x;

        if (targetPos.y > startPos.y + halfBounds.y)
            targetPos.y = startPos.y + halfBounds.y;
        else if (targetPos.y < startPos.y - halfBounds.y)
            targetPos.y = startPos.y - halfBounds.y;

        transform.position = targetPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(startPos, new Vector3(bounds.x, bounds.y));
    }
}
