using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<Animator>().SetTrigger("Close");
        StartCoroutine(DelaySceneChange());
    }

    private IEnumerator DelaySceneChange()
    {
        yield return new WaitForSeconds(0.4f);
        LevelManager.instance.EndLevel(true);
    }
}
