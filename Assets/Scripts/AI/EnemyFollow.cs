using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float detectionDistance = 5f;

    private Transform target = null;
    
    private void Start()
    {
        target = ObjectReferences.instance.player.transform;
    }

    private void Update()
    {
        if(target != null)
        {
            if (Vector2.Distance(transform.position, target.position) < detectionDistance)
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            else if (Vector2.Distance(transform.position, target.position) < 10)
                transform.Translate(-Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            target = ObjectReferences.instance.player.transform;
        }


    }
}
