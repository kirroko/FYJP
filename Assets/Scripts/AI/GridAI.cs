using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridAI : MonoBehaviour
{
    [SerializeField] private int moveSpeed = 1;

    private TurnManager TM = null;

    private void Start()
    {
        TM = TurnManager.Instance;
    }

    private void Update()
    {
        if(TM.newTurn)
        {
            Vector3 targetPos = transform.position;
            targetPos.x += TM.gridSize * moveSpeed;
            transform.position = targetPos;
            TM.newTurn = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerInfo playerInfo = collision.gameObject.GetComponent<PlayerInfo>();

        if(playerInfo != null)
        {
            if (!GetComponent<MeshRenderer>().enabled)
                SceneManager.LoadScene(0);
            else
                Destroy(gameObject);
        }
    }
}
