using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    private float placeHolder;
    public int LevelIndex;
    private void OnTriggerStay2D(Collider2D collider)
    {
        
        if(collider.gameObject.tag == "Player")
        {
            Debug.Log("Player in range");
            if (Input.GetKey(KeyCode.E))
            {
                placeHolder += Time.deltaTime * 0.5f;  
                Debug.Log(placeHolder);
                Debug.Log(LevelIndex);
                if (placeHolder >= 1.0)
                    Debug.Log("nextLevel");
               //   SceneManager.LoadScene(Level1);
            }
        }
    }

    void Update()
    {

    }

}
