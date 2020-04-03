using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           ++LevelManager.instance.CurrentLevel.collectablesCount;
           Destroy(gameObject);
        }
    }
}
