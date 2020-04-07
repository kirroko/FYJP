using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Restart();
    }

    public static void Restart()
    {
        LevelManager.instance.LoadLevel(LevelManager.instance.CurrentLevelIndex);

    }
}
