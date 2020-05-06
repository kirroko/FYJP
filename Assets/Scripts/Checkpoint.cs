using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SwapSprites))]
public class Checkpoint : MonoBehaviour
{
    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        EventManager.instance.checkpointEvent -= CheckpointEvent;
        EventManager.instance.checkpointEvent += CheckpointEvent;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.instance.SpawnPoint = transform.position;
        EventManager.instance.TriggerCheckpointEvent(gameObject);
        GetComponent<SwapSprites>().Swap(1);
    }

    private void CheckpointEvent(GameObject me)
    {
        if (me == gameObject) return;
        GetComponent<SwapSprites>().Swap(0);
    }

    private void OnDestroy()
    {
        EventManager.instance.checkpointEvent -= CheckpointEvent;
    }
}
