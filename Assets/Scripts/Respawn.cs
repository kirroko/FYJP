using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    private static GameObject player = null;
    private static Vector3 startPos = Vector3.zero;

    private void Start()
    {
        player = ObjectReferences.instance.player;
        startPos = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerInfo>() != null)
            collision.transform.position = startPos;
    }

    public static void SendToSpawn()
    {
        player.transform.position = startPos;
    }
}
