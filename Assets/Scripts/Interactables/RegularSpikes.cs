using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSpikes : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerInfo>() != null)
            LevelManager.instance.RestartLevel();
    }
}
