using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable : Respawnable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Level currentLevel = LevelManager.instance.CurrentLevel;
            ++currentLevel.numCollected;
            ObjectReferences.instance.itemCount.text = currentLevel.numCollected.ToString() + "/" + currentLevel.numToCollect.ToString();
            Gone();
        }
    }
}
