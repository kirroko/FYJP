using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.instance.EndLevel();
    }
}
