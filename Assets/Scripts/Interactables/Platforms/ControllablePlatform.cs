using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllablePlatform : MonoBehaviour
{
    [SerializeField] private Vector2 bounds = Vector2.zero;
    [SerializeField] private Vector2 offset = Vector2.zero;

    private Vector2 halfBounds = Vector2.zero;
    private Vector3 startPos = Vector3.zero;
    private Vector2 halfOffset = Vector2.zero;

    private void Start()
    {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 pos = transform.position;
        pos.x += offset.x;
        pos.y += offset.y;

        Gizmos.DrawWireCube(pos, new Vector3(bounds.x, bounds.y));
    }
}
