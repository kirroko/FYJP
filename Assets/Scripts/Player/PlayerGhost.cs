using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private List<Vector3> pos = new List<Vector3>();

    private int index = 0;


    public void Init(List<Vector3> pos)
    {
        this.pos = pos;
        this.pos.RemoveAt(0);
    }

    private void Update()
    {
        if (index >= pos.Count)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPos = pos[index];
        float dist = (targetPos - transform.position).magnitude;

        if (dist <= 0.1f)
            ++index;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
