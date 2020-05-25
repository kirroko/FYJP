using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectable : Respawnable
{
    public bool IsExtra { get { return extra; } }

    [SerializeField] private bool extra = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            AudioManager.PlaySFX("Collect", false);
            Level currentLevel = LevelManager.instance.CurrentLevel;
            ++currentLevel.numCollected;
            ObjectReferences.instance.itemCount.text = currentLevel.numCollected.ToString() + "/" + currentLevel.numToCollect.ToString();
            Gone();
        }
    }
}
