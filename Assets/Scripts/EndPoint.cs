using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is the End Point of the level
 * 
 * After Player Trigger this, they will be sent to the result screen
 */ 
public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Close");
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Exit");
        StartCoroutine(DelaySceneChange(0.6f));
    }

    private IEnumerator DelaySceneChange(float time)
    {
        yield return new WaitForSeconds(time);
        LevelManager.instance.EndLevel(true);
    }
}
